import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppService } from '../../../app.service';
import { NotificationService, ResponseModel, ResponseStatus } from '../../../jx-core';

@Component({
    selector: 'app-vendor-dialog',
    templateUrl: './vendor-dialog.component.html',
    styleUrls: ['./vendor-dialog.component.scss']
})
export class VendorDialogComponent implements OnInit {
    public form: FormGroup;
    constructor(public dialogRef: MatDialogRef<VendorDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        public appService: AppService,
        public alertService: NotificationService,
        public fb: FormBuilder) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            vendorId: 0,
            address: ['', Validators.required],
            phoneNo: ['', Validators.required],
            companyName: ['', Validators.required],
            email: ['', Validators.required],
            tinNo: ['', Validators.required],
            //city: [''],
            //zipCode: [''],
        });

        if (this.data.customer) {
            this.form.patchValue(this.data.customer);
        };
    }

    public onSubmit() {
        if (this.form.valid) {
            const action = this.form.value.vendorId > 0 ? 'update' : 'create'
            this.appService.postBy(`purchasing/vendors/${action}`, this.form.value, this.form.value.vendorId > 0)
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
