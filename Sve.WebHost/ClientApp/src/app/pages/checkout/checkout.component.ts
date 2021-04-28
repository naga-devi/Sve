import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatStepper } from '@angular/material/stepper';
import { Data, AppService } from '../../app.service';
import { CheckOutModel } from '../../app.models';
import { catchError } from 'rxjs/operators';
import { of, throwError } from 'rxjs';
import { ResponseModel, ResponseStatus } from '../../jx-core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-checkout',
    templateUrl: './checkout.component.html',
    styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
    @ViewChild('horizontalStepper', { static: true }) horizontalStepper: MatStepper;
    @ViewChild('verticalStepper', { static: true }) verticalStepper: MatStepper;
    billingForm: FormGroup;
    deliveryForm: FormGroup;
    paymentForm: FormGroup;
    countries = [];
    months = [];
    years = [];
    deliveryMethods = [];
    grandTotal = 0;
    orderId = null;
    checkoutStatusText = 'Processing your order...!';
    showBackButton = true;
    filterData: any;
    constructor(public appService: AppService,
        public router: Router,
        public formBuilder: FormBuilder) { }

    ngOnInit() {
        this.getPreRequisites();
        if (this.appService.Data.cartList.length === 0)
            this.router.navigate(['/']);
        this.orderId = null;
        //console.log(this.appService.Data.cartList);
        this.appService.Data.cartList.forEach(product => {
            this.grandTotal += product.cartCount * product.newPrice;
        });
    }

    getPreRequisites = () => {
        this.appService.getFilterTypesByCategory(0).subscribe(data => {
            this.filterData = data;
        });
    }

    getProductTitle(product: any) {
        var tempNames=[];
        if(this.filterData){
            if(this.filterData.brands && this.filterData.brands.length > 0){
                tempNames.push(this.filterData.brands.filter(x => x.brandId === product.brandId)[0].name);
            }

            if(this.filterData.materialTypes && this.filterData.materialTypes.length > 0){
                tempNames.push(this.filterData.materialTypes.filter(x => x.materialTypeId === product.materialTypeId)[0].name);
            }

            if(this.filterData.sizes && this.filterData.sizes.length > 0){
                tempNames.push(this.filterData.sizes.filter(x => x.sizeId === product.sizeId)[0].name);
            }

            if(this.filterData.grades && this.filterData.grades.length > 0){
                tempNames.push(this.filterData.grades.filter(x => x.gradeId === product.gradeId)[0].name);
            }

            if(this.filterData.colors && this.filterData.colors.length > 0){
                tempNames.push(this.filterData.colors.filter(x => x.colorId === product.colorId)[0].name);
            }
        }

        tempNames.push(product.name);

        return tempNames && tempNames.length > 0 ? tempNames.join(' | ') :'';
    }

    public placeOrder() {
        if (this.appService.Data.cartList && this.appService.Data.cartList.length > 0) {

            var checkOutModel = new Array<CheckOutModel>();
            this.appService.Data.cartList.forEach(prod => {
                this.grandTotal += prod.cartCount * prod.newPrice;
                let checkoutItem= new CheckOutModel(prod.id, prod.categoryId, prod.taxSlabId, prod.name, prod.productBaseId, prod.cartCount, prod.unitPrice, prod.cgst, prod.sgst);
                checkOutModel.push(checkoutItem);
            });
            //console.log(checkOutModel);
            this.showBackButton = false;
            this.appService.placeOrder(checkOutModel)
                .pipe(
                    catchError(err => {
                        console.log('Handling error locally and rethrowing it...', err);
                        this.showBackButton = true;
                        this.checkoutStatusText = 'Error in processing. Please contact administrator.';
                        return throwError(err);
                    })
                )
                .subscribe(
                    (data: ResponseModel) => {
                        this.orderId = data.newId;
                        if (data.code === ResponseStatus.Success && this.orderId && this.orderId > 0) {
                            this.showBackButton = false;
                            this.horizontalStepper._steps.forEach(step => step.editable = false);
                            this.verticalStepper._steps.forEach(step => step.editable = false);
                            this.appService.Data.cartList.length = 0;
                            this.appService.Data.totalPrice = 0;
                            this.appService.Data.totalCartCount = 0;
                            this.checkoutStatusText = 'Congratulation! Your order has been processed.';
                        }
                        else {
                            this.showBackButton = true;
                            this.checkoutStatusText = 'Error in processing. Please contact administrator.';
                        }
                    },
                    err => {
                        this.showBackButton = true;
                        this.checkoutStatusText = 'Error in processing. Please contact administrator.';
                        console.log(err);
                    }
                );
        }
    }
}
