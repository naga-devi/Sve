
namespace Sve.Service.Domain.Purchasing
{
	using System;

    internal class PurchaseReturns //: AuditEntityBase
    {
        public int PurchaseOrderId { get; set; }
        public int Id { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Remarks { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public virtual PurchaseOrderHeader PurchaseOrder { get; set; }
    }
}