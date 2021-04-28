import { Component, OnInit } from '@angular/core';
import { Category } from 'src/app/app.models';
import { AppService } from 'src/app/app.service';
import { CategoryDialogComponent } from './category-dialog/category-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { AppSettings, Settings } from 'src/app/app.settings';
import { NotificationService, QueryParamsModel, ResponseModel, ResponseStatus } from '../../../../jx-core';

@Component({
    selector: 'app-categories',
    templateUrl: './categories.component.html',
    styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {
    public categories: Category[] = [];
    public page: any;
    public count = 0;
    public settings: Settings;
    constructor(public appService: AppService, public dialog: MatDialog, public appSettings: AppSettings,
        public alert : NotificationService) {
        this.settings = this.appSettings.settings;
    }

    ngOnInit(): void {
        this.getCategories();
    }

    public getCategories() {
        const queryParams = new QueryParamsModel({}, 'asc', 'Name', this.page || 1, 10);
        this.appService.postBy('product-category/find', queryParams).subscribe(data => {
            this.categories = data.items;
            this.count = data.totalCount;
        });
    }

    public onPageChanged(event: any) {
        this.page = event;
        window.scrollTo(0, 0);
    }

    public openCategoryDialog(data: any) {
        const dialogRef = this.dialog.open(CategoryDialogComponent, {
            data: {
                category: data,
                categories: this.categories
            },
            panelClass: ['theme-dialog'],
            autoFocus: false,
            direction: (this.settings.rtl) ? 'rtl' : 'ltr'
        });
        dialogRef.afterClosed().subscribe(response => {
            if (response) {
                this.getCategories();
            }
        });
    }

    public remove(category: any) {
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            maxWidth: "400px",
            data: {
                title: "Confirm Action",
                message: "Are you sure you want remove this category?"
            }
        });
        dialogRef.afterClosed().subscribe(dialogResult => {
            if (dialogResult) {
                this.appService.deleteBy(`product-category/delete/${category.id}`)
                    .subscribe(
                        (data: ResponseModel) => {
                            if (data.code === ResponseStatus.Success) {
                                this.alert.success(data.message);
                                this.getCategories();
                            }
                            else {
                                this.alert.error(data.message);
                            }
                        },
                        err => {
                            this.alert.error(err);
                        }
                    );
            }
        });
    }

}
