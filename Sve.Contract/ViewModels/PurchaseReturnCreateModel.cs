using System;
using System.Collections.Generic;

namespace Sve.Contract.ViewModels
{
    public class PurchaseReturnCreateModel
    {
        public PurchaseReturnHeaderModel Header { get; set; }
        public List<PurchaseReturnItemModel> Details { get; set; }

    }

    public class PurchaseReturnHeaderModel
    {
        public int PurchaseOrderId { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Remarks { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal GrandTotal { get; set; }
        public string CreatedBy { get; set; }
    }

    public class PurchaseReturnItemModel
    {
        /// <summary>
        /// Purchase details id
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// RejectedQty
        /// </summary>
        public short RejectedQty { get; set; } = 0;
    }
}
