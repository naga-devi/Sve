/// <reference path="settings/colors/colors.component.ts" />
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { SwiperModule } from 'ngx-swiper-wrapper';
import { InputFileModule } from 'ngx-input-file';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ProductZoomComponent } from './product-detail/product-zoom/product-zoom.component';
import { AddProductComponent } from './add-product/add-product.component';
import { StockListComponent } from './stockitems/stock.component';
import { StockEditPriceDialogComponent } from './stockitems/edit-price-dialog/edit-price-dialog';
import { CategoriesComponent } from './settings/categories/categories.component';
import { BrandsComponent } from './settings/brands/brands.component';
import { SizesComponent } from './settings/sizes/sizes.component';
import { MaterialTypesComponent } from './settings/materialtypes/materialtypes.component';
import { CategoryDialogComponent } from './settings/categories/category-dialog/category-dialog.component';
import { SizesDialogComponent } from './settings/sizes/sizes-dialog/sizes-dialog.component';
import { MaterialtypesDialogComponent } from './settings/materialtypes/materialtypes-dialog/materialtypes-dialog.component';
import { BrandsDialogComponent } from './settings/brands/brands-dialog/brands-dialog.component';

import { ColorsComponent } from './settings/colors/colors.component';
import { ColorsDialogComponent } from './settings/colors/colors-dialog/colors-dialog.component';
import { GradesComponent } from './settings/grades/grades.component';
import { GradesDialogComponent } from './settings/grades/grades-dialog/grades-dialog.component';

export const routes = [
    { path: '', redirectTo: 'product-list', pathMatch: 'full' },
    { path: 'categories', component: CategoriesComponent, data: { breadcrumb: 'Categories' } },
    { path: 'category-brands', component: BrandsComponent, data: { breadcrumb: 'Brands' } },
    { path: 'category-colors', component: ColorsComponent, data: { breadcrumb: 'Colors' } },
    { path: 'category-grades', component: GradesComponent, data: { breadcrumb: 'Grades' } },
    { path: 'category-sizes', component: SizesComponent, data: { breadcrumb: 'Sizes' } },
    { path: 'category-material-types', component: MaterialTypesComponent, data: { breadcrumb: 'Material Types' } },
    { path: 'product-list', component: ProductListComponent, data: { breadcrumb: 'Product List' } },
    { path: 'product-detail', component: ProductDetailComponent, data: { breadcrumb: 'Product Detail' } },
    { path: 'product-detail/:id', component: ProductDetailComponent, data: { breadcrumb: 'Product Detail' } },
    { path: 'add-product', component: AddProductComponent, data: { breadcrumb: 'Add Product' } },
    { path: 'add-product/:id', component: AddProductComponent, data: { breadcrumb: 'Edit Product' } },
];

@NgModule({
    declarations: [
        ProductListComponent,
        ProductDetailComponent,
        ProductZoomComponent,
        AddProductComponent,
        CategoriesComponent,
        CategoryDialogComponent,
        StockListComponent,
        StockEditPriceDialogComponent,
        SizesComponent,
        SizesDialogComponent,
        MaterialTypesComponent,
        MaterialtypesDialogComponent,
        BrandsComponent,
        BrandsDialogComponent,
        MaterialTypesComponent,
        MaterialtypesDialogComponent,
        SizesComponent,
        SizesDialogComponent,
        ColorsComponent,
        ColorsDialogComponent,
        GradesComponent,
        GradesDialogComponent
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        ReactiveFormsModule,
        SharedModule,
        NgxPaginationModule,
        SwiperModule,
        InputFileModule
    ]
})
export class ProductsModule { }
