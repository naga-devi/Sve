import { Component, OnInit } from '@angular/core';
import { Category } from 'src/app/app.models';
import { AppService } from 'src/app/app.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { AppSettings, Settings } from 'src/app/app.settings';
import { NotificationService, QueryParamsModel , ResponseModel, ResponseStatus } from '../../../../jx-core';
import { ColorsDialogComponent } from './colors-dialog/colors-dialog.component';

@Component({
    selector: 'app-colors',
    templateUrl: './colors.component.html',
    styleUrls: ['./colors.component.scss']
})
export class ColorsComponent implements OnInit {
    public colors: any[] = [];
    public page: any;
    public count = 0;
    public settings: Settings;
    constructor(public appService: AppService, public dialog: MatDialog, public appSettings: AppSettings,
        public alertService: NotificationService) {
        this.settings = this.appSettings.settings;
    }

    ngOnInit(): void {
        this.getBrands();
    }

    public getBrands() {
        const queryParams = new QueryParamsModel({}, 'asc', 'Name', this.page || 1, 10);
        this.appService.postBy('product/colors/find', queryParams).subscribe(data => {
            this.colors = data.items;
            this.count = data.totalCount;
        });
    }

    public onPageChanged(event) {
        this.page = event;
        window.scrollTo(0, 0);
    }

    public openEditDialog(data: any) {
        const dialogRef = this.dialog.open(ColorsDialogComponent, {
            data: {
                color: data,
            },
            panelClass: ['theme-dialog'],
            autoFocus: false,
            direction: (this.settings.rtl) ? 'rtl' : 'ltr'
        });
        dialogRef.afterClosed().subscribe(response => {
            if (response) {
                this.getBrands();
            }
        });
    }

    public remove(row: any) {
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            maxWidth: "400px",
            data: {
                title: "Confirm Action",
                message: "Are you sure you want remove this Color?"
            }
        });
        dialogRef.afterClosed().subscribe(dialogResult => {
            if (dialogResult) {
                this.appService.deleteBy(`product/colors/delete/${row.colorId}`)
                    .subscribe(
                        (data: ResponseModel) => {
                            if (data.code === ResponseStatus.Success) {
                                this.alertService.success(data.message);
                                this.getBrands();
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
