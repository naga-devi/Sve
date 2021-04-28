import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { NotificationService } from "src/app/jx-core";
import { AppService } from '../../../../app.service';

@Component({
  selector: "app-sales-details-view-dialog",
  templateUrl: "./order-details-dialog.html",
  styleUrls: ["./order-details-dialog.scss"],
})
export class SalesDetailsViewDialogComponent implements OnInit {
purchaseItems=[];
  constructor(public dialogRef: MatDialogRef<SalesDetailsViewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public appService: AppService,
    public alertService: NotificationService) { }

  ngOnInit(): void {
    this.getSalesViewByOrderId();
  }  

  public getSalesViewByOrderId() {
    this.appService.getBy(`sales/${this.data.order.salesOrderId}/details/view`).subscribe(
      (response: any[]) => {
        this.purchaseItems = response;
      },
      (err: any) => {
        this.alertService.error(err);
      }
    );
  }

  ngOnDestroy() {
    //this.sub.unsubscribe();
  }
}
