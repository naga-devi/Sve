(window.webpackJsonp=window.webpackJsonp||[]).push([[11],{pBKm:function(c,e,t){"use strict";t.r(e),t.d(e,"routes",(function(){return F})),t.d(e,"DashboardModule",(function(){return C}));var a=t("ofXK"),i=t("tyNb"),n=t("PCNd"),s=t("zQsl"),r=t("fXoL"),o=t("XiUz"),l=t("Wp6s"),d=t("NFeN");let u=(()=>{class c{constructor(){}ngOnInit(){}}return c.\u0275fac=function(e){return new(e||c)},c.\u0275cmp=r.Qb({type:c,selectors:[["app-tiles"]],decls:49,vars:0,consts:[["fxLayout","row wrap"],["fxFlex","50","fxFlex.xl","16.6","fxFlex.lg","16.6","fxFlex.md","33.3","fxFlex.sm","33.3",1,"p-1"],[1,"gradient-green","tile","p-1"],[1,"gradient-red","tile","p-1"],[1,"gradient-orange","tile","p-1"],[1,"gradient-pink","tile","p-1"],[1,"gradient-gray","tile","p-1"],[1,"gradient-brown","tile","p-1"]],template:function(c,e){1&c&&(r.cc(0,"div",0),r.cc(1,"div",1),r.cc(2,"mat-card",2),r.cc(3,"mat-icon"),r.Oc(4,"monetization_on"),r.bc(),r.cc(5,"h2"),r.Oc(6,"$33500"),r.bc(),r.cc(7,"p"),r.Oc(8,"profit"),r.bc(),r.bc(),r.bc(),r.cc(9,"div",1),r.cc(10,"mat-card",3),r.cc(11,"mat-icon"),r.Oc(12,"thumb_up_alt"),r.bc(),r.cc(13,"h2"),r.Oc(14,"5300"),r.bc(),r.cc(15,"p"),r.Oc(16,"likes"),r.bc(),r.bc(),r.bc(),r.cc(17,"div",1),r.cc(18,"mat-card",4),r.cc(19,"mat-icon"),r.Oc(20,"group"),r.bc(),r.cc(21,"h2"),r.Oc(22,"14280"),r.bc(),r.cc(23,"p"),r.Oc(24,"users"),r.bc(),r.bc(),r.bc(),r.cc(25,"div",1),r.cc(26,"mat-card",5),r.cc(27,"mat-icon"),r.Oc(28,"shopping_cart"),r.bc(),r.cc(29,"h2"),r.Oc(30,"7520"),r.bc(),r.cc(31,"p"),r.Oc(32,"orders"),r.bc(),r.bc(),r.bc(),r.cc(33,"div",1),r.cc(34,"mat-card",6),r.cc(35,"mat-icon"),r.Oc(36,"pie_chart"),r.bc(),r.cc(37,"h2"),r.Oc(38,"$2700"),r.bc(),r.cc(39,"p"),r.Oc(40,"tax"),r.bc(),r.bc(),r.bc(),r.cc(41,"div",1),r.cc(42,"mat-card",7),r.cc(43,"mat-icon"),r.Oc(44,"save_alt"),r.bc(),r.cc(45,"h2"),r.Oc(46,"12700"),r.bc(),r.cc(47,"p"),r.Oc(48,"downloads"),r.bc(),r.bc(),r.bc(),r.bc())},directives:[o.d,o.a,l.a,d.a],styles:[".tile[_ngcontent-%COMP%]{text-align:right}.tile[_ngcontent-%COMP%]   .mat-icon[_ngcontent-%COMP%]{position:absolute;left:4px;top:50%;line-height:0;font-size:54px;opacity:.3}.tile[_ngcontent-%COMP%]   h2[_ngcontent-%COMP%]{font-weight:500}.tile[_ngcontent-%COMP%]   p[_ngcontent-%COMP%]{text-transform:uppercase}"]}),c})();const m=[{name:"Orders",series:[{name:"1980",value:21632}]}],b=[{name:"Product-1",value:69400},{name:"Product-2",value:59400},{name:"Product-3",value:82400},{name:"Product-4",value:73400},{name:"Product-5",value:25400},{name:"Product-6",value:23400},{name:"Product-7",value:49300},{name:"Product-8",value:55400},{name:"Product-9",value:37400},{name:"Product-10",value:65220},{name:"Product-11",value:79400},{name:"Product-12",value:58400},{name:"Product-13",value:41400},{name:"Product-14",value:37400},{name:"Product-15",value:33700},{name:"Product-16",value:42700},{name:"Product-17",value:52700},{name:"Product-18",value:62700}],v=[{name:"Customers",series:[{name:"2000",value:34502}]}],h=[{name:"Item-1",value:23701},{name:"Item-2",value:33701},{name:"Item-3",value:63701},{name:"Item-4",value:52701},{name:"Item-5",value:73701},{name:"Item-6",value:43701},{name:"Item-7",value:83701},{name:"Item-8",value:29701},{name:"Item-9",value:69701},{name:"Item-10",value:58701},{name:"Item-11",value:65701},{name:"Item-12",value:47701},{name:"Item-13",value:41701},{name:"Item-14",value:25701},{name:"Item-15",value:35701}],p=[{name:"Offline Stores",value:178},{name:"Digital Stores",value:340},{name:"Others",value:280}],f=[{name:"Company 1",series:[{name:"2010",value:31632},{name:"2011",value:42589},{name:"2012",value:52458},{name:"2013",value:69632},{name:"2014",value:52305},{name:"2015",value:72412},{name:"2016",value:66285},{name:"2017",value:49855}]},{name:"Company 2",series:[{name:"2010",value:61632},{name:"2011",value:68589},{name:"2012",value:55458},{name:"2013",value:62632},{name:"2014",value:38305},{name:"2015",value:41412},{name:"2016",value:32285},{name:"2017",value:31855}]},{name:"Company 3",series:[{name:"2010",value:55632},{name:"2011",value:63589},{name:"2012",value:70458},{name:"2013",value:79632},{name:"2014",value:59305},{name:"2015",value:56412},{name:"2016",value:49285},{name:"2017",value:38855}]}],g=["resizedDiv"];let x=(()=>{class c{constructor(){this.colorScheme={domain:["rgba(255,255,255,0.8)"]},this.autoScale=!0,this.previousWidthOfResizedDiv=0}ngOnInit(){this.orders=m,this.products=b,this.customers=v,this.refunds=h,this.orders=this.addRandomValue("orders"),this.customers=this.addRandomValue("customers")}onSelect(c){console.log(c)}addRandomValue(c){switch(c){case"orders":for(let c=1;c<30;c++)this.orders[0].series.push({name:1980+c,value:Math.ceil(1e6*Math.random())});return this.orders;case"customers":for(let c=1;c<15;c++)this.customers[0].series.push({name:2e3+c,value:Math.ceil(1e6*Math.random())});return this.customers;default:return this.orders}}ngOnDestroy(){this.orders[0].series.length=0,this.customers[0].series.length=0}ngAfterViewChecked(){this.previousWidthOfResizedDiv!=this.resizedDiv.nativeElement.clientWidth&&(setTimeout(()=>this.orders=[...m]),setTimeout(()=>this.products=[...b]),setTimeout(()=>this.customers=[...v]),setTimeout(()=>this.refunds=[...h])),this.previousWidthOfResizedDiv=this.resizedDiv.nativeElement.clientWidth}}return c.\u0275fac=function(e){return new(e||c)},c.\u0275cmp=r.Qb({type:c,selectors:[["app-info-cards"]],viewQuery:function(c,e){var t;1&c&&r.Tc(g,!0),2&c&&r.Bc(t=r.lc())&&(e.resizedDiv=t.first)},decls:50,vars:10,consts:[["fxLayout","row wrap"],["resizedDiv",""],["fxFlex","100","fxFlex.gt-sm","25","fxFlex.sm","50",1,"p-1"],[1,"p-0","gradient-purple"],["fxLayoutAlign","space-between center",1,"p-1"],["fxLayoutAlign","center center"],[1,"icon-sm","px-1"],[1,"w-100","h-100p"],[3,"scheme","results","select"],[1,"p-0","gradient-teal"],[3,"scheme","results","autoScale","select"],[1,"p-0","gradient-indigo"],[1,"p-0","gradient-amber"]],template:function(c,e){1&c&&(r.cc(0,"div",0,1),r.cc(2,"div",2),r.cc(3,"mat-card",3),r.cc(4,"div",4),r.cc(5,"p"),r.Oc(6,"Products profit"),r.bc(),r.cc(7,"div",5),r.cc(8,"mat-icon",6),r.Oc(9,"trending_up"),r.bc(),r.cc(10,"span"),r.Oc(11,"38%"),r.bc(),r.bc(),r.bc(),r.cc(12,"div",7),r.cc(13,"ngx-charts-bar-vertical",8),r.kc("select",(function(c){return e.onSelect(c)})),r.bc(),r.bc(),r.bc(),r.bc(),r.cc(14,"div",2),r.cc(15,"mat-card",9),r.cc(16,"div",4),r.cc(17,"p"),r.Oc(18,"Total orders"),r.bc(),r.cc(19,"div",5),r.cc(20,"mat-icon",6),r.Oc(21,"trending_up"),r.bc(),r.cc(22,"span"),r.Oc(23,"16%"),r.bc(),r.bc(),r.bc(),r.cc(24,"div",7),r.cc(25,"ngx-charts-line-chart",10),r.kc("select",(function(c){return e.onSelect(c)})),r.bc(),r.bc(),r.bc(),r.bc(),r.cc(26,"div",2),r.cc(27,"mat-card",11),r.cc(28,"div",4),r.cc(29,"p"),r.Oc(30,"Refunds"),r.bc(),r.cc(31,"div",5),r.cc(32,"mat-icon",6),r.Oc(33,"trending_down"),r.bc(),r.cc(34,"span"),r.Oc(35,"-11%"),r.bc(),r.bc(),r.bc(),r.cc(36,"div",7),r.cc(37,"ngx-charts-bar-vertical",8),r.kc("select",(function(c){return e.onSelect(c)})),r.bc(),r.bc(),r.bc(),r.bc(),r.cc(38,"div",2),r.cc(39,"mat-card",12),r.cc(40,"div",4),r.cc(41,"p"),r.Oc(42,"Customers"),r.bc(),r.cc(43,"div",5),r.cc(44,"mat-icon",6),r.Oc(45,"trending_up"),r.bc(),r.cc(46,"span"),r.Oc(47,"17%"),r.bc(),r.bc(),r.bc(),r.cc(48,"div",7),r.cc(49,"ngx-charts-line-chart",10),r.kc("select",(function(c){return e.onSelect(c)})),r.bc(),r.bc(),r.bc(),r.bc(),r.bc()),2&c&&(r.Jb(13),r.vc("scheme",e.colorScheme)("results",e.products),r.Jb(12),r.vc("scheme",e.colorScheme)("results",e.orders)("autoScale",e.autoScale),r.Jb(12),r.vc("scheme",e.colorScheme)("results",e.refunds),r.Jb(12),r.vc("scheme",e.colorScheme)("results",e.customers)("autoScale",e.autoScale))},directives:[o.d,o.a,l.a,o.c,d.a,s.c,s.d],styles:[".mat-chip.info-chip[_ngcontent-%COMP%]{padding:4px 8px}.mat-chip.info-chip[_ngcontent-%COMP%]   .mat-icon[_ngcontent-%COMP%]{margin-right:4px}.h-100p[_ngcontent-%COMP%]{height:100px}"]}),c})();var O=t("bSwM");const w=["resizedDiv"];let y=(()=>{class c{constructor(){this.showLegend=!1,this.gradient=!0,this.colorScheme={domain:["#2F3E9E","#D22E2E","#378D3B"]},this.showLabels=!0,this.explodeSlices=!0,this.doughnut=!1,this.previousWidthOfResizedDiv=0}ngOnInit(){this.data=p}onSelect(c){console.log(c)}ngAfterViewChecked(){this.previousWidthOfResizedDiv!=this.resizedDiv.nativeElement.clientWidth&&setTimeout(()=>this.data=[...p]),this.previousWidthOfResizedDiv=this.resizedDiv.nativeElement.clientWidth}}return c.\u0275fac=function(e){return new(e||c)},c.\u0275cmp=r.Qb({type:c,selectors:[["app-montly-sales"]],viewQuery:function(c,e){var t;1&c&&r.Tc(w,!0),2&c&&r.Bc(t=r.lc())&&(e.resizedDiv=t.first)},decls:14,vars:9,consts:[["fxLayoutAlign","center"],["fxLayout","row","fxLayoutAlign","space-around"],["color","primary",1,"example-margin",3,"checked","change"],[1,"w-100","h-300p"],["resizedDiv",""],[3,"scheme","results","legend","explodeSlices","labels","doughnut","gradient","select"]],template:function(c,e){1&c&&(r.cc(0,"mat-card"),r.cc(1,"mat-card-header",0),r.cc(2,"mat-card-subtitle"),r.cc(3,"h2"),r.Oc(4,"Montly Sales"),r.bc(),r.bc(),r.bc(),r.cc(5,"mat-card-content"),r.cc(6,"div",1),r.cc(7,"mat-checkbox",2),r.kc("change",(function(){return e.explodeSlices=!e.explodeSlices})),r.Oc(8,"Explode Slices"),r.bc(),r.cc(9,"mat-checkbox",2),r.kc("change",(function(){return e.showLabels=!e.showLabels})),r.Oc(10,"Show Labels"),r.bc(),r.bc(),r.cc(11,"div",3,4),r.cc(13,"ngx-charts-pie-chart",5),r.kc("select",(function(c){return e.onSelect(c)})),r.bc(),r.bc(),r.bc(),r.bc()),2&c&&(r.Jb(7),r.vc("checked",e.explodeSlices),r.Jb(2),r.vc("checked",e.showLabels),r.Jb(4),r.vc("scheme",e.colorScheme)("results",e.data)("legend",e.showLegend)("explodeSlices",e.explodeSlices)("labels",e.showLabels)("doughnut",e.doughnut)("gradient",e.gradient))},directives:[l.a,l.c,o.c,l.e,l.b,o.d,O.a,s.f],styles:[""]}),c})();var S=t("bTqV"),P=t("Qu3c");function L(c,e){1&c&&(r.cc(0,"button",11),r.cc(1,"mat-icon"),r.Oc(2,"receipt"),r.bc(),r.bc())}function D(c,e){if(1&c&&(r.cc(0,"div",5),r.cc(1,"div",6),r.cc(2,"span",7),r.Oc(3),r.bc(),r.bc(),r.cc(4,"div",6),r.Oc(5),r.bc(),r.cc(6,"div",6),r.Oc(7),r.bc(),r.cc(8,"div",6),r.Oc(9),r.bc(),r.cc(10,"div",6),r.cc(11,"div",8),r.cc(12,"button",9),r.cc(13,"mat-icon"),r.Oc(14,"remove_red_eye"),r.bc(),r.bc(),r.Mc(15,L,3,0,"button",10),r.bc(),r.bc(),r.bc()),2&c){const c=e.$implicit;r.Jb(3),r.Pc(c.number),r.Jb(2),r.Pc(c.date),r.Jb(2),r.Pc(c.status),r.Jb(2),r.Pc(c.total),r.Jb(6),r.vc("ngIf",c.invoice)}}let A=(()=>{class c{constructor(){this.orders=[{number:"#3258",date:"March 29, 2018",status:"Completed",total:"$140.00 for 2 items",invoice:!0},{number:"#3145",date:"February 14, 2018",status:"On hold",total:"$255.99 for 1 item",invoice:!1},{number:"#2972",date:"January 7, 2018",status:"Processing",total:"$255.99 for 1 item",invoice:!0},{number:"#2971",date:"January 5, 2018",status:"Completed",total:"$73.00 for 1 item",invoice:!0},{number:"#1981",date:"December 24, 2017",status:"Pending Payment",total:"$285.00 for 2 items",invoice:!1},{number:"#1781",date:"September 3, 2017",status:"Refunded",total:"$49.00 for 2 items",invoice:!1}]}ngOnInit(){}}return c.\u0275fac=function(e){return new(e||c)},c.\u0275cmp=r.Qb({type:c,selectors:[["app-latest-orders"]],decls:13,vars:1,consts:[[1,"p-0"],[1,"mat-table","admin-table","header-sm","truncated"],[1,"mat-header-row"],[1,"mat-header-cell"],["class","mat-row",4,"ngFor","ngForOf"],[1,"mat-row"],[1,"mat-cell"],[1,"order"],[1,"p-1","actions"],["mat-mini-fab","","color","primary","matTooltip","View"],["mat-mini-fab","","color","warn","matTooltip","View invoice","class","btn-invoice",4,"ngIf"],["mat-mini-fab","","color","warn","matTooltip","View invoice",1,"btn-invoice"]],template:function(c,e){1&c&&(r.cc(0,"mat-card",0),r.cc(1,"div",1),r.cc(2,"div",2),r.cc(3,"div",3),r.Oc(4,"Order"),r.bc(),r.cc(5,"div",3),r.Oc(6,"Date"),r.bc(),r.cc(7,"div",3),r.Oc(8,"Status"),r.bc(),r.cc(9,"div",3),r.Oc(10,"Total"),r.bc(),r.Xb(11,"div",3),r.bc(),r.Mc(12,D,16,5,"div",4),r.bc(),r.bc()),2&c&&(r.Jb(12),r.vc("ngForOf",e.orders))},directives:[l.a,a.l,S.b,P.a,d.a,a.m],styles:[""]}),c})();const z=["resizedDiv"];let _=(()=>{class c{constructor(){this.showXAxis=!0,this.showYAxis=!0,this.gradient=!1,this.showLegend=!1,this.showXAxisLabel=!1,this.xAxisLabel="Year",this.showYAxisLabel=!1,this.yAxisLabel="Profit",this.colorScheme={domain:["#283593","#039BE5","#FF5252"]},this.autoScale=!0,this.roundDomains=!0,this.previousWidthOfResizedDiv=0}ngOnInit(){this.analytics=f}onSelect(c){console.log(c)}ngAfterViewChecked(){this.previousWidthOfResizedDiv!=this.resizedDiv.nativeElement.clientWidth&&(this.analytics=[...f]),this.previousWidthOfResizedDiv=this.resizedDiv.nativeElement.clientWidth}}return c.\u0275fac=function(e){return new(e||c)},c.\u0275cmp=r.Qb({type:c,selectors:[["app-analytics"]],viewQuery:function(c,e){var t;1&c&&r.Tc(z,!0),2&c&&r.Bc(t=r.lc())&&(e.resizedDiv=t.first)},decls:9,vars:12,consts:[["fxLayoutAlign","center"],[1,"w-100","h-300p"],["resizedDiv",""],[3,"scheme","results","gradient","xAxis","yAxis","legend","showXAxisLabel","showYAxisLabel","xAxisLabel","yAxisLabel","autoScale","roundDomains","select"]],template:function(c,e){1&c&&(r.cc(0,"mat-card"),r.cc(1,"mat-card-header",0),r.cc(2,"mat-card-subtitle"),r.cc(3,"h2"),r.Oc(4,"Analytics"),r.bc(),r.bc(),r.bc(),r.cc(5,"mat-card-content"),r.cc(6,"div",1,2),r.cc(8,"ngx-charts-line-chart",3),r.kc("select",(function(c){return e.onSelect(c)})),r.bc(),r.bc(),r.bc(),r.bc()),2&c&&(r.Jb(8),r.vc("scheme",e.colorScheme)("results",e.analytics)("gradient",e.gradient)("xAxis",e.showXAxis)("yAxis",e.showYAxis)("legend",e.showLegend)("showXAxisLabel",e.showXAxisLabel)("showYAxisLabel",e.showYAxisLabel)("xAxisLabel",e.xAxisLabel)("yAxisLabel",e.yAxisLabel)("autoScale",e.autoScale)("roundDomains",e.roundDomains))},directives:[l.a,l.c,o.c,l.e,l.b,s.d],encapsulation:2}),c})();const F=[{path:"",component:(()=>{class c{constructor(){}ngOnInit(){}}return c.\u0275fac=function(e){return new(e||c)},c.\u0275cmp=r.Qb({type:c,selectors:[["app-dashboard"]],decls:41,vars:0,consts:[["fxLayout","row wrap"],["fxFlex","100","fxFlex.gt-sm","45",1,"p-1"],["fxFlex","100","fxFlex.gt-sm","55",1,"p-1"],["fxFlex","100","fxFlex.gt-sm","70",1,"p-1"],["fxFlex","100","fxFlex.gt-sm","30","fxLayout","column"],["fxFlex","100",1,"p-1"],["fxLayout","row","fxLayoutAlign","space-between center",1,"text-muted"],[1,"mat-icon-xlg"],[1,"fw-400"]],template:function(c,e){1&c&&(r.Xb(0,"app-tiles"),r.Xb(1,"app-info-cards"),r.cc(2,"div",0),r.cc(3,"div",1),r.Xb(4,"app-montly-sales"),r.bc(),r.cc(5,"div",2),r.Xb(6,"app-latest-orders"),r.bc(),r.bc(),r.cc(7,"div",0),r.cc(8,"div",3),r.Xb(9,"app-analytics"),r.bc(),r.cc(10,"div",4),r.cc(11,"div",5),r.cc(12,"mat-card"),r.cc(13,"div",6),r.cc(14,"mat-icon",7),r.Oc(15,"monetization_on"),r.bc(),r.cc(16,"div"),r.cc(17,"h1"),r.Oc(18,"$ 35700"),r.bc(),r.cc(19,"h2",8),r.Oc(20,"Profit"),r.bc(),r.bc(),r.bc(),r.bc(),r.bc(),r.cc(21,"div",5),r.cc(22,"mat-card"),r.cc(23,"div",6),r.cc(24,"mat-icon",7),r.Oc(25,"cloud_download"),r.bc(),r.cc(26,"div"),r.cc(27,"h1"),r.Oc(28,"187230"),r.bc(),r.cc(29,"h2",8),r.Oc(30,"Downloads"),r.bc(),r.bc(),r.bc(),r.bc(),r.bc(),r.cc(31,"div",5),r.cc(32,"mat-card"),r.cc(33,"div",6),r.cc(34,"mat-icon",7),r.Oc(35,"shopping_cart"),r.bc(),r.cc(36,"div"),r.cc(37,"h1"),r.Oc(38,"78,25 %"),r.bc(),r.cc(39,"h2",8),r.Oc(40,"Sales"),r.bc(),r.bc(),r.bc(),r.bc(),r.bc(),r.bc(),r.bc())},directives:[u,x,o.d,o.a,y,A,_,l.a,o.c,d.a],styles:[""]}),c})(),pathMatch:"full"}];let C=(()=>{class c{}return c.\u0275mod=r.Ub({type:c}),c.\u0275inj=r.Tb({factory:function(e){return new(e||c)},imports:[[a.c,i.h.forChild(F),n.a,s.e]]}),c})()}}]);