
namespace Sve.Contract.Models.Purchasing
{
    using System;
    using System.Collections.Generic;

    public class PurchaseOrderHeader
    {
        public PurchaseOrderHeader()
        {
            PurchaseOrderDetail = new HashSet<PurchaseOrderDetail>();
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
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public Vendors Vendor { get; set; }
        public ICollection<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
    }
}