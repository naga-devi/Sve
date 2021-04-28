import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatStepper } from '@angular/material/stepper';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppService } from '../../../../app.service';
import { catchError } from 'rxjs/operators';
import { of, throwError } from 'rxjs';
import { environment } from '../../../../../environments/environment'
import { ResponseModel, UploadDownloadService, QueryParamsModel, Utilities, ResponseStatus } from '../../../../jx-core';

@Component({
    selector: 'app-sales-orders-pending-orders-dialog',
    templateUrl: './pending.orders.dialog.html',
    styleUrls: ['./pending.orders.dialog.scss']
})
export class PendingOrderDialogComponent implements OnInit {
    @ViewChild('horizontalStepper', { static: true }) horizontalStepper: MatStepper;
    @ViewChild('verticalStepper', { static: true }) verticalStepper: MatStepper;
    billingForm: FormGroup;
    deliveryForm: FormGroup;
    paymentForm: FormGroup;
    customerId = 0;
    orders = [];
    paymodes = [{ id: 1, name: 'Cash' }, { id: 2, name: 'Online' }];
    totalItems = 0;
    totalSubTotal = 0;
    orderRequest: any = {};
    isOrderChanged: boolean = false;
    checkoutStatusText = 'Processing your order...!';
    showBackButton = true;
    entityResponse: ResponseModel;
    constructor(public dialogRef: MatDialogRef<PendingOrderDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        public appService: AppService,
        public uploadDownloadService: UploadDownloadService,
        public fb: FormBuilder) { }

    ngOnInit(): void {
        this.createBillingForm();
        this.createPaymentForm();
        if (this.data.salesOrderId) {
            this.getOrdersList();
            //this.form.patchValue(this.data.customer);
        };
    }

    createBillingForm() {
        this.billingForm = this.fb.group({
            name: ['', Validators.required],
            address: ['', Validators.required],
            phoneNo: ['', Validators.required],
            companyName: '',
            email: [''],
            tinNo: [''],
            city: [''],
            zipCode: [''],
        });
    }

    createPaymentForm() {
        this.paymentForm = this.fb.group({
            totalAmount: [this.totalSubTotal, Validators.required],
            discountPercentage: [''],
            netAmount: ['', Validators.required],
            roundOffAmount: [''],
            grandTotal: ['', Validators.required],
            paidAmount: ['', Validators.required],
            balanceAmount: ['', Validators.required],
            paymode: ['', Validators.required],
            transactionNo: [''],
        });
    }

    public getOrdersList() {
        const queryParams = new QueryParamsModel({}, 'desc', 'OrderDate', 1, 500);
        this.appService.postBy(`sales/${this.data.salesOrderId}/details/find`, queryParams).subscribe(data => {
            this.orders = data.items ? data.items : [];
            this.updateTotalItemCount();
            //this.count = data.totalCount;
        });
    }

    //billing section
    updateTotalItems() {
        this.isOrderChanged = true;
        this.updateTotalItemCount();
    }

    updateTotalItemCount() {
        this.totalItems = 0;
        this.orders.forEach(item => {
            this.totalItems += item.orderQty;
        });

        this.updateSubTotal();
    }

    updateSubTotal() {
        this.totalSubTotal = 0;
        this.orders.forEach(item => {
            this.totalSubTotal += ((Number(item.unitPrice) + Number(item.cgstAmount) + Number(item.sgstAmount)) * Number(item.orderQty));
        });

        this.paymentForm.controls.totalAmount.setValue(this.totalSubTotal.toFixed(2));
        //this.paymentForm.controls.discountPercentage.setValue(0);
        this.paymentForm.controls.netAmount.setValue(this.totalSubTotal.toFixed(2));
        this.paymentForm.controls.grandTotal.setValue(this.totalSubTotal.toFixed(2));
        this.paymentForm.controls.paidAmount.setValue(this.totalSubTotal.toFixed(2));
        this.paymentForm.controls.discountPercentage.setValue(0);
        this.paymentForm.controls.roundOffAmount.setValue(0);
        this.paymentForm.controls.balanceAmount.setValue(0);
        //this.updateGrandTotal();
    }


