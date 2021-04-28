import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppService } from '../../../../../app.service';
import { NotificationService, ResponseModel, ResponseStatus } from '../../../../../jx-core';

@Component({
    selector: 'app-category-dialog',
    templateUrl: './category-dialog.component.html',
    styleUrls: ['./category-dialog.component.scss']
})
export class CategoryDialogComponent implements OnInit {
    public form: FormGroup;
    constructor(public dialogRef: MatDialogRef<CategoryDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        public appService: AppService,
        public alertService: NotificationService,
        public fb: FormBuilder) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            id: 0,
            name: [null, Validators.required],
            //hasSubCategory: false,
            parentId: 0
        });

        if (this.data.category) {
            this.form.patchValue(this.data.category);
        };
    }

    public onSubmit() {
        if (this.form.valid) {
            const action = this.form.value.id > 0 ? 'update' : 'create'
            this.appService.postBy(`product-category/${action}`, this.form.value,this.form.value.id > 0)
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
