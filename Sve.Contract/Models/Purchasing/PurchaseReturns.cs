using System;

namespace Sve.Contract.Models.Purchasing
{
    public class PurchaseReturns
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
        public PurchaseOrderHeader PurchaseOrder { get; set; }
    }
}