import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Data, AppService } from '../../app.service';
import { Product } from '../../app.models';

@Component({
    selector: 'app-controls',
    templateUrl: './controls.component.html',
    styleUrls: ['./controls.component.scss']
})
export class ControlsComponent implements OnInit {
    @Input() product: Product;
    @Input() type: string;
    @Output() onOpenProductDialog: EventEmitter<any> = new EventEmitter();
    @Output() onQuantityChange: EventEmitter<any> = new EventEmitter<any>();
    public count: number = 1;
    public align = 'center center';
    public form: FormGroup;
    constructor(public appService: AppService, public snackBar: MatSnackBar, public fb: FormBuilder) { }

    ngOnInit() {
        this.form = this.fb.group({
            quantity: null
        });

        if (this.product) {
            if (this.product.cartCount > 0) {
                this.count = this.product.cartCount;
                this.form.patchValue({quantity: this.count});
            }
        }        
        this.layoutAlign();
    }

    public layoutAlign() {
        if (this.type == 'all') {
            this.align = 'space-between center';
        }
        else if (this.type == 'wish') {
            this.align = 'start center';
        }
        else {
            this.align = 'center center';
        }
    }



    //public increment(count) {
    //    if (this.count < this.product.availibilityCount) {
    //        this.count++;
    //        let obj = {
    //            productBaseId: this.product.productBaseId,
    //            productId: this.product.id,
    //            soldQuantity: this.count,
    //            total: this.count * this.product.newPrice
    //        }
    //        this.changeQuantity(obj);
    //    }
    //    else {
    //        this.snackBar.open('You can not choose more items than available. In stock ' + this.count + ' items.', '×', { panelClass: 'error', verticalPosition: 'top', duration: 3000 });
    //    }
    //}

    //public decrement(count) {
    //    if (this.count > 1) {
    //        this.count--;
    //        let obj = {
    //            productBaseId: this.product.productBaseId,
    //            productId: this.product.id,
    //            soldQuantity: this.count,
    //            total: this.count * this.product.newPrice
    //        }
    //        this.changeQuantity(obj);
    //    }
    //}

    public quantityChanged(quanity: any) {
        this.count = parseInt(quanity, 0);
        if (this.count <= this.product.availibilityCount) {
            let obj = {
                productBaseId: this.product.productBaseId,
                productId: this.product.id,
                soldQuantity: this.count,
                total: this.count * this.product.newPrice
            }
            this.changeQuantity(obj);
        }
        else {
            this.snackBar.open('You can not choose more items than available. In stock ' + this.product.availibilityCount + ' items.', '×', { panelClass: 'error', verticalPosition: 'top', duration: 3000 });
        }
    }

    public addToCompare(product: Product) {
        this.appService.addToCompare(product);
    }

    public addToWishList(product: Product) {
        this.appService.addToWishList(product);
    }

    public addToCart(product: Product) {
        if (localStorage.getItem('cart') != null) {
            const cartItems = JSON.parse(localStorage.getItem('cart'));
            if (cartItems && cartItems.length > 0) {
                this.appService.Data.cartList = cartItems;
            }
        }
        // console.log(product)
        let currentProduct = this.appService.Data.cartList.filter(item => item.id == product.id)[0];

        if (currentProduct) {
            if ((currentProduct.cartCount + this.count) <= this.product.availibilityCount) {
                product.cartCount = currentProduct.cartCount + this.count;
            }
            else {
                this.snackBar.open('You can not add more items than available. In stock ' + this.product.availibilityCount + ' items and you already added ' + currentProduct.cartCount + ' item to your cart', '×', { panelClass: 'error', verticalPosition: 'top', duration: 5000 });
                return false;
            }
        }
        else {
            product.cartCount = this.count;
        }
        this.form.patchValue({
            quantity: null
        });
        this.appService.addToCart(product);
    }

    public openProductDialog(event) {
        this.onOpenProductDialog.emit(event);
    }

    public changeQuantity(value) {
        this.onQuantityChange.emit(value);
    }

}