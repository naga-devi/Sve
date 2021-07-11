import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AppService } from 'src/app/app.service';
import { Category } from 'src/app/app.models';
import { ActivatedRoute, Router } from '@angular/router';
import { InputFile } from 'ngx-input-file';
import { NotificationService, ResponseModel, ResponseStatus } from '../../../../app/jx-core';


@Component({
    selector: 'app-add-product',
    templateUrl: './add-product.component.html',
    styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent implements OnInit {
    public form: FormGroup;
    public colors = [];
    public sizes = [];
    public selectedColors: string;
    public categories: Category[];
    private sub: any;
    public productId: any;
    deletedImageIds = [];
    public taxSlabs = [];

    constructor(public appService: AppService,
        public formBuilder: FormBuilder,
        private activatedRoute: ActivatedRoute,
        public router: Router,
        public alertService: NotificationService) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            'name': [null, Validators.compose([Validators.required, Validators.minLength(4)])],
            'images': null,
            "minimumStock": [null, Validators.required],
            "ratingsCount": null,
            "ratingsValue": null,
            "description": null,
            "hsn": [null, Validators.required],
            "taxSlabId": [2, Validators.required],
            "categoryId": [null, Validators.required]
        });
        this.getCategories();
        this.getTaxSlab();
        this.sub = this.activatedRoute.params.subscribe(params => {
            if (params['id']) {
                this.productId = params['id'];
                this.getProductById();
            }
        });
        this.getSubmitButtonTitle();
    }

    getSubmitButtonTitle=()=>{
        this.productId= this.productId || 0;
        if(this.productId > 0){
            return 'SAVE';
        }
        else{
            return 'SAVE AND ADD MORE';
        }
    }

    public getCategories() {
        this.appService.getBy('product-category/all').subscribe(data => {
            this.categories = data;
            this.categories.shift();
        });
    }

    public getTaxSlab = () => {
        this.appService.getBy('product/taxslabs/all').subscribe(data => {
            this.taxSlabs = data;
        });
    }

    public getProductById() {
        this.deletedImageIds = [];
        this.appService.getBy(`product/productdetails/id/${this.productId}`).subscribe((data: any) => {
            //console.log(data);
            this.form.patchValue(data);
            //this.selectedColors = data.color;
            const images: any[] = [];
            data.images.forEach((item: { medium: any; imageId: any; }) => {
                let image = {
                    link: item.medium,
                    preview: item.medium,
                    imageId: item.imageId
                }
                images.push(image);
            })
            this.form.controls.images.setValue(images);
        });
    }

    public onSubmit(saveAndContinue: boolean) {
        let newImages = [];
        if (this.form.controls.images.value && this.form.controls.images.value.length > 0) {
            this.form.controls.images.value.forEach((item: { imageId: number; preview: any; file: { name: any; }; }) => {
                let id = item.imageId || 0;
                if (id == 0) {
                    newImages.push({ data: item.preview, name: item.file.name })
                };
            })
        }
        let product = this.form.value;
        product.id = this.productId || 0;
        product.addedImages = newImages;
        product.deletedImages = this.deletedImageIds;
        //console.log(product);
        this.appService.postBy('product/productdetails/save', product).subscribe(
            (response: ResponseModel) => {
                if (response.code === ResponseStatus.Success) {
                    this.alertService.success(response.message);
                    if (saveAndContinue) {
                        //this.router.navigate([`/admin/products/add-product/${response.newId}`]);
                        this.router.navigate(['/admin/products/product-list']);
                    }
                    else if (product.id == 0) {
                        //window.location.reload();
                        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
                        this.router.navigate([this.router.url]);
                        //this.router.navigate(['/admin/products/add-product'], {re});
                    }
                    // else{
                    //     this.productId = response.newId;
                    //     this.getProductById();
                    // }
                }
                else {
                    this.alertService.error(response.message);
                }
            },
            err => {
                this.alertService.error(err);
            }
        );
    }

    public uploadImage(file: InputFile) {
        //console.log(this.form.controls.images.value);
        //const apiUrl = 'my-url';
        //const formData = new FormData();
        //formData.append('file', file.file, file.file.name);
        //console.log(file);
        //console.log(file.file.name);
        //this.appService.postForm(apiUrl, formData).subscribe(
        //    (response: ResponseModel) => {
        //        if (response.code === ResponseStatus.Success) {
        //            this.alertService.success(response.message);
        //        }
        //        else {
        //            this.alertService.error(response.message);
        //        }
        //    },
        //    err => {
        //        this.alertService.error(err);
        //    }
        //);
    }

    public deleteImage(file: { imageId: number; }) {
        if (file && file.imageId && file.imageId > 0)
            this.deletedImageIds.push(file.imageId);
        //console.log(file);
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }
}
