import { Component, OnInit, HostListener } from "@angular/core";
import { AppService } from "src/app/app.service";
import { Product, ProductFilter } from "src/app/app.models";
import { ConfirmDialogComponent } from "src/app/shared/confirm-dialog/confirm-dialog.component";
import { MatDialog } from "@angular/material/dialog";
import { NotificationService, ResponseModel, ResponseStatus } from "../../../jx-core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: "app-product-list",
  templateUrl: "./product-list.component.html",
  styleUrls: ["./product-list.component.scss"],
})
export class ProductListComponent implements OnInit {
  public products: Array<Product> = [];
  public viewCol: number = 25;
  public page: any;
  public count = 0;
  public productFilter = new ProductFilter();
  public categories = [];
  public form: FormGroup;
  constructor(
    public appService: AppService,
    public dialog: MatDialog,
    public formBuilder: FormBuilder,
    public spinner: NgxSpinnerService,
    public alertService: NotificationService
  ) {}

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      'categoryId': 0,
      'name': '',
    });
    if (window.innerWidth < 1280) {
      this.viewCol = 33.3;
    }
    this.productFilter.categoryId = 0;
    this.productFilter.sortBy = 1;
    this.productFilter.startPrice = 0;
    this.productFilter.endPrice = 0;
    this.getAllProducts();
    this.getCategories();
  }

  public getAllProducts() {
    this.spinner.show('PROD_LIST');
    this.productFilter.pageSize = 12;
    this.productFilter.pageNumber = this.page || 1;
    this.productFilter.categoryId = this.form.controls["categoryId"].value;
    this.productFilter.name = this.form.controls["name"].value;
    //console.log(this.productFilter);
    this.appService
      .postBy("product/productdetails/find", this.productFilter)
      .subscribe((data) => {
        this.products = data.items ? data.items : [];
        this.count = data.totalCount;
        this.spinner.hide('PROD_LIST');
      });
  }

  public getCategories() {
    this.appService.getCategories().subscribe((data) => {
      this.categories = data;
      this.categories = this.categories.filter((x) => x.id != 0);
    });
  }

  public onPageChanged(event) {
    this.page = event;
    window.scrollTo(0, 0);
  }

  @HostListener("window:resize")
  public onWindowResize(): void {
    window.innerWidth < 1280 ? (this.viewCol = 33.3) : (this.viewCol = 25);
  }

  public remove(product: any) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: "Confirm Action",
        message: "Are you sure you want delete this product?",
      },
    });
    dialogRef.afterClosed().subscribe((dialogResult) => {
      if (dialogResult) {
        this.appService
          .deleteBy(`product/productdetails/delete/${product.productId}`)
          .subscribe(
            (response: ResponseModel) => {
              if (response.code === ResponseStatus.Success) {
                this.alertService.success(response.message);
                this.getAllProducts();
              } else {
                this.alertService.error(response.message);
              }
            },
            (err) => {
              this.alertService.error(err);
            }
          );
      }
    });
  }

  onFilterSubmit() {
    this.getAllProducts();
  }
}
