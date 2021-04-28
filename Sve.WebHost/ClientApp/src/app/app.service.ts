import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Category, Product, ProductFilter, CheckOutModel } from './app.models';
import { environment } from '../environments/environment'
import { QueryResultsModel } from './jx-core';

export class Data {
    constructor(public categories: Category[],
        public compareList: Product[],
        public wishList: Product[],
        public cartList: Product[],
        public totalPrice: number,
        public totalCartCount: number) { }
}

@Injectable()
export class AppService {
    public Data = new Data(
        [], // categories
        [], // compareList
        [],  // wishList
        [],  // cartList
        null, //totalPrice,
        0 //totalCartCount
    )
    public url = "assets/data/";
    constructor(public http: HttpClient, public snackBar: MatSnackBar) {
        if (this.Data.cartList.length === 0) {
            localStorage.removeItem('cart');
        }
    }

    public getCategories(): Observable<Category[]> {
        if (this.Data.categories && this.Data.categories.length > 0) {
            //return of(this.Data.categories);
            return Observable.create((observer) => {

                // observable execution
                observer.next(this.Data.categories)
                observer.complete()
            });
        }
        else {
            return this.http.get<Category[]>(`${environment.apiUrl}product-category/all`);
        }
    }

    public getProducts(filter?: ProductFilter): Observable<QueryResultsModel> {
        //console.log(filter);
        if (!filter) {
            filter = new ProductFilter();
            filter.pageNumber = 1;
            filter.pageSize = 6;
        }
        return this.http.post<QueryResultsModel>(`${environment.apiUrl}v1/cart/search`, filter);
    }

    public getFilterTypesByCategory(categoryId: number): Observable<any> {
        return this.http.get<any>(`${environment.apiUrl}v1/cart/filter-types/${categoryId}`);
    }

    public getProductById(id): Observable<Product> {
        return this.http.get<Product>(`${environment.apiUrl}v1/cart/view/${id}`);
    }

    public getProductPrice(productId: number, sizeId: number, brandId: number, materialTypeId: number, gradeId: number): Observable<Product> {
        return this.http.get<Product>(`${environment.apiUrl}v1/cart/product-price/${productId}/${sizeId}/${brandId}/${materialTypeId}/${gradeId}`);
    }

    public placeOrder(orderDetails: CheckOutModel[]): Observable<any> {
        return this.http.post<any>(`${environment.apiUrl}v1/cart/place-order`, orderDetails);
    }

    public getBanners(): Observable<any[]> {
        return this.http.get<any[]>(this.url + 'banners.json');
    }

    public getBy(url): Observable<any> {
        return this.http.get<any>(`${environment.apiUrl}${url}`);
    }

    public deleteBy(url): Observable<any> {
        return this.http.delete<any>(`${environment.apiUrl}${url}`);
    }

    public postBy(url, postData, isPut: boolean = false): Observable<any> {
        if (isPut)
            return this.http.put<any>(`${environment.apiUrl}${url}`, postData);
        else
            return this.http.post<any>(`${environment.apiUrl}${url}`, postData);
    }

    public postForm(url, formData): Observable<any> {
        return this.http.post<any>(`${environment.apiUrl}${url}`, formData, {
            reportProgress: true,
            //responseType: 'json'
            observe: 'events'
        });
    }

    public addToCompare(product: Product) {
        let message, status;
        if (this.Data.compareList.filter(item => item.id == product.id)[0]) {
            message = 'The product ' + product.name + ' already added to comparison list.';
            status = 'error';
        }
        else {
            this.Data.compareList.push(product);
            message = 'The product ' + product.name + ' has been added to comparison list.';
            status = 'success';
        }
        this.snackBar.open(message, '×', { panelClass: [status], verticalPosition: 'top', duration: 3000 });
    }

    public addToWishList(product: Product) {
        let message, status;
        if (this.Data.wishList.filter(item => item.id == product.id)[0]) {
            message = 'The product ' + product.name + ' already added to wish list.';
            status = 'error';
        }
        else {
            this.Data.wishList.push(product);
            message = 'The product ' + product.name + ' has been added to wish list.';
            status = 'success';
        }
        this.snackBar.open(message, '×', { panelClass: [status], verticalPosition: 'top', duration: 3000 });
    }

    public addToCart(product: Product) {
        let message, status;

        this.Data.totalPrice = null;
        this.Data.totalCartCount = null;

        if (this.Data.cartList.length === 0) {
            localStorage.removeItem('cart');
        }

        if (this.Data.cartList.filter(item => item.id == product.id)[0]) {
            let item = this.Data.cartList.filter(item => item.id == product.id)[0];
            item.cartCount = product.cartCount;
        }
        else {
            this.Data.cartList.push(product);
        }
        this.Data.cartList.forEach(product => {
            this.Data.totalPrice = this.Data.totalPrice + (product.cartCount * product.newPrice);
            this.Data.totalCartCount = this.Data.totalCartCount + product.cartCount;
        });

        message = 'The product ' + product.name + ' has been added to cart.';
        status = 'success';
        this.snackBar.open(message, '×', { panelClass: [status], verticalPosition: 'top', duration: 3000 });
        localStorage.setItem('cart', JSON.stringify(this.Data.cartList));
    }

    public resetProductCartCount(product: Product) {
        product.cartCount = 0;        
        let compareProduct = this.Data.compareList.filter(item => item.id == product.id)[0];
        if (compareProduct) {
            compareProduct.cartCount = 0;
        };
        let wishProduct = this.Data.wishList.filter(item => item.id == product.id)[0];
        if (wishProduct) {
            wishProduct.cartCount = 0;
        };
    }

    public getBrands() {
        return [
            { name: 'aloha', image: 'assets/images/brands/aloha.png' },
            { name: 'dream', image: 'assets/images/brands/dream.png' },
            { name: 'congrats', image: 'assets/images/brands/congrats.png' },
            { name: 'best', image: 'assets/images/brands/best.png' },
            { name: 'original', image: 'assets/images/brands/original.png' },
            { name: 'retro', image: 'assets/images/brands/retro.png' },
            { name: 'king', image: 'assets/images/brands/king.png' },
            { name: 'love', image: 'assets/images/brands/love.png' },
            { name: 'the', image: 'assets/images/brands/the.png' },
            { name: 'easter', image: 'assets/images/brands/easter.png' },
            { name: 'with', image: 'assets/images/brands/with.png' },
            { name: 'special', image: 'assets/images/brands/special.png' },
            { name: 'bravo', image: 'assets/images/brands/bravo.png' }
        ];
    }
} 