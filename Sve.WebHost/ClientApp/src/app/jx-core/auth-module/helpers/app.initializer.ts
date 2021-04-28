import { AuthenticationService } from "../services/authentication.service";

export function appInitializer(authenticationService: AuthenticationService) {
    return () => new Promise(resolve => {
        //console.log('refresh token on app start up')
        // attempt to refresh token on app start up to auto authenticate
        authenticationService.refreshJwtToken()
            .subscribe()
            .add(resolve);
    });
}