import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppService } from '../../../app.service';
import { NotificationService, ResponseModel, ResponseStatus } from '../../../jx-core';

@Component({
    selector: 'app-customer-dialog',
    templateUrl: './customer-dialog.component.html',
    styleUrls: ['./customer-dialog.component.scss']
})
export class CustomerDialogComponent implements OnInit {
    public form: FormGroup;
    constructor(public dialogRef: MatDialogRef<CustomerDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        public appService: AppService,
        public alertService: NotificationService,
        public fb: FormBuilder) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            customerId: 0,
            name: ['', Validators.required],
            address: ['', Validators.required],
            phoneNo: ['', Validators.required],
            companyName: '',
            email: [''],
            tinNo: [''],
            city: [''],
            zipCode: [''],
        });

        if (this.data.customer) {
            this.form.patchValue(this.data.customer);
        };
    }

    public onSubmit() {
        console.log(this.form.value);
        if (this.form.valid) {
            this.dialogRef.close(this.form.value);
        }

        if (this.form.valid) {
            const action = this.form.value.customerId > 0 ? 'update' : 'create'
            this.appService.postBy(`sales/customers/${action}`, this.form.value, this.form.value.customerId > 0)
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
