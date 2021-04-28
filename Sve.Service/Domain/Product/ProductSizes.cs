using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Product
{
    internal partial class ProductSizes
    {
        public ProductSizes()
        {
            StockGroups = new HashSet<StockGroups>();
        }

        public int? CategoryId { get; set; }
        public int SizeId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public ProductCategory Category { get; set; }
        public ICollection<StockGroups> StockGroups { get; set; }
    }
}
