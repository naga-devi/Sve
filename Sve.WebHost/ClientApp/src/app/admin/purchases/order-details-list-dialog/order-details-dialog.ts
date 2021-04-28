import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { NotificationService } from "../../../jx-core";
import { AppService } from '../../../app.service';

@Component({
  selector: "app-purchases-details-list-dialog",
  templateUrl: "./order-details-dialog.html",
  styleUrls: ["./order-details-dialog.scss"],
})
export class PurchaseDetailsListDialogComponent implements OnInit {
purchaseItems=[];
  constructor(public dialogRef: MatDialogRef<PurchaseDetailsListDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public appService: AppService,
    public alertService: NotificationService) { }

  ngOnInit(): void {
    this.getPurchaseItemsByOrderId();
  }  

  public getPurchaseItemsByOrderId() {
    this.appService.getBy(`purchasing/${this.data.order.purchaseOrderId}/details/view`).subscribe(
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
