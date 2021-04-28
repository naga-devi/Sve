using JxNet.Extensions.EFCore.SqlServer;
using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Product
{
    internal partial class Colors: CreateAuditEntityBase
    {
        public Colors()
        {
            StockGroups = new HashSet<StockGroups>();
        }

        public int? CategoryId { get; set; }
        public int ColorId { get; set; }
        public string Name { get; set; }

        public virtual ProductCategory Category { get; set; }
        public virtual ICollection<StockGroups> StockGroups { get; set; }
    }
}
