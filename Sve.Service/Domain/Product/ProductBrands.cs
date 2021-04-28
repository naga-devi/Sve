using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Product
{
    internal partial class ProductBrands
    {
        public ProductBrands()
        {
            StockGroups = new HashSet<StockGroups>();
        }

        public int? CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public ProductCategory Category { get; set; }
        public ICollection<StockGroups> StockGroups { get; set; }
    }
}
