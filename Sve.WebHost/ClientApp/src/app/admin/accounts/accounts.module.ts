import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { JxNetCoreModule } from '../../jx-core';
import { NewTransactionComponent } from './transactions/add/transaction.component';
import {TransactionListComponent} from './transactions/list/list';

export const routes = [
  { path: '', redirectTo: 'transaction', pathMatch: 'full' },
    { path: 'transaction', component: TransactionListComponent, data: { breadcrumb: 'Transaction Details' } },
    { path: 'new-payment', component: NewTransactionComponent, data: { breadcrumb: 'New Payment' } },
]
@NgModule({
  declarations: [
    NewTransactionComponent,
    TransactionListComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    SharedModule,
    NgxPaginationModule,
    JxNetCoreModule
  ]
})
export class AccountsModule { }
