import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { emailValidator, matchingPasswords } from '../../theme/utils/app-validators';
import { AuthenticationService } from '../../jx-core';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-sign-in',
    templateUrl: './sign-in.component.html',
    styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
    loginForm: FormGroup;
    registerForm: FormGroup;
    returnUrl: string;
    submitted = false;
    hide = true;
    loading = false;
    error = '';

    constructor(
        public formBuilder: FormBuilder,
        public router: Router,
        private route: ActivatedRoute,
        private authenticationService: AuthenticationService,
        public snackBar: MatSnackBar) {
            //TODO
        // redirect to home if already logged in
        //this.authenticationService.redirectIfAlreadyLoggedIn();
        if (this.authenticationService.currentUser) { 
            this.router.navigate(['/']);
        }
    }

    ngOnInit() {
        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.loginForm = this.formBuilder.group({
            'email': ['administrator', Validators.compose([Validators.required])],
            'password': ['123456', Validators.compose([Validators.required, Validators.minLength(6)])]
        });

        this.registerForm = this.formBuilder.group({
            'name': ['', Validators.compose([Validators.required, Validators.minLength(3)])],
            'email': ['', Validators.compose([Validators.required, emailValidator])],
            'password': ['', Validators.required],
            'confirmPassword': ['', Validators.required]
        }, { validator: matchingPasswords('password', 'confirmPassword') });

    }

    get f() {
        return this.loginForm.controls;
    }

    public onLoginFormSubmit(): void {
        this.submitted = true;
        if (this.loginForm.valid) {
            this.loading = true;
            this.authenticationService.login(this.f.email.value, this.f.password.value)
                .pipe(first())
                .subscribe({
                    next: () => {
                        this.router.navigate([this.returnUrl]);
                    },
                    error: error => {
                        this.error = error;
                        this.loading = false;
                    }
                });
        }
    }

    public onRegisterFormSubmit(values: Object): void {
        if (this.registerForm.valid) {
            this.snackBar.open('You registered successfully!', '×', { panelClass: 'success', verticalPosition: 'top', duration: 3000 });
        }
    }
}
