using JxNet.Extensions.EFCore.SqlServer;
using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Purchasing
{
    internal partial class PurchaseOrderHeader : AuditEntityBase
    {
        public PurchaseOrderHeader()
        {
            OrderDetails = new HashSet<PurchaseOrderDetail>();
        }

        public int VendorId { get; set; }
        public int PurchaseOrderId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string InvoiceNo { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal CgstAmount { get; set; }
        public decimal SgstAmount { get; set; }
        public decimal IgstAmount { get; set; }
        public decimal Freight { get; set; }
        public decimal SubTotal { get; set; }
        public decimal RoundOffAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }

        public virtual Vendors Vendor { get; set; }
        public virtual ICollection<PurchaseOrderDetail> OrderDetails { get; set; }
    }
}
