import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppService } from '../../../../../app.service';
import { NotificationService, ResponseModel, ResponseStatus } from '../../../../../jx-core';

@Component({
    selector: 'app-brands-dialog',
    templateUrl: './brands-dialog.component.html',
    styleUrls: ['./brands-dialog.component.scss']
})
export class BrandsDialogComponent implements OnInit {
    public form: FormGroup;
    categories: [];
    constructor(public dialogRef: MatDialogRef<BrandsDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        public appService: AppService,
        public alertService: NotificationService,
        public fb: FormBuilder) { }

    ngOnInit(): void {
        this.getCategories();
        this.form = this.fb.group({
            brandId: 0,
            name: [null, Validators.required],
            //hasSubCategory: false,
            categoryId: null
        });

        if (this.data.brand) {
            this.form.patchValue(this.data.brand);
        };
    }

    public getCategories() {
        this.appService.getBy('product-category/all').subscribe(data => {
            this.categories = data;
        });
    }

    public onSubmit() {
        if (this.form.valid) {
            const action = this.form.value.brandId > 0 ? 'update' : 'create'
            this.appService.postBy(`product/brands/${action}`, this.form.value, this.form.value.brandId > 0)
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
