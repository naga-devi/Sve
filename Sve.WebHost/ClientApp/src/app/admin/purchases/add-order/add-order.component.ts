import { Component, EventEmitter, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { AppService } from "src/app/app.service";
import { Category } from "src/app/app.models";
import { ActivatedRoute, Router } from "@angular/router";
import { MatDialog } from "@angular/material/dialog";
import { AppSettings, Settings } from "src/app/app.settings";
import { AddPurchaseItemDialogComponent } from "./add-purchase-dialog/add-purchase-item-dialog.component";
import { ConfirmDialogComponent } from "src/app/shared/confirm-dialog/confirm-dialog.component";
import {
  Utilities,
  ResponseModel,
  ResponseStatus,
  toServerDate,
  NotificationService,
} from "../../../jx-core";

const KEY_PURCHASE_ORDER_HEADER = "sve_purchase_order_head";

@Component({
  selector: "app-purchases-add-order",
  templateUrl: "./add-order.component.html",
  styleUrls: ["./add-order.component.scss"],
})
export class AddPurchaseOrderComponent implements OnInit {
  overAllDiscountPercentage=0.09;//9%
  public form: FormGroup;
  public categories: Category[];
  public productId: any;
  purchases = [];
  public settings: Settings;
  prerequisites = {};
  products = [];
  unitmeasures = [];
  totalAmount = 0;
  subTotal = 0;
  purchaseOrderId = 0;
  orderToEdit: any;

  private sub: any; 

  constructor(
    public appService: AppService,
    public appSettings: AppSettings,
    public formBuilder: FormBuilder,
    public dialog: MatDialog,
    public alertService: NotificationService,
    private activatedRoute: ActivatedRoute,
    public router: Router
  ) {
    this.settings = this.appSettings.settings;
  }

  ngOnInit(): void {
    this.initForm();
    this.loadPreRequisites();
    this.loadAllProducts();
    this.getUnitMeasures();
    this.sub = this.activatedRoute.params.subscribe((params) => {
      if (params["id"]) {
        this.purchaseOrderId = params["id"];
        this.getPurchaseOrderById();
      }
    });

    if (this.purchaseOrderId == 0) {
      const req = this.getOrderFromLS();
      if (req) {
        this.form.patchValue(req.header);
        this.purchases = req.details;
      }
    }
  }

  getPurchaseOrderById() {
    this.appService
      .getBy(`purchasing/purchase-order/edit/${this.purchaseOrderId}`)
      .subscribe(
        (response) => {
          this.orderToEdit = response;
          this.form.patchValue(response);
          this.purchases = response.details;
        },
        (err) => {
          this.alertService.error(err);
        }
      );
  }

  initForm() {
    this.form = this.formBuilder.group({
      vendorId: 0,
      purchaseOrderId: 0,
      companyName: [
        null,
        Validators.compose([Validators.required, Validators.minLength(4)]),
      ],
      tinNo: [null],
      email: [null],
      phoneNo: [null, Validators.required],
      address: [null, Validators.required],
      invoiceNo: [null, Validators.required],
      purchaseDate: [null, Validators.required],
      totalAmount: [null, Validators.required],
      discount: [null],
      netAmount: [null, Validators.required],
      cgstAmount: [null],
      sgstAmount: [null],
      subTotal: [null, Validators.required],
      roundOffAmount: [null],
      grandTotal: [null, Validators.required],
    });

    this.form.valueChanges.subscribe(selectedValue => {
        //console.log('form value changed')
        this.setOrderInLS();
      })
  }

  setNetAmount = () => {
    //console.log(this.purchases);
    this.totalAmount = 0;
    let netAmount = 0;
    let cgstAmount = 0;
    let sgstAmount = 0;

    this.purchases.forEach((purchaseItem) => {
      this.totalAmount =
        Number(this.totalAmount) + Number(purchaseItem.taxableAmount);
      cgstAmount = cgstAmount + Number(purchaseItem.totalCgstAmount);
      sgstAmount = sgstAmount + Number(purchaseItem.totalSgstAmount);
    });

    const discount = Number(this.form.controls.discount.value);
    const roundOffAmount = Number(this.form.controls.roundOffAmount.value);

    if (discount > 0) {
      netAmount = this.totalAmount - discount;
      cgstAmount = netAmount * this.overAllDiscountPercentage;
      sgstAmount = netAmount * this.overAllDiscountPercentage;
    } else {
      netAmount = this.totalAmount;
    }

    this.subTotal = netAmount + cgstAmount + sgstAmount;

    const grandTotal = this.subTotal - roundOffAmount;

    this.form.patchValue({
      totalAmount:this.totalAmount === 0 ? null : this.totalAmount.toFixed(2),
      netAmount: netAmount.toFixed(2),
      cgstAmount: cgstAmount === 0 ? null : cgstAmount.toFixed(2),
      sgstAmount: sgstAmount === 0 ? null : sgstAmount.toFixed(2),
      subTotal: this.subTotal.toFixed(2),
      grandTotal: grandTotal.toFixed(2),
      roundOffAmount: 0.0,
    });
  };

  updateRoundOff() {
    const subTotal = Number(this.form.controls.subTotal.value);
    const grandTotal = Number(this.form.controls.grandTotal.value);
    const roundOff = subTotal - grandTotal;
    this.form.controls.roundOffAmount.setValue(roundOff.toFixed(2));
    this.setOrderInLS();
  }

  public loadPreRequisites() {
    this.appService.getBy("v1/cart/filter-types/0").subscribe(
      (response) => {
        this.prerequisites = response;
      },
      (err) => {
        this.alertService.error(err);
      }
    );
  }

  public loadAllProducts() {
    this.appService.getBy("product/productdetails/all").subscribe(
      (response) => {
        this.products = response;
      },
      (err) => {
        this.alertService.error(err);
      }
    );
  }

  getUnitMeasures = () => {
    this.appService.getBy("product/unitmeasure/all").subscribe(
      (response) => {
        this.unitmeasures = response;
      },
      (err) => {
        this.alertService.error(err);
      }
    );
  };

  public openAddItemDialog(data: any, rowIndex: number, isNew: boolean) {
    const dialogRef = this.dialog.open(AddPurchaseItemDialogComponent, {
      data: {
        purchase: data,
        products: this.products,
        prerequisites: this.prerequisites,
        unitmeasures: this.unitmeasures,
      },
      panelClass: ["theme-dialog"],
      autoFocus: true,
      direction: this.settings.rtl ? "rtl" : "ltr",
    });
    dialogRef.afterClosed().subscribe((purchase) => {
      // if (purchase) {
      //   this.purchases.push(purchase);
      //   this.purchases.forEach((x) => {
      //     x.id == purchase.id;
      //   });
      // }
      if (purchase) {
        // const index: number = this.purchases.findIndex(
        //     (x) => x.id == purchase.id
        // );
        // if (index !== -1) {
        //     this.purchases[index] = purchase;
        // } else {
        //     let last_purchase = this.purchases[this.purchases.length - 1];
        //     if (last_purchase) purchase.id = last_purchase.id + 1;
        //     else purchase.id = 1;
        //     this.purchases.push(purchase);
        // }

        if (isNew) {
          this.purchases.push(purchase);
        } else {
          this.purchases[rowIndex] = purchase;
        }

        this.setNetAmount();
        this.setOrderInLS();
      }
    });
  }

  public remove(purchase: any, rowIndex: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: "Confirm Action",
        message: "Are you sure you want remove this purchase?",
      },
    });
    dialogRef.afterClosed().subscribe((dialogResult) => {
      if (dialogResult) {
        // const index: number = this.purchases.indexOf(purchase);
        // if (index !== -1) {
        //     this.purchases.splice(index, 1);
        // }
        this.purchases.splice(rowIndex, 1);
        this.setNetAmount();
        this.setOrderInLS();
      }
    });
  }

  public getVendorByTinNo() {
    this.appService
      .getBy(`purchasing/vendors/tin-no/${this.form.controls.tinNo.value}`)
      .subscribe((data) => {
        //if (data) this.ven = data.customerId;
        this.form.patchValue(data === null ? {} : data);
      });
  }

  //customer details
  public getVendorByPhone() {
    if (parseInt(this.form.controls.vendorId.value, 0) > 0) return;

    this.appService
      .getBy(
        `purchasing/vendors/phone-number/${this.form.controls.phoneNo.value}`
      )
      .subscribe((data) => {
        //if (data) this.customerId = data.customerId;
        this.form.patchValue(data === null ? {} : data);
      });
  }

  public onSubmit() {
    if (this.purchases.length > 0 && this.form.valid) {
      let vendor = this.form.value;
      vendor.discount = Number(vendor.discount);
      vendor.cgstAmount = Number(vendor.cgstAmount);
      vendor.sgstAmount = Number(vendor.sgstAmount);

      if (vendor.purchaseDate)
        vendor.purchaseDate = toServerDate(vendor.purchaseDate);

      const request = {
        purchases: this.purchases,
        vendor: vendor,
      };
      //console.log(request);
      this.appService
        .postBy("purchasing/purchase-order/save", request)
        .subscribe(
          (response: ResponseModel) => {
            if (response.code === ResponseStatus.Success) {
              this.alertService.success(response.message);
              this.setOrderInLS(true);
              this.router.navigate([`/admin/purchases/orders`]);
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

  copyPurchaseItem(item: any, rowIndex: number) {
    item.id = 0;
    this.openAddItemDialog(item, rowIndex, true);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  setOrderInLS(reset: boolean = false) {
    const req = {
      header: this.form.value,
      details: this.purchases,
    };
    localStorage.setItem(KEY_PURCHASE_ORDER_HEADER, JSON.stringify(req));

    if (reset) {
      localStorage.removeItem(KEY_PURCHASE_ORDER_HEADER);
    }
  }

  getOrderFromLS() {
    const req = localStorage.getItem(KEY_PURCHASE_ORDER_HEADER);

    if (req) return JSON.parse(req);

    return null;
  }
}
