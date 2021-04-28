import { Inject, Injectable } from "@angular/core";
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from "@angular/router";
import { JxModuleConfig, JX_MODULE_CONFIG } from "../../_config";
import { AuthenticationService } from "../services/authentication.service";

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    @Inject(JX_MODULE_CONFIG) private jxConfig: JxModuleConfig,
    private router: Router,
    private authService: AuthenticationService
  ) {}

  // canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
  //     if (this.authenticationService.isLoggedIn) {
  //         // logged in so return true
  //         return true;
  //     } else {
  //         this.authenticationService.clearLocalStorage();
  //         // not logged in so redirect to login page with the return url
  //         this.router.navigate([this.jxConfig.loginUrl], { queryParams: { returnUrl: state.url } });
  //         return false;
  //     }
  // }

  canActivate(_route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const currentUser = this.authService.currentUser;
    if (currentUser) {
      // authorised so return true
      return true;
    }

    // not logged in so redirect to login page with the return url
    this.router.navigate([this.jxConfig.loginUrl], {
      queryParams: { returnUrl: state.url },
    });
    return false;
    // return this.authService.user$.pipe(
    //   map((user) => {
    //     if (user) {
    //       return true;
    //     } else {
    //       this.router.navigate([this.jxConfig.loginUrl], {
    //         queryParams: { returnUrl: state.url },
    //       });
    //       return false;
    //     }
    //   })
    // );
  }
}
