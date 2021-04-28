using System;
using System.Collections.Generic;

namespace Sve.Contract.Models.Product
{
    public partial class Brands
    {
        public Brands()
        {
            StockItems = new HashSet<StockGroups>();
        }

        public int? CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public ProductCategory Category { get; set; }
        public ICollection<StockGroups> StockItems { get; set; }
    }
}
