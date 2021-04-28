namespace Sve.Contract.Models.Product
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductDetails
    {
        public int? TaxSlabId { get; set; }
        public int? CategoryId { get; set; }
        public int ProductId { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string Name { get; set; }
        public int? Hsn { get; set; }
		public short? MinimumStock { get; set; }
		public byte? RatingsCount { get; set; }
		public short? RatingsValue { get; set; }
		public string Description { get; set; }
		public int? Status { get; set; }

        public ProductCategory Category { get; set; }
        public List<ProductImages> ProductImages { get; set; }
        public List<StockGroups> ProductStockItems { get; set; }
        public TaxSlabs TaxSlab { get; set; }
    }
}