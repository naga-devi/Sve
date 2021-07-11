import {
  Component,
  OnInit,
  Inject,
} from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AppService } from "../../../../app.service";

@Component({
  selector: "app-purchases-orders-add-purchase-item",
  templateUrl: "./add-purchase-item-dialog.component.html",
  styleUrls: ["./add-purchase-item-dialog.component.scss"],
})
export class AddPurchaseItemDialogComponent implements OnInit {
  public form: FormGroup;
  public materialTypes = [];
  public sizes = [];
  public brands = [];
  public products = [];
  public grades = [];
  public colors = [];

  constructor(
    public dialogRef: MatDialogRef<AddPurchaseItemDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public appService: AppService,
    public fb: FormBuilder
  ) {}

  ngOnInit(): void {
    //console.log(this.data);
    this.products = this.data.products;
    this.form = this.fb.group({
      id: 0,
      categoryId: [null, Validators.required],
      productId: [null, Validators.required],
      materialTypeId: [null, Validators.required],
      sizeId: [null, Validators.required],
      brandId: [null, Validators.required],
      colorId: [1],
      gradeId: [1],
      unitMeasureId: [null, Validators.required],
      unitPrice: [null, Validators.required],
      receivedQty: [null, Validators.required],
      mrp: [null, Validators.required],
      totalCgstAmount: [null, Validators.required],
      totalSgstAmount: [null, Validators.required],
      taxableAmount: [null],
      cgstAmount: [null],
      sgstAmount: [null],
      totalAmount: [null],
      discount: [null],
      subtotal: [null],
    });

    if (this.data.purchase) {
      this.data.purchase.totalAmount = this.data.purchase.totalAmount || 0;
      this.data.purchase.subtotal = this.data.purchase.subtotal || 0;
      this.form.patchValue(this.data.purchase);
    }
    // else{
    //     this.data.purchase={"id":0,"categoryId":10,"productId":43,"materialTypeId":10,"sizeId":27,"brandId":9,"colorId":3,"gradeId":3,"unitMeasureId":3,"unitPrice":26,"receivedQty":360,"mrp":120,"totalCgstAmount":825.55,"totalSgstAmount":825.55,"taxableAmount":9172.8,"cgstAmount":2.2932,"sgstAmount":2.2932,"totalAmount":30.066399999999998,"discount":2,"subtotal":25.48};
    //     this.form.patchValue(this.data.purchase);
    // }
  }

  onProductFilter(search: string) {
    //console.log(search);
    let filter = search.toLowerCase();
    this.products = this.data.products.filter(
      (option) =>
        option.categoryId == this.form.controls["categoryId"].value &&
        parseInt(this.form.controls["categoryId"].value, 0) &&
        option.name.toLowerCase().startsWith(filter)
    );
  }

  updatePrice() {
    const unitPrice = Number(this.form.controls.unitPrice.value);
    const receivedQty = Number(this.form.controls.receivedQty.value);
    const discount = Number(this.form.controls.discount.value);
    let subtotal = 0;
    const selectedProductId = parseInt(this.form.controls.productId.value, 0);
    const taxSlabId = this.products.filter(
      (x) => x.productId == selectedProductId
    )[0].taxSlabId;
    const cgstPercentage = this.data.prerequisites.taxItems.filter((x) => x.taxSlabId === taxSlabId)[0].cgst;
    const sgstPercentage = this.data.prerequisites.taxItems.filter((x) => x.taxSlabId === taxSlabId)[0].sgst;
    if (unitPrice > 0 && cgstPercentage > 0 && sgstPercentage > 0) {
      if (discount > 0) {
        subtotal = unitPrice - (unitPrice * discount) / 100;
      } else {
        subtotal = unitPrice;
      }

      const cgst = (cgstPercentage / 100) * subtotal;
      const sgst = (sgstPercentage / 100) * subtotal;
      const totalAmount = subtotal + cgst + sgst;

      this.form.controls["subtotal"].setValue(Number(subtotal).toFixed(2));
      this.form.controls["cgstAmount"].setValue(Number(cgst).toFixed(2));
      this.form.controls["sgstAmount"].setValue(Number(sgst).toFixed(2));
      this.form.controls["totalAmount"].setValue(Number(totalAmount).toFixed(2));

      this.form.controls["totalCgstAmount"].setValue(
        Number(cgst * receivedQty).toFixed(2)
      );
      this.form.controls["totalSgstAmount"].setValue(
        Number(sgst * receivedQty).toFixed(2)
      );
      this.form.controls["taxableAmount"].setValue(
        Number(subtotal * receivedQty).toFixed(2)
      );
    }
  }

  public onSubmit() {
    if (this.form.valid) {
      //let newPurchase = this.form.value;
      this.dialogRef.close(this.form.value);
    }
  }
}
