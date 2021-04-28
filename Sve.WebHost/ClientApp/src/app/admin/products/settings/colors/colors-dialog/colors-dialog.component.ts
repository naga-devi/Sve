import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppService } from '../../../../../app.service';
import { NotificationService, ResponseModel, ResponseStatus } from '../../../../../jx-core';

@Component({
    selector: 'app-colors-dialog',
    templateUrl: './colors-dialog.component.html',
    styleUrls: ['./colors-dialog.component.scss']
})
export class ColorsDialogComponent implements OnInit {
    public form: FormGroup;
    categories: [];
    public colors = ["#5C6BC0", "#66BB6A", "#EF5350", "#BA68C8", "#FF4081", "#9575CD", "#90CAF9", "#B2DFDB", "#DCE775", "#FFD740", "#00E676", "#FBC02D", "#FF7043", "#F5F5F5", "#696969"];
    public selectedColors: string;
    constructor(public dialogRef: MatDialogRef<ColorsDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        public appService: AppService,
        public alertService: NotificationService,
        public fb: FormBuilder) { }

    ngOnInit(): void {
        this.getCategories();
        this.form = this.fb.group({
            colorId: 0,
            name: [null, Validators.required],
            //hasSubCategory: false,
            categoryId: null
        });

        if (this.data.color) {
            this.form.patchValue(this.data.color);
        };
    }

    public getCategories() {
        this.appService.getBy('product-category/all').subscribe(data => {
            this.categories = data;
        });
    }

    public onColorSelectionChange(event: any) {
        if (event.value) {
            this.selectedColors = event.value;
            this.form.patchValue({ name: this.selectedColors });
        }
    }

    public onSubmit() {
        if (this.form.valid) {
            const action = this.form.value.colorId > 0 ? 'update' : 'create'
            this.appService.postBy(`product/colors/${action}`, this.form.value, this.form.value.colorId > 0)
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
