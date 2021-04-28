import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { JxNetCoreModule } from '../../jx-core';
import { NewTransactionComponent } from './transactions/add/transaction.component';

export const routes = [
  { path: '', redirectTo: 'transaction', pathMatch: 'full' },
    { path: 'transaction', component: NewTransactionComponent, data: { breadcrumb: 'New Payment' } },
    { path: 'new-payment', component: NewTransactionComponent, data: { breadcrumb: 'New Payment' } },
]
@NgModule({
  declarations: [
    NewTransactionComponent,
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
