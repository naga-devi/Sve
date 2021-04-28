import { Component, OnInit, ViewChild, HostListener, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ProductDialogComponent } from '../../shared/products-carousel/product-dialog/product-dialog.component';
import { AppService } from '../../app.service';
import { Product, Category, ProductFilter } from "../../app.models";
import { Settings, AppSettings } from 'src/app/app.settings';
import { MessageService } from '../../jx-core';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit, OnDestroy {
    @ViewChild('sidenav', { static: true }) sidenav: any;
    public sidenavOpen: boolean = true;
    private sub: any;
    public viewType: string = 'grid';
    public viewCol: number = 25;
    public counts = [12, 24, 36];
    public count: any = 12;
    public totalItems = 0;
    public sortings = [{ id: 1, name: 'Sort by Default' }, { id: 2, name: 'Best match' }, { id: 3, name: 'Lowest first' }, { id: 4, name: 'Highest first' }];
    public sortId: number;
    public sortName: string;
    public products: Array<Product> = [];
    public categories: Category[];
    public priceFrom: number = 0;
    public priceTo: number = 0;
    public brands = [];
    public sizes = [];
    public materialTypes = [];
    public grades = [];
    public colors = [];
    public page: any = 1;
    public settings: Settings;
    showEmpty = true;
    public productFilter = new ProductFilter();
    public enableActionLinks = false;
    subscription: Subscription;
    constructor(public appSettings: AppSettings,
        private activatedRoute: ActivatedRoute,
        public appService: AppService,
        public dialog: MatDialog,
        private router: Router,
        public messageService: MessageService) {
        this.settings = this.appSettings.settings;
    }

    ngOnInit() {
        this.count = this.counts[0];
        this.sortId = this.sortings[0].id;
        this.sortName = this.sortings[0].name;
        this.productFilter.categoryId = 0;

        this.activatedRoute.queryParams.subscribe(params => {
            this.productFilter.name = this.isUndefined(params['q']) ? '' : params['q'];
            this.productFilter.categoryId = this.isUndefined(params['cat']) ? 0 : parseInt(params['cat'], 0);
        });
        if (this.productFilter.categoryId == 0) {
            this.sub = this.activatedRoute.params.subscribe(params => {
                //var name = this.isUndefined(params['name']) ? '' : params['name'];
                this.productFilter.categoryId = this.isUndefined(params['id']) ? 0 : parseInt(params['id'], 0);
            });
        }
        if (window.innerWidth < 960) {
            this.sidenavOpen = false;
        };
        if (window.innerWidth < 1280) {
            this.viewCol = 33.3;
        };

        this.productFilter.sortBy = this.sortId;
        this.productFilter.pageSize = 6;
        this.productFilter.pageNumber = 1;
        this.productFilter.startPrice = 0;
        this.productFilter.endPrice = 0;

        this.getCategories(this.productFilter.categoryId);
        //this.getBrands();
        this.getProducts();

        // triggers when search changed
        this.subscription = this.messageService.getMessage().subscribe(message => {
            if (message) {
                this.productFilter.name = message.q;
                this.productFilter.categoryId = message.cat;
                this.getCategories(this.productFilter.categoryId);
                this.getProducts();
            }
        });
    }

    public getProducts() {
        this.showEmpty = false;
        this.enableActionLinks = false;

        if (this.sizes && this.sizes.length > 0) {
            let selectedIds = [];
            this.sizes.filter(x => x.selected).forEach(x => {
                selectedIds.push(x.sizeId);
            });
            this.productFilter.SizeIds = selectedIds;
        }

        if (this.brands && this.brands.length > 0) {
            let selectedIds = [];
            this.brands.filter(x => x.selected).forEach(x => {
                selectedIds.push(x.brandId);
            });;
            this.productFilter.brandIds = selectedIds;
        }

        if (this.materialTypes && this.materialTypes.length > 0) {
            let selectedIds = [];
            this.materialTypes.filter(x => x.selected).forEach(x => {
                selectedIds.push(x.materialTypeId);
            });;
            this.productFilter.materialTypeIds = selectedIds;
        }

        if (this.grades && this.grades.length > 0) {
            let selectedIds = [];
            this.grades.filter(x => x.selected).forEach(x => {
                selectedIds.push(x.gradeId);
            });;
            this.productFilter.gradeIds = selectedIds;
        }

        if (this.colors && this.colors.length > 0) {
            let selectedIds = [];
            this.colors.filter(x => x.selected).forEach(x => {
                selectedIds.push(x.colorId);
            });;
            this.productFilter.colorIds = selectedIds;
        }

        this.appService.getProducts(this.productFilter).subscribe(data => {
            this.showEmpty = true;
            this.products = data.items ? data.items : [];
            this.totalItems = data.totalCount;
            this.enableActionLinks = this.products.length > 0;
        });
    }

    public getCategories(categoryId: number) {
        this.categories = [];
        this.appService.getFilterTypesByCategory(categoryId).subscribe(data => {
            this.categories = data.categories;
            this.brands = data.brands;
            this.sizes = data.sizes;
            this.grades = data.grades;
            if (this.grades && this.grades.length > 0) {
                this.grades = this.grades.shift();
            }
            this.colors = data.colors;
            if (this.colors && this.colors.length > 0) {
                this.colors = this.colors.shift();
            }
            this.materialTypes = data.materialTypes;
        });
    }

    public getBrands() {
        this.brands = this.appService.getBrands();
        this.brands.forEach(brand => { brand.selected = false });
    }

    ngOnDestroy() {
        if (this.sub) this.sub.unsubscribe();
        // unsubscribe to ensure no memory leaks
        this.subscription.unsubscribe();
    }

    @HostListener('window:resize')
    public onWindowResize(): void {
        (window.innerWidth < 960) ? this.sidenavOpen = false : this.sidenavOpen = true;
        (window.innerWidth < 1280) ? this.viewCol = 33.3 : this.viewCol = 25;
    }

    public changeCount(count) {
        this.count = count;
        this.productFilter.pageSize = this.count;
        this.getProducts();
    }

    public changeSorting(sort) {
        this.sortId = sort.id;
        this.sortName = sort.name;
        this.productFilter.sortBy = this.sortId;
        this.getProducts();
    }

    public changeViewType(viewType, viewCol) {
        this.viewType = viewType;
        this.viewCol = viewCol;
    }

    public openProductDialog(product) {
        let dialogRef = this.dialog.open(ProductDialogComponent, {
            data: product,
            panelClass: 'product-dialog',
            direction: (this.settings.rtl) ? 'rtl' : 'ltr'
        });
        dialogRef.afterClosed().subscribe(product => {
            if (product) {
                this.router.navigate(['/products', product.id, product.name]);
            }
        });
    }

    public onPageChanged(event) {
        this.page = event;
        this.productFilter.pageNumber = this.page;
        this.getProducts();
        window.scrollTo(0, 0);
    }

    public onChangeCategory(category: Product) {
        if (category.id > 0) {
            //this.router.navigate(['/products', event.target.innerText.toLowerCase()]);
            this.router.navigate(['/products', category.id, category.name.toLowerCase()]);
        }
    }

    public onStartPriceChange(event: any) {
        //console.log(event.value);
        this.productFilter.startPrice = event.value;
        if ((event.value > 0 && this.productFilter.endPrice == 0) || (event.value > 0 && this.productFilter.endPrice > event.value)) {
            this.getProducts();
        }
    }

    public onEndPriceChange(event: any) {
        if ((event.value > 0 && this.productFilter.startPrice == 0) || (this.productFilter.startPrice < event.value)) {
            this.getProducts();
        }
    }

    public brandChanged() {
        this.getProducts();
    }

    public sizeChanged() {
        this.getProducts();
    }

    public materialChanged() {
        this.getProducts();
    }

    public gradeChanged() {
        this.getProducts();
    }

    public colorChanged() {
        this.getProducts();
    }

    isUndefined(value) {
        // Obtain `undefined` value that's
        // guaranteed to not have been re-assigned
        var undefined = void (0);
        return value === undefined;
    }
}
