
namespace Sve.Contract.Models.Accounts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class Transactions
    {
		public int? VoucherTypeId { get; set; }
		public int? AccountTypeId { get; set; }
		public int? CustomerId { get; set; }
		public int? PayModeId { get; set; }
        public long TransactionId { get; set; }
		public decimal? PaidAmount { get; set; }
		public DateTime? PaidDate { get; set; }
		public string Remarks { get; set; }
		public short? Status { get; set; }
    }
}