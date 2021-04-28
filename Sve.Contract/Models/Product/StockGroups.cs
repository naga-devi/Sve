namespace Sve.Contract.Models.Product
{
    using Sve.Contract.Models.Purchasing;
    using Sve.Contract.Models.Sales;
    using System;
    using System.Collections.Generic;

    public class StockGroups
    {
        public int? ProductId { get; set; }
        public int StockGroupId { get; set; }
        public int? MaterialTypeId { get; set; }
        public int SizeId { get; set; }
        public int BrandId { get; set; }
        public int? GradeId { get; set; }
        public int? ColorId { get; set; }
        public decimal NetPrice { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? Discount { get; set; }
        public decimal? SellPrice { get; set; }
        public int? Status { get; set; }
        public decimal? BasicMrp { get; set; }
        public string Description { get; set; }

        public decimal Cgst { get; set; }
        public decimal Sgst { get; set; }
        public bool? IsPrime { get; set; }
        public short? MinimumStock { get; set; }
        public byte? RatingsCount { get; set; }
        public short? RatingsValue { get; set; }

        public decimal? NewPrice
        {
            get
            {
                SellPrice = SellPrice ?? 0;
                return SellPrice > 0 ? Math.Round((decimal)SellPrice, GlobalContants.MoneyRoundDecimals) : SellPrice;
            }
        }

        public decimal? OldPrice => Discount > 0 ? Mrp : null;

        public Brands Brand { get; set; }
        public MaterialTypes MaterialType { get; set; }
        public ProductDetails Product { get; set; }
        public Sizes Size { get; set; }
        public Colors Color { get; set; }
        public Grades Grade { get; set; }
        //public ICollection<Purchases> StockItemsHistory { get; set; }
        public ICollection<SalesOrderDetails> SalesOrderDetails { get; set; }
        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

    }
}