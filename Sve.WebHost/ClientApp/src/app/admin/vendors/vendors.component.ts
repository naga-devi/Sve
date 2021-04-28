import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/app.service';
import { MatDialog } from '@angular/material/dialog';
import { AppSettings, Settings } from 'src/app/app.settings';
import { QueryParamsModel } from '../../jx-core';
import { VendorDialogComponent } from './vendor-dialog/vendor-dialog.component';

@Component({
    selector: 'app-vendors',
    templateUrl: './vendors.component.html',
    styleUrls: ['./vendors.component.scss']
})
export class VendorsComponent implements OnInit {
    public vendors = [];
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
        this.getVendors();
    }

    public onPageChanged(event) {
        this.page = event;
        window.scrollTo(0, 0);
    }

    public getVendors() {
        const queryParams = new QueryParamsModel(this.filter, 'asc', 'Name', this.page || 1, 500);
        this.appService.postBy(`purchasing/vendors/find`, queryParams).subscribe(data => {
            this.vendors = data.items ? data.items : [];
            this.count = data.totalCount;
        });
    }

    public openDialog(data: any) {
        const dialogRef = this.dialog.open(VendorDialogComponent, {
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
                this.getVendors();
            }
        });
    }
}
