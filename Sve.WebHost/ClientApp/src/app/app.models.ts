export class Category {
    constructor(public id: number,
        public name: string,
        public hasSubCategory: boolean,
        public parentId: number) { }
}


export class Product {
    constructor(
        public id: number, // stockGroupId
        public productBaseId: number,
        public name: string,
        public images: Array<any>,
        public oldPrice: number,
        public newPrice: number,
        public discount: number,
        public unitPrice: number,
        public cgst: number,
        public sgst: number,
        public ratingsCount: number,
        public ratingsValue: number,
        public description: string,
        public availibilityCount: number,
        public cartCount: number,
        public weight: number,
        public categoryId: number,
        public sizeId: number,
        public sizes: Array<any>,
        public brandId: number,
        public brands: Array<any>,
        public materialTypeId: number,
        public materialTypes: Array<any>,
        public gradeId: number,
        public grades: Array<any>,
        public colorId: number,
        public colors: Array<any>,
        public taxSlabId: number
    ) { }
}

export class ProductFilter {
    public categoryId: number;
    public name: string;
    public startPrice: number;
    public endPrice: number;
    public SizeIds: Array<number>;
    public brandIds: Array<number>;
    public materialTypeIds: Array<number>;
    public gradeIds: Array<number>;
    public colorIds: Array<number>;
    public sortBy: number;
    public pageNumber: number;
    public pageSize: number;
}

export class CheckOutModel {
    constructor(
        public id: number, // stockGroupId
        public categoryId: number,
        public taxSlabId: number,
        public name: string,
        public productBaseId: number,
        public quantity: number,
        public unitPrice: number,       
        public cgst: number,
        public sgst: number) { }
}
