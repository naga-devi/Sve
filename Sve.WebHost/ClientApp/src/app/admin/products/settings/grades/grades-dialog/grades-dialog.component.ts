import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppService } from '../../../../../app.service';
import { NotificationService, ResponseModel, ResponseStatus } from '../../../../../jx-core';

@Component({
    selector: 'app-grades-dialog',
    templateUrl: './grades-dialog.component.html',
    styleUrls: ['./grades-dialog.component.scss']
})
export class GradesDialogComponent implements OnInit {
    public form: FormGroup;
    categories: [];
    constructor(public dialogRef: MatDialogRef<GradesDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        public appService: AppService,
        public alertService: NotificationService,
        public fb: FormBuilder) { }

    ngOnInit(): void {
        this.getCategories();
        this.form = this.fb.group({
            gradeId: 0,
            name: [null, Validators.required],
            //hasSubCategory: false,
            categoryId: null
        });

        if (this.data.grade) {
            this.form.patchValue(this.data.grade);
        };
    }

    public getCategories() {
        this.appService.getBy('product-category/all').subscribe(data => {
            this.categories = data;
        });
    }

    public onSubmit() {
        if (this.form.valid) {
            const action = this.form.value.gradeId > 0 ? 'update' : 'create'
            this.appService.postBy(`product/grades/${action}`, this.form.value, this.form.value.gradeId > 0)
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
