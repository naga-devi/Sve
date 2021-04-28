import { Inject, Injectable } from "@angular/core";
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpResponse,
  HttpErrorResponse,
} from "@angular/common/http";
import { NgxSpinnerService } from "ngx-spinner";
import { Observable, throwError } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { AuthenticationService } from "../services/authentication.service";
import { Router } from "@angular/router";
import { JxModuleConfig, JX_MODULE_CONFIG } from "../../_config";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    @Inject(JX_MODULE_CONFIG) private jxConfig: JxModuleConfig,
    private router: Router,
    private spinner: NgxSpinnerService,
    private authenticationService: AuthenticationService
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    this.spinner.show();

    return next.handle(request).pipe(
      map((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          this.spinner.hide();
        }
        return event;
      }),
      catchError((err: HttpErrorResponse) => {
        this.spinner.hide();
        //const started = Date.now();
        //const elapsed = Date.now() - started;
        //console.log(`Request for ${request.urlWithParams} failed after ${elapsed} ms.`);
        if (
          [401, 403].includes(err.status) &&
          this.authenticationService.currentUser
        ) {
          // auto logout if 401 or 403 response returned from api
          this.authenticationService.logout();
          this.router.navigate([this.jxConfig.loginUrl], {
            queryParams: { returnUrl: this.router.routerState.snapshot.url },
          });
        }
        //TODO
        // if (!environment.production) {
        //   console.error(err);
        // }
        const error = (err && err.error && err.error.message) || err.statusText;
        return throwError(error);
      })
    );
  }
}
