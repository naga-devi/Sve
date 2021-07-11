(window.webpackJsonp=window.webpackJsonp||[]).push([[25],{b8Qw:function(r,o,e){"use strict";e.r(o),e.d(o,"routes",(function(){return M})),e.d(o,"SignInModule",(function(){return y}));var t=e("ofXK"),c=e("tyNb"),n=e("3Pt+"),i=e("PCNd"),s=e("VAss"),a=e("mIbC"),m=e("SxV6"),l=e("fXoL"),u=e("dNgK"),d=e("Wp6s"),b=e("XiUz"),f=e("kmnG"),p=e("qFsG"),g=e("bTqV"),h=e("NFeN"),v=e("znSr");function w(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Username is required"),l.bc())}function F(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Password is required"),l.bc())}function q(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Password isn't long enough, minimum of 6 characters"),l.bc())}function O(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Full Name is required"),l.bc())}function P(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Full Name isn't long enough, minimum of 3 characters"),l.bc())}function I(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Email is required"),l.bc())}function x(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Invalid email address"),l.bc())}function S(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Password is required"),l.bc())}function C(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Password isn't long enough, minimum of 6 characters"),l.bc())}function J(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Confirm Password is required"),l.bc())}function N(r,o){1&r&&(l.cc(0,"mat-error"),l.Oc(1,"Passwords do not match"),l.bc())}const M=[{path:"",component:(()=>{class r{constructor(r,o,e,t,c){this.formBuilder=r,this.router=o,this.route=e,this.authenticationService=t,this.snackBar=c,this.submitted=!1,this.hide=!0,this.loading=!1,this.error="",this.authenticationService.currentUser&&this.router.navigate(["/"])}ngOnInit(){this.returnUrl=this.route.snapshot.queryParams.returnUrl||"/",this.loginForm=this.formBuilder.group({email:["administrator",n.v.compose([n.v.required])],password:["123456",n.v.compose([n.v.required,n.v.minLength(6)])]}),this.registerForm=this.formBuilder.group({name:["",n.v.compose([n.v.required,n.v.minLength(3)])],email:["",n.v.compose([n.v.required,s.a])],password:["",n.v.required],confirmPassword:["",n.v.required]},{validator:Object(s.b)("password","confirmPassword")})}get f(){return this.loginForm.controls}onLoginFormSubmit(){this.submitted=!0,this.loginForm.valid&&(this.loading=!0,this.authenticationService.login(this.f.email.value,this.f.password.value).pipe(Object(m.a)()).subscribe({next:()=>{this.router.navigate([this.returnUrl])},error:r=>{this.error=r,this.loading=!1}}))}onRegisterFormSubmit(r){this.registerForm.valid&&this.snackBar.open("You registered successfully!","\xd7",{panelClass:"success",verticalPosition:"top",duration:3e3})}}return r.\u0275fac=function(o){return new(o||r)(l.Wb(n.d),l.Wb(c.e),l.Wb(c.a),l.Wb(a.AuthenticationService),l.Wb(u.b))},r.\u0275cmp=l.Qb({type:r,selectors:[["app-sign-in"]],decls:53,vars:13,consts:[["fxLayout","row wrap"],["fxFlex","100","fxFlex.gt-sm","50",1,"p-2"],[1,"text-muted","text-center"],[3,"formGroup","ngSubmit"],["appearance","outline",1,"w-100","mt-2"],["matInput","","placeholder","User name","formControlName","email","required",""],[4,"ngIf"],["appearance","outline",1,"w-100","mt-1"],["matInput","","type","password","placeholder","Password","formControlName","password","required",""],[1,"text-center","mt-2"],["mat-fab","","color","primary",1,"mat-elevation-z6"],["fxFlex","100","fxFlex.gt-sm","50","ngClass.sm","mt-2","ngClass.xs","mt-2",1,"p-2"],["matInput","","placeholder","Full Name","formControlName","name","required",""],["matInput","","placeholder","Email","formControlName","email","required",""],["matInput","","placeholder","Password","formControlName","password","type","password","minlength","6","required",""],["matInput","","placeholder","Confirm Password","formControlName","confirmPassword","type","password","required",""],["mat-fab","","color","primary",1,"mat-elevation-z6",3,"click"]],template:function(r,o){1&r&&(l.cc(0,"mat-card"),l.cc(1,"div",0),l.cc(2,"div",1),l.cc(3,"h2",2),l.Oc(4,"Sign In"),l.bc(),l.cc(5,"form",3),l.kc("ngSubmit",(function(){return o.onLoginFormSubmit()})),l.cc(6,"mat-form-field",4),l.cc(7,"mat-label"),l.Oc(8,"Username"),l.bc(),l.Xb(9,"input",5),l.Mc(10,w,2,0,"mat-error",6),l.bc(),l.cc(11,"mat-form-field",7),l.cc(12,"mat-label"),l.Oc(13,"Password"),l.bc(),l.Xb(14,"input",8),l.Mc(15,F,2,0,"mat-error",6),l.Mc(16,q,2,0,"mat-error",6),l.bc(),l.cc(17,"div",9),l.cc(18,"button",10),l.cc(19,"mat-icon"),l.Oc(20,"exit_to_app"),l.bc(),l.bc(),l.bc(),l.bc(),l.bc(),l.cc(21,"div",11),l.cc(22,"h2",2),l.Oc(23,"Don't have an account? Sign up now!"),l.bc(),l.cc(24,"form",3),l.kc("ngSubmit",(function(){return o.onRegisterFormSubmit(o.registerForm.value)})),l.cc(25,"mat-form-field",4),l.cc(26,"mat-label"),l.Oc(27,"Full Name"),l.bc(),l.Xb(28,"input",12),l.Mc(29,O,2,0,"mat-error",6),l.Mc(30,P,2,0,"mat-error",6),l.bc(),l.cc(31,"mat-form-field",7),l.cc(32,"mat-label"),l.Oc(33,"Email"),l.bc(),l.Xb(34,"input",13),l.Mc(35,I,2,0,"mat-error",6),l.Mc(36,x,2,0,"mat-error",6),l.bc(),l.cc(37,"mat-form-field",7),l.cc(38,"mat-label"),l.Oc(39,"Password"),l.bc(),l.Xb(40,"input",14),l.Mc(41,S,2,0,"mat-error",6),l.Mc(42,C,2,0,"mat-error",6),l.bc(),l.cc(43,"mat-form-field",7),l.cc(44,"mat-label"),l.Oc(45,"Confirm Password"),l.bc(),l.Xb(46,"input",15),l.Mc(47,J,2,0,"mat-error",6),l.Mc(48,N,2,0,"mat-error",6),l.bc(),l.cc(49,"div",9),l.cc(50,"button",16),l.kc("click",(function(){return o.onRegisterFormSubmit(o.registerForm.value)})),l.cc(51,"mat-icon"),l.Oc(52,"person_add"),l.bc(),l.bc(),l.bc(),l.bc(),l.bc(),l.bc(),l.bc()),2&r&&(l.Jb(5),l.vc("formGroup",o.loginForm),l.Jb(5),l.vc("ngIf",null==o.loginForm.controls.email.errors?null:o.loginForm.controls.email.errors.required),l.Jb(5),l.vc("ngIf",null==o.loginForm.controls.password.errors?null:o.loginForm.controls.password.errors.required),l.Jb(1),l.vc("ngIf",o.loginForm.controls.password.hasError("minlength")),l.Jb(8),l.vc("formGroup",o.registerForm),l.Jb(5),l.vc("ngIf",null==o.registerForm.controls.name.errors?null:o.registerForm.controls.name.errors.required),l.Jb(1),l.vc("ngIf",o.registerForm.controls.name.hasError("minlength")),l.Jb(5),l.vc("ngIf",null==o.registerForm.controls.email.errors?null:o.registerForm.controls.email.errors.required),l.Jb(1),l.vc("ngIf",o.registerForm.controls.email.hasError("invalidEmail")),l.Jb(5),l.vc("ngIf",null==o.registerForm.controls.password.errors?null:o.registerForm.controls.password.errors.required),l.Jb(1),l.vc("ngIf",o.registerForm.controls.password.hasError("minlength")),l.Jb(5),l.vc("ngIf",null==o.registerForm.controls.confirmPassword.errors?null:o.registerForm.controls.confirmPassword.errors.required),l.Jb(1),l.vc("ngIf",o.registerForm.controls.confirmPassword.hasError("mismatchedPasswords")))},directives:[d.a,b.d,b.a,n.w,n.p,n.i,f.c,f.g,p.b,n.c,n.o,n.g,n.u,t.m,g.b,h.a,v.a,n.k,f.b],styles:[".auth[_ngcontent-%COMP%]{white-space:nowrap;padding:7px 14px;font-weight:500}"]}),r})(),pathMatch:"full"}];let y=(()=>{class r{}return r.\u0275mod=l.Ub({type:r}),r.\u0275inj=l.Tb({factory:function(o){return new(o||r)},imports:[[t.c,c.h.forChild(M),n.t,i.a]]}),r})()}}]);