    //payment section
    updateGrandTotal() {

        const discountPercentage = Number(this.paymentForm.controls.discountPercentage.value);
        const grandTotal = this.totalSubTotal - (this.totalSubTotal * (discountPercentage / 100));
        //const balance = Utilities.toFixed(Number(this.paymentForm.controls.grandTotal.value) - Number(this.paymentForm.controls.paidAmount.value), 2);
        const roundOff = Number(this.paymentForm.controls.netAmount.value) - Number(this.paymentForm.controls.grandTotal.value);

        this.paymentForm.controls.netAmount.setValue(grandTotal.toFixed(2));
        this.paymentForm.controls.grandTotal.setValue(grandTotal.toFixed(2));
        this.paymentForm.controls.paidAmount.setValue(grandTotal.toFixed(2));
        //this.paymentForm.controls.balanceAmount.setValue(balance);
        this.paymentForm.controls.roundOffAmount.setValue(roundOff.toFixed(2));
    }

    updateRoundOff() {
        const grandTotal = Number(this.paymentForm.controls.grandTotal.value);
        const roundOff = Number(this.paymentForm.controls.netAmount.value) - Number(this.paymentForm.controls.grandTotal.value);
        this.paymentForm.controls.paidAmount.setValue(grandTotal.toFixed(2));
        //this.paymentForm.controls.balanceAmount.setValue(0);
        this.paymentForm.controls.roundOffAmount.setValue(roundOff.toFixed(2));
    }

    updateBalance() {
        const balance = Number(this.paymentForm.controls.grandTotal.value) - Number(this.paymentForm.controls.paidAmount.value);

        this.paymentForm.controls.balanceAmount.setValue(balance.toFixed(2));
    }

    //customer details
    public getCustomerByPhone() {
        this.appService.getBy(`sales/customers/phone-number/${this.billingForm.controls.phoneNo.value}`).subscribe(data => {
            if (data) {
                this.customerId = data.customerId;
                this.billingForm.patchValue(data);
            }
        });
    }

    public getCustomerByTinNo() {
        this.appService.getBy(`sales/customers/tin-no/${this.billingForm.controls.tinNo.value}`).subscribe(data => {
            if (data) {
                this.customerId = data.customerId;
                this.billingForm.patchValue(data);
            }
        });
    }

    public confirmOrder() {
        if (this.billingForm.valid && this.paymentForm.valid && this.orders.length > 0) {
            let salesOrder = this.paymentForm.value;
            salesOrder.OrderDetails = this.orders;
            this.orderRequest.salesOrder = salesOrder;
            this.orderRequest.customer = this.billingForm.value;
            this.orderRequest.customer.customerId = this.customerId;
            this.orderRequest.isOrderChanged = this.isOrderChanged;
            console.log(this.orderRequest);

            this.appService.postBy(`sales/orders/confirm/${this.data.salesOrderId}`, this.orderRequest)
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
                        this.entityResponse = data;
                        if (data.code === ResponseStatus.Success) {
                            this.showBackButton = false;
                            this.checkoutStatusText = 'Congratulation! Your order has been confirmed.';
                            this.horizontalStepper._steps.forEach(step => step.editable = false);
                            //this.verticalStepper._steps.forEach(step => step.editable = false);

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

    returnToOrders() {
        this.dialogRef.close(this.entityResponse);
    }

    downloadInvoiece() {
        this.uploadDownloadService.downloadFileFromApi(`${environment.apiUrl}sales/orders/download-invoice/${this.data.salesOrderId}`);
    }

    public compareFunction(o1: any, o2: any) {
        return (o1.name == o2.name && o1.code == o2.code);
    }
}
