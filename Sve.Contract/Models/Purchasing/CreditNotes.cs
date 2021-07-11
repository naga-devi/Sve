using System;
using System.Collections.Generic;

namespace Sve.Contract.Models.Purchasing
{
    public class CreditNotes
    {
		public int VendorId { get; set; }
		public int CreditNoteId { get; set; }
		public decimal Discount { get; set; }
		public DateTime? IssueDate { get; set; }
		public string Remarks { get; set; }
		public byte Status { get; set; }
		public Vendors Vendor { get; set; }
		public virtual ICollection<CreditNotesInOrders> CreditOrders { get; set; }
	}
}