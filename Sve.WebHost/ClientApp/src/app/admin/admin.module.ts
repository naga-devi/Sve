import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { SharedModule } from "../shared/shared.module";

import { InputFileConfig, InputFileModule } from "ngx-input-file";
const config: InputFileConfig = {
  fileAccept: "*",
};

import { AdminComponent } from "./admin.component";
import { MenuComponent } from "./components/menu/menu.component";
import { UserMenuComponent } from "./components/user-menu/user-menu.component";
import { FullScreenComponent } from "./components/fullscreen/fullscreen.component";
import { MessagesComponent } from "./components/messages/messages.component";
import { BreadcrumbComponent } from "./components/breadcrumb/breadcrumb.component";
import { AuthGuard, JxNetCoreModule } from "../jx-core/index";

export const routes = [
  {
    path: "",
    canActivate: [AuthGuard],
    component: AdminComponent,
    children: [
      {
        path: "",
        loadChildren: () =>
          import("./dashboard/dashboard.module").then((m) => m.DashboardModule),
      },
      {
        path: "products",
        loadChildren: () =>
          import("./products/products.module").then((m) => m.ProductsModule),
      },
      {
        path: "sales",
        loadChildren: () =>
          import("./sales/sales.module").then((m) => m.SalesModule),
      },
      {
        path: "purchases",
        loadChildren: () =>
          import("./purchases/purchases.module").then((m) => m.PurchasesModule),
      },
      {
        path: "customers",
        loadChildren: () =>
          import("./customers/customers.module").then((m) => m.CustomersModule),
        data: { breadcrumb: "Customers" },
      },
      {
        path: "vendors",
        loadChildren: () =>
            import("./vendors/vendors.module").then((m) => m.VendorsModule),
        data: { breadcrumb: "Coupons" },
      },
      {
        path: "accounts",
        loadChildren: () =>
            import("./accounts/accounts.module").then((m) => m.AccountsModule),
        data: { breadcrumb: "Accounts" },
      },
      {
        path: "analytics",
        loadChildren: () =>
          import("./analytics/analytics.module").then((m) => m.AnalyticsModule),
        data: { breadcrumb: "Analytics" },
      }
    ],
  },
];

@NgModule({
  declarations: [
    AdminComponent,
    MenuComponent,
    UserMenuComponent,
    FullScreenComponent,
    MessagesComponent,
    BreadcrumbComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    SharedModule,
    InputFileModule.forRoot(config),
    JxNetCoreModule
  ],
})
export class AdminModule {}
