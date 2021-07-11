
namespace Sve.Contract.Models.Purchasing
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class CreditNotesInOrders
    {
		public int Id { get; set; }
		public int CreditNoteId { get; set; }
		public int PurchaseOrderId { get; set; }
    }
}