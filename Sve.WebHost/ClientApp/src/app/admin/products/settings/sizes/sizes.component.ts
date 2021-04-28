import { Component, OnInit } from '@angular/core';
import { Category } from 'src/app/app.models';
import { AppService } from 'src/app/app.service';
import { SizesDialogComponent } from './sizes-dialog/sizes-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { AppSettings, Settings } from 'src/app/app.settings';
import { NotificationService, QueryParamsModel, ResponseModel, ResponseStatus } from '../../../../jx-core';
@Component({
    selector: 'app-sizes',
    templateUrl: './sizes.component.html',
    styleUrls: ['./sizes.component.scss']
})
export class SizesComponent implements OnInit {
    public sizes: Category[] = [];
    public page: any;
    public count = 0;
    public settings: Settings;
    constructor(public appService: AppService, public dialog: MatDialog,
        public appSettings: AppSettings, public alertService: NotificationService) {
        this.settings = this.appSettings.settings;
    }

    ngOnInit(): void {
        this.getSizes();
    }

    public getSizes() {
        const queryParams = new QueryParamsModel({}, 'asc', 'Name', this.page || 1, 10);
        this.appService.postBy('product/sizes/find', queryParams).subscribe(data => {
            this.sizes = data.items;
            this.count = data.totalCount;
        });
    }

    public onPageChanged(event) {
        this.page = event;
        window.scrollTo(0, 0);
    }

    public openDialog(data: any) {
        const dialogRef = this.dialog.open(SizesDialogComponent, {
            data: {
                size: data,
            },
            panelClass: ['theme-dialog'],
            autoFocus: false,
            direction: (this.settings.rtl) ? 'rtl' : 'ltr'
        });
        dialogRef.afterClosed().subscribe(response => {
            if (response) {
                this.getSizes();
            }
        });
    }

    public remove(size: any) {
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            maxWidth: "400px",
            data: {
                title: "Confirm Action",
                message: "Are you sure you want remove this size?"
            }
        });
        dialogRef.afterClosed().subscribe(dialogResult => {
            if (dialogResult) {
                this.appService.deleteBy(`product/sizes/delete/${size.sizeId}`)
                    .subscribe(
                        (data: ResponseModel) => {
                            if (data.code === ResponseStatus.Success) {
                                this.alertService.success(data.message);
                                this.getSizes();
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
        });
    }

}
