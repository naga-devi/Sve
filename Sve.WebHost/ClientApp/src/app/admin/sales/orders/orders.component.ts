import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AppService } from '../../../app.service';
import { AppSettings, Settings } from '../../../app.settings';
import { PendingOrderDialogComponent } from './pending-orders/pending.orders.dialog';
import { UploadDownloadService , QueryParamsModel } from '../../../jx-core';
import { environment } from '../../../../environments/environment'
import { SalesDetailsViewDialogComponent } from './details-view-dialog/order-details-dialog';

@Component({
    selector: 'app-orders',
    templateUrl: './orders.component.html',
    styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
    public orders = []
    public page = 1;
    public count = 0;
    filter: any = {};
    settings: Settings;
    constructor(public appService: AppService,
        public dialog: MatDialog,
        public uploadDownloadService: UploadDownloadService,
        public appSettings: AppSettings) {
        this.settings = this.appSettings.settings;

    }


    ngOnInit(): void {
        this.getOrdersList();
    }

    public getOrdersList() {
        const queryParams = new QueryParamsModel(this.filter, 'desc', 'OrderDate', this.page || 1, 20);
        this.appService.postBy(`sales/orders/find`, queryParams).subscribe(data => {
            this.orders = data.items ? data.items : [];
            this.count = data.totalCount;
        });
    }

    public onPageChanged(event) {
        this.page = event;
        window.scrollTo(0, 0);
    }

    searchByOrderNo(event) {
        //console.log("You entered: ", event.target.value);
        if (parseInt(event.target.value, 0) == 0)
            return;
        this.filter.SalesOrderId = parseInt(event.target.value, 0);
        this.getOrdersList();
    }

    public openOrderEditDialog(data: any) {
        const dialogRef = this.dialog.open(PendingOrderDialogComponent, {
            data: {
                salesOrderId: data.salesOrderId,
                //stores: this.stores,
                //countries: this.countries
            },
            panelClass: ['theme-dialog'],
            autoFocus: false,
            direction: (this.settings.rtl) ? 'rtl' : 'ltr'
        });
        dialogRef.afterClosed().subscribe(order => {
            this.getOrdersList();
        });
    }

    openOrderViewDialog(order: any) {
        const dialogRef = this.dialog.open(SalesDetailsViewDialogComponent, {
            data: {
                order: order,
                //countries: this.countries
            },
            panelClass: ['theme-dialog'],
            autoFocus: false,
            direction: (this.settings.rtl) ? 'rtl' : 'ltr',
            width: "80%",
        });
        dialogRef.afterClosed().subscribe(order => {
            return;
        });
    }

    downloadInvoice(salesOrderId: any) {
        this.uploadDownloadService.downloadFileFromApi(`${environment.apiUrl}sales/orders/download-invoice/${salesOrderId}`, `${salesOrderId}-invoice.pdf`);
    }

    printInvoice(salesOrderId: any) {
        this.uploadDownloadService.printFileFromApi(`${environment.apiUrl}sales/orders/download-invoice/${salesOrderId}`, `${salesOrderId}-invoice.pdf`);
    }

}
