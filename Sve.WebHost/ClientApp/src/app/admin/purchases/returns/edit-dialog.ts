import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import {
  NotificationService,
  ResponseModel,
  ResponseStatus,
} from "../../../jx-core";
import { AppService } from "../../../app.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: "app-purchases-returns-edit-dialog",
  templateUrl: "./edit-dialog.html",
  styleUrls: ["./edit-dialog.scss"],
})
export class PurchaseReturnsEditDialogComponent implements OnInit {
  purchaseItems = [];
  public form: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<PurchaseReturnsEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public appService: AppService,
    public formBuilder: FormBuilder,
    public alertService: NotificationService
  ) {}

  ngOnInit(): void {
    this.getPurchaseItemsByOrderId();
    this.initForm();
  }

  public getPurchaseItemsByOrderId() {
    this.appService
      .getBy(`purchasing/${this.data.order.purchaseOrderId}/details/view`)
      .subscribe(
        (response: any[]) => {
          this.purchaseItems = response;
          // if(this.purchaseItems && this.purchaseItems.length > 0){
          //   this.purchaseItems.forEach(x=>{
          //     x['rejectedQty']=null;
          //   });
          // }
        },
        (err: any) => {
          this.alertService.error(err);
        }
      );
  }

  onReturnSave() {
    console.log(this.purchaseItems);
    let selectedItems = this.purchaseItems.filter(
      (x) => parseInt(x.purchase.rejectedQty, 0) > 0
    );
    if (selectedItems && selectedItems.length > 0) {
      let rejectedDetails = [];
      selectedItems.forEach((x) => {
        rejectedDetails.push(x.purchase);
      });
      const request = {
        header: this.form.value,
        details: rejectedDetails,
      };

      this.appService
        .postBy(`purchasing/purchasereturns/create`, request)
        .subscribe(
          (response: ResponseModel) => {
            if (response.code === ResponseStatus.Success) {
              this.alertService.success(response.message);
              this.dialogRef.close({
                response,
              });
            } else {
              this.alertService.error(response.message);
            }
          },
          (err) => {
            this.alertService.error(err);
          }
        );
    }
  }

  ngOnDestroy() {
    //this.sub.unsubscribe();
  }

  validateRejectedQty(rateInput: any, maxQty: number) {
    let selectedItems = this.purchaseItems.filter(
      (x) => parseInt(x.purchase.rejectedQty, 0) > 0
    );
    if (selectedItems && selectedItems.length > 0) {
      let totalAmount = 0;
      selectedItems.forEach((x) => {
        let qty = parseInt(x.purchase.rejectedQty, 0);
        totalAmount += qty * x.purchase.purchasedCost;
      });
      this.form.controls.totalAmount.setValue(totalAmount.toFixed(2));
    }

    const _x = parseInt(rateInput, 0);
    return _x < 0 ? 0 : _x > maxQty ? 0 : _x;
  }

  updateRoundOff() {
    const subTotal = Number(this.form.controls.totalAmount.value);
    const grandTotal = Number(this.form.controls.grandTotal.value);
    const roundOff = subTotal - grandTotal;
    this.form.controls.roundOff.setValue(roundOff.toFixed(2));
  }

  initForm() {
    this.form = this.formBuilder.group({
      purchaseOrderId: this.data.order.purchaseOrderId,
      returnDate: [new Date(), Validators.required],
      totalAmount: [null, Validators.required],
      roundOff: [null, Validators.required],
      grandTotal: [null, Validators.required],
      remarks: [null],
    });
  }
}
