import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import {
  NotificationService,
  ResponseModel,
  ResponseStatus,
  toServerDate,
} from "../../../jx-core";
import { AppService } from "../../../app.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: "app-purchases-credit-note-edit-dialog",
  templateUrl: "./edit-dialog.html",
  styleUrls: ["./edit-dialog.scss"],
})
export class CreditNoteWithOrderEditDialogComponent implements OnInit {
  purchaseItems = [];
  public form: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<CreditNoteWithOrderEditDialogComponent>,
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

  issueCreditNote() {
    debugger
    let _form= this.form.value;
    _form["issueDate"]= toServerDate(_form["issueDate"]);
    this.appService.postBy(`purchasing/creditnotes/save-with-purchaseorder/${this.data.order.purchaseOrderId}`, _form).subscribe(
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

  ngOnDestroy() {
    //this.sub.unsubscribe();
  }

  initForm() {
    this.form = this.formBuilder.group({
      vendorId: this.data.order.vendorId,
      purchaseOrderId: this.data.order.purchaseOrderId,
      issueDate: [new Date(), Validators.required],
      currentBalance: ["50000", Validators.required],
      discount: [null, Validators.required],
      remarks: [null],
    });
  }
}
