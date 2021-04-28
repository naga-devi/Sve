using JxNet.Extensions.EFCore.SqlServer;
using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Product
{
    internal partial class Grades : CreateAuditEntityBase
    {
        public Grades()
        {
            StockGroups = new HashSet<StockGroups>();
        }

        public int? CategoryId { get; set; }
        public int GradeId { get; set; }
        public string Name { get; set; }

        public ProductCategory Category { get; set; }
        public ICollection<StockGroups> StockGroups { get; set; }
    }
}
