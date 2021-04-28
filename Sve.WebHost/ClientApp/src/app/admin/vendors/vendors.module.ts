import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { VendorDialogComponent } from './vendor-dialog/vendor-dialog.component';
import { VendorsComponent } from './vendors.component';

export const routes = [
    { path: '', component: VendorsComponent, pathMatch: 'full' }
];

@NgModule({
    declarations: [
        VendorsComponent,
        VendorDialogComponent
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        ReactiveFormsModule,
        SharedModule,
        NgxPaginationModule
    ]
})
export class VendorsModule { }
