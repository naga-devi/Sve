namespace Sve.Service.Domain.Product
{
    using System;
    using System.Collections.Generic;

    internal partial class ProductMaterialTypes
    {
        public ProductMaterialTypes()
        {
            StockGroups = new HashSet<StockGroups>();
        }

        public int? CategoryId { get; set; }
        public int MaterialTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public virtual ProductCategory Category { get; set; }
        public virtual ICollection<StockGroups> StockGroups { get; set; }
    }
}
