namespace Sve.Service.Domain.Purchasing
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Sve.Service.Domain.Accounts;
    using System;
    using System.Collections.Generic;

    internal class CreditNotes : AuditEntityBase
    {
        public CreditNotes()
        {
            Orders = new HashSet<CreditNotesInOrders>();
        }

        public int VendorId { get; set; }
        public int CreditNoteId { get; set; }
        public decimal Discount { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }
        public DateTime? IssueDate { get; set; }

        public virtual Vendors Vendor { get; set; }
        public virtual ICollection<CreditNotesInOrders> Orders { get; set; }
    }
}