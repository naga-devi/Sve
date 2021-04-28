import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppService } from '../../../../app.service';
import { NotificationService, ResponseModel, ResponseStatus } from '../../../../jx-core';
import { Utilities } from '../../../../../app/jx-core';

@Component({
    selector: 'app-stock-edit-price-dialog',
    templateUrl: './edit-price-dialog.html',
    styleUrls: ['./edit-price-dialog.scss']
})
export class StockEditPriceDialogComponent implements OnInit {
    public form: FormGroup;
    constructor(public dialogRef: MatDialogRef<StockEditPriceDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        public appService: AppService,
        public alertService: NotificationService,
        public fb: FormBuilder) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            oldPrice: null,
            netPrice: [null, Validators.required],
            taxAmount: null,
            mrp: null,
            discount: null,
            sellPrice: null
        });

        if (this.data.stockData) {
            this.form.patchValue(this.data.stockData);
            this.form.controls['oldPrice'].setValue(this.form.controls['netPrice'].value);
        };
    }

    updatePrice() {
        const mrp = Number(this.form.controls.mrp.value);
        const discount = Number(this.form.controls.discount.value);
        const subtotal = mrp - ( mrp*discount/100 );
        const netPrice = Utilities.toFixed((subtotal/(100+this.data.stockData.taxSlab.cgst + this.data.stockData.taxSlab.sgst))*100, 2);
        const taxAmount = Utilities.toFixed(subtotal - netPrice, 2);
        this.form.controls['taxAmount'].setValue(taxAmount);
        this.form.controls['netPrice'].setValue(netPrice);
        
        const sellPrice = Utilities.toFixed(netPrice + taxAmount, 2);
        this.form.controls['sellPrice'].setValue(sellPrice);
    }

    // updateDiscount() {
    //     const netPrice = Number(this.form.controls.netPrice.value);
    //     const cgst = (this.data.stockData.taxSlab.cgst / 100) * netPrice;
    //     const sgst = (this.data.stockData.taxSlab.sgst / 100) * netPrice;
    //     const taxAmount = Utilities.toFixed(cgst + sgst, 2);
    //     const mrp = Utilities.toFixed(netPrice + taxAmount, 2);
    //     const discount = Number(this.form.controls.discount.value);
    //     const sellPrice = Utilities.toFixed(mrp - ((discount / 100) * mrp), 2);
    //     this.form.controls['sellPrice'].setValue(sellPrice);
    // }

    public onSubmit() {
        if (this.form.valid) {
            this.appService.postBy(`product/${this.data.stockData.stockGroupId}/stockitems/update-price`, this.form.value, true)
                .subscribe(
                    (data: ResponseModel) => {
                        if (data.code === ResponseStatus.Success) {
                            this.alertService.success(data.message);
                            this.dialogRef.close(data);
                        }
                        else {
                            this.alertService.error(data.message);
                        }
                    },
                    err => {
                        this.alertService.error(err); 
                    }
                );
        }
    }
}
