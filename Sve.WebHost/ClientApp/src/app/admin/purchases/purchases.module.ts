import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormsModule } from '@angular/forms';
import { PurchaseOrdersComponent } from './order-list/orders.component';
import { AddPurchaseOrderComponent } from './add-order/add-order.component';
import { AddPurchaseItemDialogComponent } from './add-order/add-purchase-dialog/add-purchase-item-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import {PurchaseDetailsListDialogComponent} from './order-details-list-dialog/order-details-dialog';
import { JxNetCoreModule } from '../../jx-core';
import {PurchaseReturnsEditDialogComponent} from './returns/edit-dialog'
import {CreditNoteWithOrderEditDialogComponent} from './credit-notes/edit-dialog'
import {PurchaseReturnsListComponent} from './returns/list'
import {CreditNoteListComponent} from './credit-notes/list'


export const routes = [
    { path: '', redirectTo: 'orders', pathMatch: 'full' },
    { path: 'orders', component: PurchaseOrdersComponent, data: { breadcrumb: 'Purchase Orders' } },
    { path: 'add-purchase', component: AddPurchaseOrderComponent, data: { breadcrumb: 'Add Purchase order' } },
    { path: 'edit/:id', component: AddPurchaseOrderComponent, data: { breadcrumb: 'Edit purchase order' } },
    { path: 'credit-notes', component: CreditNoteListComponent, data: { breadcrumb: 'Credit Notes' } },
    { path: 'purchase-returns', component: PurchaseReturnsListComponent, data: { breadcrumb: 'Purchase Returns' } }
    //{ path: 'transactions', component: TransactionsComponent, data: { breadcrumb: 'Transactions' } }
];

@NgModule({
    declarations: [
        PurchaseOrdersComponent,
        AddPurchaseOrderComponent,
        AddPurchaseItemDialogComponent,
        PurchaseDetailsListDialogComponent,
        PurchaseReturnsEditDialogComponent,
        CreditNoteWithOrderEditDialogComponent,
        CreditNoteListComponent,
        PurchaseReturnsListComponent
    ],
    imports: [
        CommonModule,
        JxNetCoreModule,
        FormsModule,
        RouterModule.forChild(routes),
        SharedModule,
        ReactiveFormsModule,
        NgxPaginationModule,
    ]
})
export class PurchasesModule { }
