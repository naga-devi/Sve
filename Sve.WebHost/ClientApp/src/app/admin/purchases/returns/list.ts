import { Component, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { AppService } from "../../../app.service";
import { NotificationService, QueryParamsModel, ResponseModel, ResponseStatus } from "../../../jx-core";
import { AppSettings, Settings } from "../../../app.settings";
//import { PendingOrderDialogComponent } from './pending-orders/pending.orders.dialog';
import { UploadDownloadService } from "../../../jx-core";
import { Router } from "@angular/router";

@Component({
  selector: "app-purchases-returns",
  templateUrl: "./list.html",
  styleUrls: ["./list.scss"],
})
export class PurchaseReturnsListComponent implements OnInit {
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
      active :'returnDate'
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
      .postBy(`purchasing/purchasereturns/find`, queryParams)
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
}
