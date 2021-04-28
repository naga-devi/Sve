import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormsModule } from '@angular/forms';
import { OrdersComponent } from './orders/orders.component';
import { PendingOrderDialogComponent } from './orders/pending-orders/pending.orders.dialog';
import {SalesDetailsViewDialogComponent} from './orders/details-view-dialog/order-details-dialog';
import {SalesReportsDayLedgerComponent} from './reports/day-ledger.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AnalyticsModule } from '../analytics/analytics.module';

export const routes = [
    { path: '', redirectTo: 'orders', pathMatch: 'full' },
    { path: 'orders', component: OrdersComponent, data: { breadcrumb: 'Orders' } },
    { path: 'day-ledger', component: SalesReportsDayLedgerComponent, data: { breadcrumb: 'Day ledger' } },
];

@NgModule({
    declarations: [
        OrdersComponent,
        PendingOrderDialogComponent,
        SalesDetailsViewDialogComponent,
        SalesReportsDayLedgerComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule.forChild(routes),
        SharedModule,
        ReactiveFormsModule,
        NgxPaginationModule,
        AnalyticsModule
    ]
})
export class SalesModule { }
