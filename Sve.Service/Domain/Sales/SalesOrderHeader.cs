namespace Sve.Service.Domain.Sales
{
    using JxNet.Extensions.EFCore.SqlServer;
    using System;
    using System.Collections.Generic;

    internal partial class SalesOrderHeader : AuditEntityBase
    {
        public int SalesOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal NetAmount { get; set; }
        public decimal? Freight { get; set; }
        public decimal? RoundOffAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public byte Paymode { get; set; }
        public string TransactionNo { get; set; }
        public string Comment { get; set; }
        public byte Status { get; set; }

        public ICollection<CustomersInOrders> CustomersInOrders { get; set; }
        public ICollection<SalesOrderDetails> OrderDetails { get; set; }
    }
}
