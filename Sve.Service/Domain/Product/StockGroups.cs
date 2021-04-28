namespace Sve.Service.Domain.Product
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Sve.Service.Domain.Purchasing;
    using Sve.Service.Domain.Sales;
    using System.Collections.Generic;

    internal partial class StockGroups : AuditEntityBase
    {
        public int? ProductId { get; set; }
        public int StockGroupId { get; set; }
        public int MaterialTypeId { get; set; }
        public int SizeId { get; set; }
        public int BrandId { get; set; }
        public int GradeId { get; set; }
        public int ColorId { get; set; }
        public decimal BasicMrp { get; set; }
        public decimal NetPrice { get; set; }
        public decimal Cgst { get; set; }
        public decimal Sgst { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? Discount { get; set; }
        public decimal? SellPrice { get; set; }
        public bool? IsPrime { get; set; }
        public short? MinimumStock { get; set; }
        public byte? RatingsCount { get; set; }
        public short? RatingsValue { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public Colors Color { get; set; }
        public Grades Grade { get; set; }
        public ProductBrands Brand { get; set; }
        public ProductMaterialTypes MaterialType { get; set; }
        public ProductDetails Product { get; set; }
        public ProductSizes Size { get; set; }
        public ICollection<SalesOrderDetails> SalesOrderDetails { get; set; }
        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}
