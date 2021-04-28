namespace Sve.Service.Domain.Product
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Sve.Service.Domain.Purchasing;
    using Sve.Service.Domain.Sales;
    using System;
    using System.Collections.Generic;

    internal partial class UnitMeasure : AuditEntityBase
    {
        public UnitMeasure()
        {
            PurchaseOrderDetail = new HashSet<PurchaseOrderDetail>();
            SalesOrderDetails = new HashSet<SalesOrderDetails>();
        }

        public int UnitMeasureId { get; set; }
        public string Name { get; set; }

        public ICollection<SalesOrderDetails> SalesOrderDetails { get; set; }
        public ICollection<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
    }
}
