import { Component, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { AppService } from "../../../app.service";
import { NotificationService, QueryParamsModel, ResponseModel, ResponseStatus } from "../../../jx-core";
import { AppSettings, Settings } from "../../../app.settings";
//import { PendingOrderDialogComponent } from './pending-orders/pending.orders.dialog';
import { environment } from "../../../../environments/environment";
import { PurchaseDetailsListDialogComponent } from "../order-details-list-dialog/order-details-dialog";
import { UploadDownloadService } from "../../../jx-core";
import { Router } from "@angular/router";
import { PurchaseReturnsEditDialogComponent } from "../returns/edit-dialog";
import { CreditNoteWithOrderEditDialogComponent } from "../credit-notes/edit-dialog";

@Component({
  selector: "app-orders",
  templateUrl: "./orders.component.html",
  styleUrls: ["./orders.component.scss"],
})
export class PurchaseOrdersComponent implements OnInit {
  // public orders = [];
  // public page: any;
  // public count = 0;
  // filter: any = {};
  settings: Settings;
  matTable = {
    data:[],
    page: 1,
    count: 10,
    totalItems:0,
    sort:{
      direction:'desc',
      active :'purchaseDate'
    },
    filter: {InvoiceNo:''}
  };


  constructor(
    public appService: AppService,
    public dialog: MatDialog,
    public router: Router,
    public alertService: NotificationService,
    public uploadDownloadService: UploadDownloadService,
    public appSettings: AppSettings
  ) {
    this.settings = this.appSettings.settings;
  }

  ngOnInit(): void {
    this.getOrdersList();
  }

  public getOrdersList() {
    const queryParams = new QueryParamsModel(
      this.matTable.filter,
      this.matTable.sort.active,
      this.matTable.sort.direction ,
      this.matTable.page || 1,
      10
    );
    this.appService
      .postBy(`purchasing/purchase-order/find`, queryParams)
      .subscribe((data) => {
        this.matTable.data = data.items ? data.items : [];
        this.matTable.totalItems = data.totalCount;
      });
  }

  public onPageChanged(event) {
    this.matTable.page = event;
    window.scrollTo(0, 0);
  }

  searchByOrderNo(event: { target: { value: string } }) {
    //console.log("You entered: ", event.target.value);
    if (parseInt(event.target.value, 0) == 0) return;
    this.matTable.filter.InvoiceNo = event.target.value;
    this.getOrdersList();
  }

  public openOrderDetailsDialog(data: any) {
    const dialogRef = this.dialog.open(PurchaseDetailsListDialogComponent, {
      data: {
        order: data,
        //stores: this.stores,
        //countries: this.countries
      },
      panelClass: ["theme-dialog"],
      autoFocus: false,
      direction: this.settings.rtl ? "rtl" : "ltr",
      width: "80%",
    });
    dialogRef.afterClosed().subscribe((order) => {
      return;
    });
  }

  public returnOrder(data: any) {
    const dialogRef = this.dialog.open(PurchaseReturnsEditDialogComponent, {
      data: {
        order: data,
        //stores: this.stores,
        //countries: this.countries
      },
      panelClass: ["theme-dialog"],
      autoFocus: false,
      direction: this.settings.rtl ? "rtl" : "ltr",
      width: "80%",
    });
    dialogRef.afterClosed().subscribe((order) => {
      return;
    });
  }

  public issueCreditNote(data: any) {
    const dialogRef = this.dialog.open(CreditNoteWithOrderEditDialogComponent, {
      data: {
        order: data,
        //stores: this.stores,
        //countries: this.countries
      },
      panelClass: ["theme-dialog"],
      autoFocus: false,
      direction: this.settings.rtl ? "rtl" : "ltr",
      width: "80%",
    });
    dialogRef.afterClosed().subscribe((order) => {
      return;
    });
  }

  public edit(order: any) {
    this.router.navigateByUrl(`admin/purchases/edit/${order.purchaseOrderId}`);
  }

  deleteOrder(order: any){
    this.appService
      .deleteBy(`purchasing/purchase-order/delete/${order.purchaseOrderId}`)
      .subscribe((response: ResponseModel) => {
        if (response.code === ResponseStatus.Success) {
            this.alertService.success(response.message);
            this.getOrdersList();
        } else {
            this.alertService.error(response.message);
        }
    },
    (err) => {
        this.alertService.error(err);
    });
  }

  downloadInvoice(salesOrderId: any) {
    this.uploadDownloadService.downloadFileFromApi(
      `${environment.apiUrl}sales/orders/download-invoice/${salesOrderId}`,
      `${salesOrderId}-invoice.pdf`
    );
  }
}
