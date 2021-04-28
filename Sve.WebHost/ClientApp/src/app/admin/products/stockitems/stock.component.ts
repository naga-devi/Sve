import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Category } from 'src/app/app.models';
import { AppService } from 'src/app/app.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { AppSettings, Settings } from 'src/app/app.settings';
import { QueryParamsModel } from '../../../jx-core';
import { StockEditPriceDialogComponent } from './edit-price-dialog/edit-price-dialog';

@Component({
    selector: 'app-stock-items',
    templateUrl: './stock.component.html',
    styleUrls: ['./stock.component.scss']
})
export class StockListComponent implements OnInit, OnDestroy {
    public stockItems: Category[] = [];
    public page: any;
    public count = 0;
    public settings: Settings;
    private sub: any;
    @Input() productId: number = 0;
    public prerequisites: any;
    constructor(public appService: AppService, public dialog: MatDialog, public appSettings: AppSettings, private activatedRoute: ActivatedRoute) {
        this.settings = this.appSettings.settings;
    }

    ngOnInit(): void {
        this.sub = this.activatedRoute.params.subscribe(params => {
            if (params['id']) {
                this.productId = params['id'];
                this.getStockItems();
                this.loadPreRequisites();
            }
        });
    }

    public getStockItems() {
        const queryParams = new QueryParamsModel({}, 'asc', 'CreatedOn', 1, 500);
        this.appService.postBy(`product/${this.productId}/stockitems/find`, queryParams).subscribe(data => {
            this.stockItems = data.items ? data.items : [];
            this.count = data.totalCount;
        });
    }

    public onPageChanged(event) {
        this.page = event;
        window.scrollTo(0, 0);
    }    

    public editPrice(data: any) {
        const dialogRef = this.dialog.open(StockEditPriceDialogComponent, {
            data: {
                stockData: data,
                productId: this.productId
            },
            panelClass: ['theme-dialog'],
            autoFocus: false,
            direction: (this.settings.rtl) ? 'rtl' : 'ltr'
        });
        dialogRef.afterClosed().subscribe(response => {
            if (response) {
                this.getStockItems();
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
                const index: number = this.stockItems.indexOf(category);
                if (index !== -1) {
                    this.stockItems.splice(index, 1);
                }
            }
        });
    }

    public loadPreRequisites() {
        this.appService.getBy(`product/${this.productId}/stockitems/prerequisites`).subscribe(data => {
            this.prerequisites = data;
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }
}
