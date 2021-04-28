using System;
using System.Collections.Generic;

namespace Sve.Contract.Models.Product
{
    public partial class Sizes
    {
        public Sizes()
        {
            StockItems = new HashSet<StockGroups>();
        }

        public int? CategoryId { get; set; }
        public int SizeId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public virtual ProductCategory Category { get; set; }
        public virtual ICollection<StockGroups> StockItems { get; set; }
    }
}
