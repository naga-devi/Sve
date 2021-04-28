import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/app.service';
import { MatDialog } from '@angular/material/dialog';
import { CustomerDialogComponent } from './customer-dialog/customer-dialog.component';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { AppSettings, Settings } from 'src/app/app.settings';
import { QueryParamsModel } from '../../jx-core';

@Component({
    selector: 'app-customers',
    templateUrl: './customers.component.html',
    styleUrls: ['./customers.component.scss']
})
export class CustomersComponent implements OnInit {
    public customers = [];
    public countries = [];
    public page: any;
    public count = 0;
    public settings: Settings;
    filter: any = {};
    constructor(public appService: AppService, public dialog: MatDialog, public appSettings: AppSettings) {
        this.settings = this.appSettings.settings;
    }

    ngOnInit(): void {
        this.countries = [];
        this.getCustomers();
    }

    public onPageChanged(event) {
        this.page = event;
        window.scrollTo(0, 0);
    }

    public getCustomers() {
        const queryParams = new QueryParamsModel(this.filter, 'asc', 'Name', this.page || 1, 500);
        this.appService.postBy(`sales/customers/find`, queryParams).subscribe(data => {
            this.customers = data.items ? data.items : [];
            this.count = data.totalCount;
        });
    }

    public openCustomerDialog(data: any) {
        const dialogRef = this.dialog.open(CustomerDialogComponent, {
            data: {
                customer: data,
                countries: this.countries
            },
            panelClass: ['theme-dialog'],
            autoFocus: false,
            direction: (this.settings.rtl) ? 'rtl' : 'ltr'
        });
        dialogRef.afterClosed().subscribe(customer => {
            if (customer) {
                this.getCustomers();
            }
        });
    }
}
