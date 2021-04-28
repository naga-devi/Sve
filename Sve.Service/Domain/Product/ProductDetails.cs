namespace Sve.Service.Domain.Product
{
    using JxNet.Extensions.EFCore.SqlServer;
    using System.Collections.Generic;

    internal partial class ProductDetails : AuditEntityBase
    {
        public ProductDetails()
        {
            ProductImages = new HashSet<ProductImages>();
            StockGroups = new HashSet<StockGroups>();
        }

        public int? TaxSlabId { get; set; }
        public int? CategoryId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int? Hsn { get; set; }
        public short? MinimumStock { get; set; }
        public byte? RatingsCount { get; set; }
        public short? RatingsValue { get; set; }
        public string Description { get; set; }
        public byte? Status { get; set; }

        public ProductCategory Category { get; set; }
        public ProductTaxSlabs TaxSlab { get; set; }
        public ICollection<ProductImages> ProductImages { get; set; }
        public ICollection<StockGroups> StockGroups { get; set; }
    }
}
