import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { SwiperConfigInterface, SwiperDirective } from 'ngx-swiper-wrapper';
import { Data, AppService } from '../../../app.service';
import { Product } from "../../../app.models";
import { emailValidator } from '../../../theme/utils/app-validators';
import { ProductZoomComponent } from './product-zoom/product-zoom.component';

@Component({
    selector: 'app-product',
    templateUrl: './product.component.html',
    styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit {
    @ViewChild('zoomViewer', { static: true }) zoomViewer;
    @ViewChild(SwiperDirective, { static: true }) directiveRef: SwiperDirective;
    public config: SwiperConfigInterface = {};
    public product: Product;
    public image: any;
    public zoomImage: any;
    private sub: any;
    public form: FormGroup;
    public relatedProducts: Array<Product>;
    public productTitle = '';
    constructor(public appService: AppService, private activatedRoute: ActivatedRoute, public dialog: MatDialog, public formBuilder: FormBuilder) { }

    ngOnInit() {
        this.sub = this.activatedRoute.params.subscribe(params => {
            this.getProductById(params['id']); // (params['id']=> productBaseId
        });
        this.form = this.formBuilder.group({
            'review': [null, Validators.required],
            'name': [null, Validators.compose([Validators.required, Validators.minLength(4)])],
            'email': [null, Validators.compose([Validators.required, emailValidator])]
        });
        this.getRelatedProducts();
    }

    ngAfterViewInit() {
        this.config = {
            observer: false,
            slidesPerView: 4,
            spaceBetween: 10,
            keyboard: true,
            navigation: true,
            pagination: false,
            loop: false,
            preloadImages: false,
            lazy: true,
            breakpoints: {
                480: {
                    slidesPerView: 2
                },
                600: {
                    slidesPerView: 3,
                }
            }
        }
    }

    public getProductById(id) {
        this.appService.getProductById(id).subscribe(data => {
            this.product = data;
            this.image = data.images[0].medium;
            this.zoomImage = data.images[0].big;
            setTimeout(() => {
                this.config.observer = true;
                // this.directiveRef.setIndex(0);
            });
        });
    }

    public getRelatedProducts() {
        // this.appService.getProducts('related').subscribe(data => {
        //     this.relatedProducts = data;
        // })
    }

    public selectImage(image) {
        this.image = image.medium;
        this.zoomImage = image.big;
    }

    public onMouseMove(e) {
        if (window.innerWidth >= 1280) {
            var image, offsetX, offsetY, x, y, zoomer;
            image = e.currentTarget;
            offsetX = e.offsetX;
            offsetY = e.offsetY;
            x = offsetX / image.offsetWidth * 100;
            y = offsetY / image.offsetHeight * 100;
            zoomer = this.zoomViewer.nativeElement.children[0];
            if (zoomer) {
                zoomer.style.backgroundPosition = x + '% ' + y + '%';
                zoomer.style.display = "block";
                zoomer.style.height = image.height + 'px';
                zoomer.style.width = image.width + 'px';
            }
        }
    }

    public onMouseLeave(event) {
        this.zoomViewer.nativeElement.children[0].style.display = "none";
    }

    public openZoomViewer() {
        this.dialog.open(ProductZoomComponent, {
            data: this.zoomImage,
            panelClass: 'zoom-dialog'
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    getProductPriceBySize(sizeId) {
        if (this.product) {
            this.product.sizes.forEach(x => {
                x.selected = sizeId == x.id;
            });
            this.appService.getProductPrice(this.product.productBaseId, sizeId, this.product.brandId, this.product.materialTypeId, this.product.gradeId)
                .subscribe(data => {
                    this.product.sizeId = sizeId;
                    this.product.id = data.id;
                    this.product.newPrice = data.newPrice;
                    this.product.discount = data.discount;
                    this.product.oldPrice = data.oldPrice;
                    this.product.availibilityCount = data.availibilityCount;


                    this.product.unitPrice = data.unitPrice;
                    this.product.cgst = data.cgst;
                    this.product.sgst = data.sgst;
                    //this.product.mrp = data.mrp;
                    // this.product.materialTypeId = data.materialTypeId;
                    // this.product.brandId = data.brandId;
                    // this.product.colorId = data.colorId;
                    // this.product.gradeId = data.gradeId;
                });
        }
        //console.log(this.product);
    }

    getProductPriceByBrand(brandId) {
        let tempProduct = this.product;
        if (tempProduct) {
            tempProduct.brands.forEach(x => {
                x.selected = brandId == x.id;
            });

            this.appService.getProductPrice(tempProduct.productBaseId, tempProduct.sizeId, brandId, tempProduct.materialTypeId, tempProduct.gradeId)
                .subscribe(data => {
                    tempProduct.brandId = brandId;
                    tempProduct.availibilityCount = data.availibilityCount;
                    tempProduct.id = data.id;
                    tempProduct.newPrice = data.newPrice;
                    tempProduct.discount = data.discount;
                    tempProduct.oldPrice = data.oldPrice;

                    this.product.unitPrice = data.unitPrice;
                    this.product.cgst = data.cgst;
                    this.product.sgst = data.sgst;

                    this.product = tempProduct;
                });

        }
        //console.log(this.product);
    }

    getProductPriceByMaterialType = (materialTypeId: number) => {
        if (this.product) {
            this.product.materialTypes.forEach(x => {
                x.selected = materialTypeId == x.id;
            });

            this.appService.getProductPrice(this.product.productBaseId, this.product.sizeId, this.product.brandId, materialTypeId, this.product.gradeId)
                .subscribe(data => {
                    this.product.materialTypeId = materialTypeId;
                    this.product.availibilityCount = data.availibilityCount;
                    this.product.id = data.id;
                    this.product.newPrice = data.newPrice;
                    this.product.discount = data.discount;
                    this.product.oldPrice = data.oldPrice;

                    this.product.unitPrice = data.unitPrice;
                    this.product.cgst = data.cgst;
                    this.product.sgst = data.sgst;
                });

        }
    }

    getProductPriceByGrade = (gradeId: number) => {
        if (this.product) {
            this.product.grades.forEach(x => {
                x.selected = gradeId == x.id;
            });

            this.appService.getProductPrice(this.product.productBaseId, this.product.sizeId, this.product.brandId, this.product.materialTypeId, gradeId)
                .subscribe(data => {
                    this.product.gradeId = gradeId;
                    this.product.availibilityCount = data.availibilityCount;
                    this.product.id = data.id;
                    this.product.newPrice = data.newPrice;
                    this.product.discount = data.discount;
                    this.product.oldPrice = data.oldPrice;

                    this.product.unitPrice = data.unitPrice;
                    this.product.cgst = data.cgst;
                    this.product.sgst = data.sgst;
                });

        }
    }

    getProductTitle() {
        return `${this.product.brands.filter(x => x.selected)[0].name} ${this.product.materialTypes.filter(x => x.selected)[0].name} ${this.product.sizes.filter(x => x.selected)[0].name} ${this.product.name}`;
    }
    public onSubmit(values: Object): void {
        if (this.form.valid) {
            //email sent
        }
    }
}