(window.webpackJsonp=window.webpackJsonp||[]).push([[0],{Q8dc:function(t,e,c){"use strict";c.d(e,"a",(function(){return y}));var a=c("fXoL"),s=c("F5nt"),i=(c("FFMg"),c("dNgK")),n=c("3Pt+"),r=c("XiUz"),o=c("ofXK"),l=c("kmnG"),u=c("qFsG"),d=c("bTqV"),b=c("Qu3c"),h=c("NFeN");function m(t,e){if(1&t){const t=a.dc();a.cc(0,"div"),a.cc(1,"bdi"),a.cc(2,"form",3),a.cc(3,"mat-form-field",4),a.cc(4,"mat-label"),a.Oc(5,"Quantity"),a.bc(),a.cc(6,"input",5),a.kc("keyup",(function(e){return a.Fc(t),a.oc().quantityChanged(e.target.value)})),a.bc(),a.bc(),a.bc(),a.bc(),a.bc()}if(2&t){const t=a.oc();a.Jb(2),a.vc("formGroup",t.form)}}function p(t,e){if(1&t){const t=a.dc();a.cc(0,"button",10),a.kc("click",(function(){a.Fc(t);const e=a.oc(2);return e.addToCart(e.product)})),a.cc(1,"mat-icon"),a.Oc(2,"shopping_cart"),a.bc(),a.bc()}}function v(t,e){if(1&t){const t=a.dc();a.cc(0,"button",11),a.kc("click",(function(){a.Fc(t);const e=a.oc(2);return e.openProductDialog(e.product)})),a.cc(1,"mat-icon"),a.Oc(2,"remove_red_eye"),a.bc(),a.bc()}}function g(t,e){if(1&t){const t=a.dc();a.cc(0,"div"),a.cc(1,"button",6),a.kc("click",(function(){a.Fc(t);const e=a.oc();return e.addToWishList(e.product)})),a.cc(2,"mat-icon"),a.Oc(3,"favorite"),a.bc(),a.bc(),a.Mc(4,p,3,0,"button",7),a.cc(5,"button",8),a.kc("click",(function(){a.Fc(t);const e=a.oc();return e.addToCompare(e.product)})),a.cc(6,"mat-icon"),a.Oc(7,"compare"),a.bc(),a.bc(),a.Mc(8,v,3,0,"button",9),a.bc()}if(2&t){const t=a.oc();a.Jb(4),a.vc("ngIf",(null==t.product?null:t.product.availibilityCount)>0),a.Jb(4),a.vc("ngIf","all"!=t.type)}}function f(t,e){1&t&&(a.cc(0,"div",12),a.Oc(1," Sorry, this item is unavailable. Please choose a different one.\n"),a.bc())}let y=(()=>{class t{constructor(t,e,c){this.appService=t,this.snackBar=e,this.fb=c,this.onOpenProductDialog=new a.r,this.onQuantityChange=new a.r,this.count=1,this.align="center center"}ngOnInit(){this.form=this.fb.group({quantity:null}),this.product&&this.product.cartCount>0&&(this.count=this.product.cartCount,this.form.patchValue({quantity:this.count})),this.layoutAlign()}layoutAlign(){this.align="all"==this.type?"space-between center":"wish"==this.type?"start center":"center center"}quantityChanged(t){this.count=parseInt(t,0),this.count<=this.product.availibilityCount?this.changeQuantity({productBaseId:this.product.productBaseId,productId:this.product.id,soldQuantity:this.count,total:this.count*this.product.newPrice}):this.snackBar.open("You can not choose more items than available. In stock "+this.product.availibilityCount+" items.","\xd7",{panelClass:"error",verticalPosition:"top",duration:3e3})}addToCompare(t){this.appService.addToCompare(t)}addToWishList(t){this.appService.addToWishList(t)}addToCart(t){if(null!=localStorage.getItem("cart")){const t=JSON.parse(localStorage.getItem("cart"));t&&t.length>0&&(this.appService.Data.cartList=t)}let e=this.appService.Data.cartList.filter(e=>e.id==t.id)[0];if(e){if(!(e.cartCount+this.count<=this.product.availibilityCount))return this.snackBar.open("You can not add more items than available. In stock "+this.product.availibilityCount+" items and you already added "+e.cartCount+" item to your cart","\xd7",{panelClass:"error",verticalPosition:"top",duration:5e3}),!1;t.cartCount=e.cartCount+this.count}else t.cartCount=this.count;this.form.patchValue({quantity:null}),this.appService.addToCart(t)}openProductDialog(t){this.onOpenProductDialog.emit(t)}changeQuantity(t){this.onQuantityChange.emit(t)}}return t.\u0275fac=function(e){return new(e||t)(a.Wb(s.a),a.Wb(i.b),a.Wb(n.d))},t.\u0275cmp=a.Qb({type:t,selectors:[["app-controls"]],inputs:{product:"product",type:"type"},outputs:{onOpenProductDialog:"onOpenProductDialog",onQuantityChange:"onQuantityChange"},decls:4,vars:4,consts:[["fxLayout","row","fxLayout.xs","column",1,"text-muted",3,"fxLayoutAlign"],[4,"ngIf"],["class","bg-warn p-1 mt-2",4,"ngIf"],["autocomplete","off",3,"formGroup"],["appearance","outline",1,"fw-500"],["matInput","","type","number","formControlName","quantity","placeholder","0","min","1",3,"keyup"],["mat-icon-button","","matTooltip","Add to wishlist",3,"click"],["mat-icon-button","","matTooltip","Add to cart",3,"click",4,"ngIf"],["mat-icon-button","","matTooltip","Add to compare",3,"click"],["mat-icon-button","","matTooltip","Quick view",3,"click",4,"ngIf"],["mat-icon-button","","matTooltip","Add to cart",3,"click"],["mat-icon-button","","matTooltip","Quick view",3,"click"],[1,"bg-warn","p-1","mt-2"]],template:function(t,e){1&t&&(a.cc(0,"div",0),a.Mc(1,m,7,1,"div",1),a.Mc(2,g,9,2,"div",1),a.bc(),a.Mc(3,f,2,0,"div",2)),2&t&&(a.vc("fxLayoutAlign",e.align),a.Jb(1),a.vc("ngIf",(null==e.product?null:e.product.availibilityCount)>0&&("all"==e.type||"wish"==e.type)),a.Jb(1),a.vc("ngIf","wish"!=e.type),a.Jb(1),a.vc("ngIf",0==(null==e.product?null:e.product.availibilityCount)&&"all"==e.type))},directives:[r.d,r.c,o.m,n.w,n.p,n.i,l.c,l.g,u.b,n.s,n.c,n.o,n.g,d.b,b.a,h.a],styles:[""]}),t})()},TZED:function(t,e,c){"use strict";c.d(e,"a",(function(){return l}));var a=c("fXoL"),s=c("XiUz"),i=c("ofXK"),n=c("NFeN");function r(t,e){if(1&t){const t=a.dc();a.cc(0,"mat-icon",4),a.kc("click",(function(){a.Fc(t);const c=e.index;return a.oc().rate(c)})),a.Oc(1),a.bc()}if(2&t){const t=e.$implicit;a.Jb(1),a.Pc(t)}}function o(t,e){if(1&t&&(a.cc(0,"p",5),a.Oc(1),a.bc()),2&t){const t=a.oc();a.Jb(1),a.Qc("",t.ratingsCount," ratings")}}let l=(()=>{class t{constructor(){this.showText=!0}ngDoCheck(){this.ratingsCount&&this.ratingsValue&&!this.avg&&this.calculateAvgValue()}rate(t){}calculateAvgValue(){switch(this.avg=this.ratingsValue/this.ratingsCount,!0){case this.avg>0&&this.avg<20:this.stars=["star_half","star_border","star_border","star_border","star_border"];break;case 20==this.avg:this.stars=["star","star_border","star_border","star_border","star_border"];break;case this.avg>20&&this.avg<40:this.stars=["star","star_half","star_border","star_border","star_border"];break;case 40==this.avg:this.stars=["star","star","star_border","star_border","star_border"];break;case this.avg>40&&this.avg<60:this.stars=["star","star","star_half","star_border","star_border"];break;case 60==this.avg:this.stars=["star","star","star","star_border","star_border"];break;case this.avg>60&&this.avg<80:this.stars=["star","star","star","star_half","star_border"];break;case 80==this.avg:this.stars=["star","star","star","star","star_border"];break;case this.avg>80&&this.avg<100:this.stars=["star","star","star","star","star_half"];break;case this.avg>=100:this.stars=["star","star","star","star","star"];break;default:this.stars=["star_border","star_border","star_border","star_border","star_border"]}}}return t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=a.Qb({type:t,selectors:[["app-rating"]],inputs:{ratingsCount:"ratingsCount",ratingsValue:"ratingsValue",direction:"direction",showText:"showText"},decls:4,vars:4,consts:[[3,"fxLayout","fxLayoutAlign"],[1,"ratings"],["class","mat-icon-xs",3,"click",4,"ngFor","ngForOf"],["class","ratings-count text-muted",4,"ngIf"],[1,"mat-icon-xs",3,"click"],[1,"ratings-count","text-muted"]],template:function(t,e){1&t&&(a.cc(0,"div",0),a.cc(1,"div",1),a.Mc(2,r,2,1,"mat-icon",2),a.bc(),a.Mc(3,o,2,1,"p",3),a.bc()),2&t&&(a.vc("fxLayout",e.direction)("fxLayoutAlign","row"==e.direction?"start center":"center end"),a.Jb(2),a.vc("ngForOf",e.stars),a.Jb(1),a.vc("ngIf",e.showText))},directives:[s.d,s.c,i.l,i.m,n.a],styles:[".ratings[_ngcontent-%COMP%]{color:#fbc02d}.ratings-count[_ngcontent-%COMP%]{margin-left:12px;font-weight:500}"]}),t})()},VAss:function(t,e,c){"use strict";function a(t){if(t.value&&!/[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$/.test(t.value))return{invalidEmail:!0}}function s(t,e){return c=>{let a=c.controls[e];if(c.controls[t].value!==a.value)return a.setErrors({mismatchedPasswords:!0})}}c.d(e,"a",(function(){return a})),c.d(e,"b",(function(){return s}))},ZokB:function(t,e,c){"use strict";c.d(e,"a",(function(){return r}));var a=c("0IaG"),s=c("fXoL"),i=c("XiUz"),n=c("bTqV");let r=(()=>{class t{constructor(t,e){this.dialogRef=t,this.data=e}ngOnInit(){}onConfirm(){this.dialogRef.close(!0)}onDismiss(){this.dialogRef.close(!1)}}return t.\u0275fac=function(e){return new(e||t)(s.Wb(a.h),s.Wb(a.a))},t.\u0275cmp=s.Qb({type:t,selectors:[["app-confirm-dialog"]],decls:11,vars:2,consts:[["mat-dialog-title",""],["mat-dialog-content",""],["mat-dialog-actions",""],["fxLayout","row","fxLayoutAlign","space-between center",1,"w-100"],["mat-raised-button","","color","warn",3,"click"],["mat-raised-button","","color","primary",3,"click"]],template:function(t,e){1&t&&(s.cc(0,"h1",0),s.Oc(1),s.bc(),s.cc(2,"div",1),s.cc(3,"p"),s.Oc(4),s.bc(),s.bc(),s.cc(5,"div",2),s.cc(6,"div",3),s.cc(7,"button",4),s.kc("click",(function(){return e.onDismiss()})),s.Oc(8,"No"),s.bc(),s.cc(9,"button",5),s.kc("click",(function(){return e.onConfirm()})),s.Oc(10,"Yes"),s.bc(),s.bc(),s.bc()),2&t&&(s.Jb(1),s.Pc(null==e.data?null:e.data.title),s.Jb(3),s.Pc(null==e.data?null:e.data.message))},directives:[a.i,a.f,a.d,i.d,i.c,n.b],styles:[""]}),t})()},od06:function(t,e,c){"use strict";c.d(e,"a",(function(){return g}));var a=c("0IaG"),s=c("F5nt"),i=(c("FFMg"),c("fXoL")),n=c("bTqV"),r=c("NFeN"),o=c("XiUz"),l=c("nhfI"),u=c("ofXK"),d=c("znSr"),b=c("TZED"),h=c("Qu3c"),m=c("Q8dc"),p=c("vBMV");function v(t,e){if(1&t&&(i.cc(0,"div",21),i.Xb(1,"img",22),i.Xb(2,"div",23),i.bc()),2&t){const t=e.$implicit;i.Jb(1),i.Kb("data-src",t.medium)}}let g=(()=>{class t{constructor(t,e,c){this.appService=t,this.dialogRef=e,this.product=c,this.config={}}ngOnInit(){}ngAfterViewInit(){this.config={slidesPerView:1,spaceBetween:0,keyboard:!0,navigation:!0,pagination:!1,grabCursor:!0,loop:!1,preloadImages:!1,lazy:!0,effect:"fade",fadeEffect:{crossFade:!0}}}close(){this.dialogRef.close()}}return t.\u0275fac=function(e){return new(e||t)(i.Wb(s.a),i.Wb(a.h),i.Wb(a.a))},t.\u0275cmp=i.Qb({type:t,selectors:[["app-product-dialog"]],decls:43,vars:16,consts:[[1,"close-btn-outer"],["mat-mini-fab","","color","warn",3,"click"],["mat-dialog-content",""],["fxLayout","row wrap"],["fxFlex","100","fxFlex.gt-sm","50"],[1,"swiper-container","h-100",3,"swiper"],[1,"swiper-wrapper"],["class","swiper-slide",4,"ngFor","ngForOf"],["mat-icon-button","",1,"swiper-button-prev","swipe-arrow"],["mat-icon-button","",1,"swiper-button-next","swipe-arrow"],["fxFlex","100","fxFlex.gt-sm","50","ngClass.gt-sm","px-2 m-0","ngClass.sm","mt-2","ngClass.xs","mt-2"],[1,"py-1","lh"],[1,"text-muted","fw-500"],[1,"py-1"],[3,"ratingsCount","ratingsValue","direction"],[1,"py-1","text-muted","lh"],["fxLayoutAlign","end center",1,"text-muted"],["mat-icon-button","","matTooltip","View full details",3,"mat-dialog-close"],[1,"divider"],[1,"mt-2","new-price"],[3,"product","type"],[1,"swiper-slide"],[1,"swiper-lazy"],[1,"swiper-lazy-preloader"]],template:function(t,e){var c;1&t&&(i.cc(0,"div",0),i.cc(1,"button",1),i.kc("click",(function(){return e.close()})),i.cc(2,"mat-icon"),i.Oc(3,"close"),i.bc(),i.bc(),i.bc(),i.cc(4,"div",2),i.cc(5,"div",3),i.cc(6,"div",4),i.cc(7,"div",5),i.cc(8,"div",6),i.Mc(9,v,3,1,"div",7),i.bc(),i.cc(10,"button",8),i.cc(11,"mat-icon"),i.Oc(12,"keyboard_arrow_left"),i.bc(),i.bc(),i.cc(13,"button",9),i.cc(14,"mat-icon"),i.Oc(15,"keyboard_arrow_right"),i.bc(),i.bc(),i.bc(),i.bc(),i.cc(16,"div",10),i.cc(17,"h2"),i.Oc(18),i.bc(),i.cc(19,"div",11),i.cc(20,"p"),i.cc(21,"span",12),i.Oc(22,"Category: "),i.bc(),i.cc(23,"span"),i.Oc(24),i.pc(25,"filterById"),i.bc(),i.bc(),i.cc(26,"p"),i.cc(27,"span",12),i.Oc(28,"Availibility: "),i.bc(),i.cc(29,"span"),i.Oc(30),i.bc(),i.bc(),i.bc(),i.cc(31,"div",13),i.Xb(32,"app-rating",14),i.bc(),i.cc(33,"p",15),i.Oc(34),i.bc(),i.cc(35,"div",16),i.cc(36,"button",17),i.cc(37,"mat-icon"),i.Oc(38,"arrow_forward"),i.bc(),i.bc(),i.bc(),i.Xb(39,"div",18),i.cc(40,"h2",19),i.Oc(41),i.bc(),i.Xb(42,"app-controls",20),i.bc(),i.bc(),i.bc()),2&t&&(i.Jb(7),i.vc("swiper",e.config),i.Jb(2),i.vc("ngForOf",e.product.images),i.Jb(9),i.Pc(e.product.name),i.Jb(6),i.Pc(null==(c=i.rc(25,13,e.appService.Data.categories,e.product.categoryId))?null:c.name),i.Jb(6),i.Pc(e.product.availibilityCount>0?"In stock":"Unavailable"),i.Jb(2),i.vc("ratingsCount",e.product.ratingsCount)("ratingsValue",e.product.ratingsValue)("direction","row"),i.Jb(2),i.Pc(e.product.description),i.Jb(2),i.vc("mat-dialog-close",e.product),i.Jb(5),i.Qc("$",e.product.newPrice,""),i.Jb(1),i.vc("product",e.product)("type","all"))},directives:[n.b,r.a,a.f,o.d,o.a,l.a,u.l,d.a,b.a,o.c,h.a,a.e,m.a],pipes:[p.a],styles:[".product-dialog .mat-dialog-container{overflow:visible!important}.product-dialog .mat-dialog-container .close-btn-outer{position:relative}.product-dialog .mat-dialog-container .close-btn-outer button{position:absolute;right:-44px;top:-44px}.product-dialog .mat-dialog-container .swiper-slide{text-align:center}.product-dialog .mat-dialog-container .swiper-slide img{max-width:100%}"],encapsulation:2}),t})()},vBMV:function(t,e,c){"use strict";c.d(e,"a",(function(){return s})),c.d(e,"b",(function(){return i}));var a=c("fXoL");let s=(()=>{class t{transform(t,e,c){return c?t.filter(t=>t[c]==e)[0]:t.filter(t=>t.id==e)[0]}}return t.\u0275fac=function(e){return new(e||t)},t.\u0275pipe=a.Vb({name:"filterById",type:t,pure:!0}),t})(),i=(()=>{class t{transform(t,e,c){return t&&e&&c?t.filter(t=>t[e]==c):[]}}return t.\u0275fac=function(e){return new(e||t)},t.\u0275pipe=a.Vb({name:"filterByKey",type:t,pure:!0}),t})()},xur1:function(t,e,c){"use strict";c.r(e),c.d(e,"routes",(function(){return D})),c.d(e,"AnalyticsModule",(function(){return S}));var a=c("ofXK"),s=c("tyNb"),i=c("PCNd"),n=c("zQsl"),r=c("fXoL"),o=c("XiUz");const l=[{name:"Germany",series:[{name:"2017",value:71632},{name:"2010",value:40632},{name:"2000",value:76953},{name:"1990",value:31476}]},{name:"United States",series:[{name:"2017",value:82632},{name:"2010",value:49737},{name:"2000",value:55986},{name:"1990",value:37060}]},{name:"France",series:[{name:"2017",value:51732},{name:"2010",value:36745},{name:"2000",value:34774},{name:"1990",value:29476}]},{name:"United Kingdom",series:[{name:"2017",value:95652},{name:"2010",value:36240},{name:"2000",value:32543},{name:"1990",value:26424}]}],u=[{name:"Store 1",series:[{name:"Day 1",value:71632},{name:"Day 2",value:50632},{name:"Day 3",value:66953},{name:"Day 4",value:31476},{name:"Day 5",value:31632},{name:"Day 6",value:40632},{name:"Day 7",value:56953},{name:"Day 8",value:31476},{name:"Day 9",value:71632},{name:"Day 10",value:40632},{name:"Day 11",value:16953},{name:"Day 12",value:31476},{name:"Day 13",value:41632},{name:"Day 14",value:40632},{name:"Day 15",value:76953}]},{name:"Store 2",series:[{name:"Day 1",value:61632},{name:"Day 2",value:40632},{name:"Day 3",value:86953},{name:"Day 4",value:51476},{name:"Day 5",value:41632},{name:"Day 6",value:50632},{name:"Day 7",value:36953},{name:"Day 8",value:21476},{name:"Day 9",value:61632},{name:"Day 10",value:40632},{name:"Day 11",value:26953},{name:"Day 12",value:35476},{name:"Day 13",value:46632},{name:"Day 14",value:40632},{name:"Day 15",value:76953}]}],d=[{name:"Offline Stores",value:178},{name:"Digital Stores",value:340},{name:"Others",value:280}],b=[{name:"Product 1",value:178},{name:"Product 2",value:340},{name:"Product 3",value:280},{name:"Product 4",value:310},{name:"Product 5",value:385},{name:"Product 6",value:240}],h=[{name:"Store 1",value:40632},{name:"Store 2",value:49737},{name:"Store 3",value:36745},{name:"Store 4",value:36240},{name:"Store 5",value:33e3},{name:"Store 6",value:35800}],m=[{name:"Store 1",value:31632},{name:"Store 2",value:42737},{name:"Store 3",value:33745},{name:"Store 4",value:29240},{name:"Store 5",value:28250},{name:"Store 6",value:31800}];var p=c("Wp6s");let v=(()=>{class t{constructor(){this.showXAxis=!0,this.showYAxis=!0,this.gradient=!1,this.showLegend=!1,this.showXAxisLabel=!0,this.xAxisLabel="Country",this.showYAxisLabel=!0,this.yAxisLabel="Sales",this.colorScheme={domain:["#2F3E9E","#D22E2E","#378D3B","#0096A6","#F47B00","#606060"]},Object.assign(this,{sales_summary:l})}ngOnInit(){}onSelect(t){console.log(t)}}return t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=r.Qb({type:t,selectors:[["app-sales-summary"]],decls:7,vars:10,consts:[["fxLayoutAlign","center center"],[1,"w-100","h-300p"],[3,"scheme","results","gradient","xAxis","yAxis","legend","showXAxisLabel","showYAxisLabel","xAxisLabel","yAxisLabel","select"]],template:function(t,e){1&t&&(r.cc(0,"mat-card"),r.cc(1,"mat-card-header",0),r.cc(2,"mat-card-subtitle"),r.cc(3,"h2"),r.Oc(4,"Sales Summary (Top Regions)"),r.bc(),r.bc(),r.bc(),r.cc(5,"div",1),r.cc(6,"ngx-charts-bar-vertical-2d",2),r.kc("select",(function(t){return e.onSelect(t)})),r.bc(),r.bc(),r.bc()),2&t&&(r.Jb(6),r.vc("scheme",e.colorScheme)("results",e.sales_summary)("gradient",e.gradient)("xAxis",e.showXAxis)("yAxis",e.showYAxis)("legend",e.showLegend)("showXAxisLabel",e.showXAxisLabel)("showYAxisLabel",e.showYAxisLabel)("xAxisLabel",e.xAxisLabel)("yAxisLabel",e.yAxisLabel))},directives:[p.a,p.c,o.c,p.e,n.b],styles:[""]}),t})();const g=["resizedDiv"];let f=(()=>{class t{constructor(){this.showLegend=!1,this.gradient=!0,this.colorScheme={domain:["#2F3E9E","#D22E2E","#378D3B"]},this.showLabels=!0,this.explodeSlices=!1,this.doughnut=!1,this.previousWidthOfResizedDiv=0}ngOnInit(){this.data=d}onSelect(t){console.log(t)}ngAfterViewChecked(){this.previousWidthOfResizedDiv!=this.resizedDiv.nativeElement.clientWidth&&setTimeout(()=>this.data=[...d]),this.previousWidthOfResizedDiv=this.resizedDiv.nativeElement.clientWidth}}return t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=r.Qb({type:t,selectors:[["app-montly-sales"]],viewQuery:function(t,e){var c;1&t&&r.Tc(g,!0),2&t&&r.Bc(c=r.lc())&&(e.resizedDiv=c.first)},decls:9,vars:7,consts:[["fxLayoutAlign","center"],[1,"w-100","h-300p"],["resizedDiv",""],[3,"scheme","results","legend","explodeSlices","labels","doughnut","gradient","select"]],template:function(t,e){1&t&&(r.cc(0,"mat-card"),r.cc(1,"mat-card-header",0),r.cc(2,"mat-card-subtitle"),r.cc(3,"h2"),r.Oc(4,"Montly Sales"),r.bc(),r.bc(),r.bc(),r.cc(5,"mat-card-content"),r.cc(6,"div",1,2),r.cc(8,"ngx-charts-pie-chart",3),r.kc("select",(function(t){return e.onSelect(t)})),r.bc(),r.bc(),r.bc(),r.bc()),2&t&&(r.Jb(8),r.vc("scheme",e.colorScheme)("results",e.data)("legend",e.showLegend)("explodeSlices",e.explodeSlices)("labels",e.showLabels)("doughnut",e.doughnut)("gradient",e.gradient))},directives:[p.a,p.c,o.c,p.e,p.b,n.f],styles:[""]}),t})(),y=(()=>{class t{constructor(){this.showXAxis=!0,this.showYAxis=!0,this.gradient=!1,this.showLegend=!1,this.showXAxisLabel=!0,this.xAxisLabel="Days",this.showYAxisLabel=!0,this.yAxisLabel="Views",this.colorScheme={domain:["#2F3E9E","#D22E2E","#378D3B","#0096A6","#F47B00","#606060"]},this.autoScale=!0,Object.assign(this,{daily_views_stats:u})}onSelect(t){console.log(t)}}return t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=r.Qb({type:t,selectors:[["app-daily-views-stats"]],decls:7,vars:11,consts:[["fxLayoutAlign","center center"],[1,"w-100","h-400p"],[3,"scheme","results","gradient","xAxis","yAxis","legend","showXAxisLabel","showYAxisLabel","xAxisLabel","yAxisLabel","autoScale","select"]],template:function(t,e){1&t&&(r.cc(0,"mat-card"),r.cc(1,"mat-card-header",0),r.cc(2,"mat-card-subtitle"),r.cc(3,"h2"),r.Oc(4,"Daily Views Stats"),r.bc(),r.bc(),r.bc(),r.cc(5,"div",1),r.cc(6,"ngx-charts-line-chart",2),r.kc("select",(function(t){return e.onSelect(t)})),r.bc(),r.bc(),r.bc()),2&t&&(r.Jb(6),r.vc("scheme",e.colorScheme)("results",e.daily_views_stats)("gradient",e.gradient)("xAxis",e.showXAxis)("yAxis",e.showYAxis)("legend",e.showLegend)("showXAxisLabel",e.showXAxisLabel)("showYAxisLabel",e.showYAxisLabel)("xAxisLabel",e.xAxisLabel)("yAxisLabel",e.yAxisLabel)("autoScale",e.autoScale))},directives:[p.a,p.c,o.c,p.e,n.d],styles:[""]}),t})();const x=["resizedDiv"];let w=(()=>{class t{constructor(){this.showLegend=!1,this.gradient=!0,this.colorScheme={domain:["#2F3E9E","#D22E2E","#378D3B","#42A5F5","#7E57C2","#AFB42B"]},this.showLabels=!0,this.explodeSlices=!0,this.doughnut=!1,this.previousWidthOfResizedDiv=0}ngOnInit(){this.data=b}onSelect(t){console.log(t)}ngAfterViewChecked(){this.previousWidthOfResizedDiv!=this.resizedDiv.nativeElement.clientWidth&&setTimeout(()=>this.data=[...b]),this.previousWidthOfResizedDiv=this.resizedDiv.nativeElement.clientWidth}}return t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=r.Qb({type:t,selectors:[["app-most-viewed-products"]],viewQuery:function(t,e){var c;1&t&&r.Tc(x,!0),2&t&&r.Bc(c=r.lc())&&(e.resizedDiv=c.first)},decls:9,vars:7,consts:[["fxLayoutAlign","center"],[1,"w-100","h-300p"],["resizedDiv",""],[3,"scheme","results","legend","explodeSlices","labels","doughnut","gradient","select"]],template:function(t,e){1&t&&(r.cc(0,"mat-card"),r.cc(1,"mat-card-header",0),r.cc(2,"mat-card-subtitle"),r.cc(3,"h2"),r.Oc(4,"Most Viewed Products"),r.bc(),r.bc(),r.bc(),r.cc(5,"mat-card-content"),r.cc(6,"div",1,2),r.cc(8,"ngx-charts-pie-chart",3),r.kc("select",(function(t){return e.onSelect(t)})),r.bc(),r.bc(),r.bc(),r.bc()),2&t&&(r.Jb(8),r.vc("scheme",e.colorScheme)("results",e.data)("legend",e.showLegend)("explodeSlices",e.explodeSlices)("labels",e.showLabels)("doughnut",e.doughnut)("gradient",e.gradient))},directives:[p.a,p.c,o.c,p.e,p.b,n.f],styles:[""]}),t})(),A=(()=>{class t{constructor(){this.showXAxis=!0,this.showYAxis=!0,this.gradient=!1,this.showLegend=!1,this.showXAxisLabel=!0,this.xAxisLabel="Store",this.showYAxisLabel=!0,this.yAxisLabel="Transactions",this.colorScheme={domain:["#3F51B5","#E91E63","#43A047","#FDD835","#F4511E","#606060"]},Object.assign(this,{transactions:h})}onSelect(t){console.log(t)}}return t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=r.Qb({type:t,selectors:[["app-transactions"]],decls:7,vars:10,consts:[["fxLayoutAlign","center center"],[1,"w-100","h-300p"],[3,"scheme","results","gradient","xAxis","yAxis","legend","showXAxisLabel","showYAxisLabel","xAxisLabel","yAxisLabel","select"]],template:function(t,e){1&t&&(r.cc(0,"mat-card"),r.cc(1,"mat-card-header",0),r.cc(2,"mat-card-subtitle"),r.cc(3,"h2"),r.Oc(4,"Transactions"),r.bc(),r.bc(),r.bc(),r.cc(5,"div",1),r.cc(6,"ngx-charts-bar-horizontal",2),r.kc("select",(function(t){return e.onSelect(t)})),r.bc(),r.bc(),r.bc()),2&t&&(r.Jb(6),r.vc("scheme",e.colorScheme)("results",e.transactions)("gradient",e.gradient)("xAxis",e.showXAxis)("yAxis",e.showYAxis)("legend",e.showLegend)("showXAxisLabel",e.showXAxisLabel)("showYAxisLabel",e.showYAxisLabel)("xAxisLabel",e.yAxisLabel)("yAxisLabel",e.xAxisLabel))},directives:[p.a,p.c,o.c,p.e,n.a],styles:[""]}),t})(),L=(()=>{class t{constructor(){this.showLegend=!0,this.gradient=!0,this.colorScheme={domain:["#2F3E9E","#D22E2E","#378D3B","#0096A6","#F47B00","#606060"]},this.showLabels=!0,this.explodeSlices=!1,this.doughnut=!1,Object.assign(this,{refunds:m})}onSelect(t){console.log(t)}}return t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=r.Qb({type:t,selectors:[["app-refunds"]],decls:7,vars:2,consts:[["fxLayoutAlign","center center"],[1,"w-100","h-300p"],[3,"scheme","results","select"]],template:function(t,e){1&t&&(r.cc(0,"mat-card"),r.cc(1,"mat-card-header",0),r.cc(2,"mat-card-subtitle"),r.cc(3,"h2"),r.Oc(4,"Refunds by Stores"),r.bc(),r.bc(),r.bc(),r.cc(5,"div",1),r.cc(6,"ngx-charts-pie-grid",2),r.kc("select",(function(t){return e.onSelect(t)})),r.bc(),r.bc(),r.bc()),2&t&&(r.Jb(6),r.vc("scheme",e.colorScheme)("results",e.refunds))},directives:[p.a,p.c,o.c,p.e,n.g],styles:[""]}),t})();const D=[{path:"",component:(()=>{class t{constructor(){}ngOnInit(){}}return t.\u0275fac=function(e){return new(e||t)},t.\u0275cmp=r.Qb({type:t,selectors:[["app-analytics"]],decls:13,vars:0,consts:[["fxLayout","row wrap"],["fxFlex","100","fxFlex.gt-sm","60",1,"p-1"],["fxFlex","100","fxFlex.gt-sm","40",1,"p-1"],["fxFlex","100",1,"p-1"],["fxFlex","100","fxFlex.gt-sm","50",1,"p-1"]],template:function(t,e){1&t&&(r.cc(0,"div",0),r.cc(1,"div",1),r.Xb(2,"app-sales-summary"),r.bc(),r.cc(3,"div",2),r.Xb(4,"app-montly-sales"),r.bc(),r.cc(5,"div",3),r.Xb(6,"app-daily-views-stats"),r.bc(),r.cc(7,"div",4),r.Xb(8,"app-most-viewed-products"),r.bc(),r.cc(9,"div",4),r.Xb(10,"app-transactions"),r.bc(),r.cc(11,"div",3),r.Xb(12,"app-refunds"),r.bc(),r.bc())},directives:[o.d,o.a,v,f,y,w,A,L],styles:[""]}),t})(),pathMatch:"full"}];let S=(()=>{class t{}return t.\u0275mod=r.Ub({type:t}),t.\u0275inj=r.Tb({factory:function(e){return new(e||t)},imports:[[a.c,s.h.forChild(D),i.a,n.e]]}),t})()}}]);