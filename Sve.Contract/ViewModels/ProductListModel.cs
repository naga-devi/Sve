using Sve.Contract.Models.Product;
using System.Collections.Generic;

namespace Sve.Contract.ViewModels
{
    public class ProductListModel
    {
        public int? TaxSlabId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int? RatingsCount { get; set; }
        public int? RatingsValue { get; set; }
        public string ImagePath { get; set; }

        //public decimal? OldPrice { get; set; }
        //public decimal? NewPrice { get; set; }
        //public decimal? Discount { get; set; }

        public int? StockedQuantity { get; set; }
        public int? SoldQuantity { get; set; }
        public int? AvailibilityCount => (StockedQuantity ?? 0) - (SoldQuantity ?? 0);

        public StockGroups StockItems { get; set; }
        public TaxSlabs TaxSlab { get; set; }
        public string Description { get; set; }
        public ICollection<ProductImages> Images { get; set; }
    }
}
