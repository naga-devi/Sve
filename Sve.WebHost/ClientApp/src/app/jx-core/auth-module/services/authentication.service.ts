import { Inject, Injectable } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable, of } from "rxjs";
import { finalize, map } from "rxjs/operators";

import { AppDBkeys } from "../../db-keys";
import { JxModuleConfig, JX_MODULE_CONFIG } from "../../_config";

@Injectable({ providedIn: "root" })
export class AuthenticationService {
  private userSubject: BehaviorSubject<any>;
  public user$: Observable<any>;

  private profileSubject: BehaviorSubject<any>;
  public profile$: Observable<any>;

  constructor(
    @Inject(JX_MODULE_CONFIG) private jxConfig: JxModuleConfig,
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient
  ) {
    this.userSubject = new BehaviorSubject<any>(null);
    this.user$ = this.userSubject.asObservable();

    this.profileSubject = new BehaviorSubject<any>(null);
    this.profile$ = this.profileSubject.asObservable();
  }

  public get currentUser() {
    return this.userSubject.value;
  }

  public get currentProfile() {
    return this.profileSubject.value;
  }

  public get accessToken(): string {
    const jsonuser = localStorage.getItem(this.jxConfig.authTokenKey);
    if (jsonuser) {
      const token = JSON.parse(jsonuser);
      return token.accessToken;
    }
    return null;
  }

  public get refreshToken(): string {
    const jsonuser = localStorage.getItem(this.jxConfig.authTokenKey);
    if (jsonuser) {
      const token = JSON.parse(jsonuser);
      return token.refreshToken;
    }
    return null;
  }

  get isLoggedIn(): boolean {
    return this.userSubject.value != null;
  }

  getCurrentUserProfile() {
    this.http
      .get<any>(`${this.jxConfig.baseApiUrl}iam/me`)
      .subscribe((response) => {
        this.profileSubject.next(response);
      });
  }

  login(username: string, password: string) {
    return this.http
      .post<any>(`${this.jxConfig.baseApiUrl}iam/token`, { username, password })
      .pipe(
        map((response: any) => {
          if (response.status === "success") {
            const user = response;
            this.userSubject.next(user);
            this.setLocalStorage(user);
            this.getCurrentUserProfile();
            this.startRefreshTokenTimer();
            return response;
          }
          return response;
        })
      );
  }

  logout() {
    this.http
      .post<unknown>(`${this.jxConfig.baseApiUrl}iam/logout`, {})
      .pipe(
        finalize(() => {
          this.clearLocalStorage();
          this.stopRefreshTokenTimer();
          this.userSubject.next(null);
          this.router.navigate([this.jxConfig.loginUrl]);
        })
      )
      .subscribe();
  }

  refreshJwtToken() {
    const refreshToken = this.refreshToken;
    if (!refreshToken) {
      this.clearLocalStorage();
      return of(null);
    }

    return this.http
      .post<any>(`${this.jxConfig.baseApiUrl}iam/refresh-token`, {
        refreshToken,
      })
      .pipe(
        map((user) => {
          this.userSubject.next(user);
          this.setLocalStorage(user);
          this.startRefreshTokenTimer();
          this.getCurrentUserProfile();
          return user;
        })
      );
  }

  setLocalStorage(x: any) {
    localStorage.setItem(this.jxConfig.authTokenKey, JSON.stringify(x));
  }

  clearLocalStorage() {
    localStorage.removeItem(this.jxConfig.authTokenKey);
  }

  public redirectIfAlreadyLoggedIn() {
    this.user$.subscribe((x) => {
      const jsonuser = localStorage.getItem(this.jxConfig.authTokenKey);
      const token = JSON.parse(jsonuser);
      if (x && token.accessToken && token.refreshToken) {
        const returnUrl = this.route.snapshot.queryParams["returnUrl"] || "";
        this.router.navigate([returnUrl]);
      }
    });
  }

  // helper methods

  private refreshTokenTimeout;

  private startRefreshTokenTimer() {
    // parse json object from base64 encoded jwt token
    const jwtToken = JSON.parse(
      atob(this.currentUser.accessToken.split(".")[1])
    );

    // set a timeout to refresh the token a minute before it expires
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - 60 * 1000;
    this.refreshTokenTimeout = setTimeout(
      () => this.refreshJwtToken().subscribe(),
      timeout
    );
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }

  ngOnDestroy(): void {}
}
