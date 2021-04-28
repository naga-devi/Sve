using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sve.WebHost.Models.Accounts
{
    public class TransactionCreateModel
    {
		public int? VoucherTypeId { get; set; }
		public int? AccountTypeId { get; set; }
		public int? CustomerId { get; set; }
		public int? PayModeId { get; set; }
		public long? TransactionId { get; set; }
		public decimal? PaidAmount { get; set; }
		public DateTime? PaidDate { get; set; }
		public string Remarks { get; set; }
		public int Id { get; set; }
		public int? FromAccountId { get; set; }
		public int? ToAccountId { get; set; }
		public DateTime? ChequeDate { get; set; }
		public string ChequeNo { get; set; }
		public string Utrno { get; set; }
	}
}
