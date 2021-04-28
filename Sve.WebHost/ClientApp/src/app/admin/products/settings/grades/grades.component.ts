import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/app.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { AppSettings, Settings } from 'src/app/app.settings';
import { NotificationService, QueryParamsModel , ResponseModel, ResponseStatus } from '../../../../jx-core';
import { GradesDialogComponent } from './grades-dialog/grades-dialog.component';

@Component({
    selector: 'app-grades',
    templateUrl: './grades.component.html',
    styleUrls: ['./grades.component.scss']
})
export class GradesComponent implements OnInit {
    public brands: any[] = [];
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
        this.appService.postBy('product/grades/find', queryParams).subscribe(data => {
            this.brands = data.items;
            this.count = data.totalCount;
        });
    }

    public onPageChanged(event) {
        this.page = event;
        window.scrollTo(0, 0);
    }

    public openEditDialog(data: any) {
        const dialogRef = this.dialog.open(GradesDialogComponent, {
            data: {
                grade: data,
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
                message: "Are you sure you want remove this Grades?"
            }
        });
        dialogRef.afterClosed().subscribe(dialogResult => {
            if (dialogResult) {
                this.appService.deleteBy(`product/grades/delete/${row.gradeId}`)
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
