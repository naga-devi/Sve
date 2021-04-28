import { Component, OnInit } from '@angular/core';
import { MaterialtypesDialogComponent } from './materialtypes-dialog/materialtypes-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { AppSettings, Settings } from 'src/app/app.settings';
import { AppService } from '../../../../app.service';
import { NotificationService, QueryParamsModel, ResponseModel, ResponseStatus } from '../../../../jx-core';

@Component({
    selector: 'app-materialtypes',
    templateUrl: './materialtypes.component.html',
    styleUrls: ['./materialtypes.component.scss']
})
export class MaterialTypesComponent implements OnInit {
    public materialTypes: any[] = [];
    public page: any;
    public count = 0;
    public settings: Settings;
    constructor(public appService: AppService, public dialog: MatDialog, public appSettings: AppSettings, public alertService: NotificationService) {
        this.settings = this.appSettings.settings;
    }

    ngOnInit(): void {
        this.getMaterialTypes();
    }

    public getMaterialTypes() {
        const queryParams = new QueryParamsModel({}, 'asc', 'Name', this.page || 1, 10);
        this.appService.postBy('product/materialtypes/find', queryParams).subscribe(data => {
            this.materialTypes = data.items;
            this.count = data.totalCount;
        });
    }

    public onPageChanged(event) {
        this.page = event;
        window.scrollTo(0, 0);
    }

    public openDialog(data: any) {
        const dialogRef = this.dialog.open(MaterialtypesDialogComponent, {
            data: {
                materialType: data,
            },
            panelClass: ['theme-dialog'],
            autoFocus: false,
            direction: (this.settings.rtl) ? 'rtl' : 'ltr'
        });
        dialogRef.afterClosed().subscribe(response => {
            if (response) {
                this.getMaterialTypes();
            }
        });
    }

    public remove(row: any) {
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            maxWidth: "400px",
            data: {
                title: "Confirm Action",
                message: "Are you sure you want remove this Material Type?"
            }
        });
        dialogRef.afterClosed().subscribe(dialogResult => {
            if (dialogResult) {
                this.appService.deleteBy(`product/materialtypes/delete/${row.materialTypeId}`)
                    .subscribe(
                        (data: ResponseModel) => {
                            if (data.code === ResponseStatus.Success) {
                                this.alertService.success(data.message);
                                this.getMaterialTypes();
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
