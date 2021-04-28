
namespace Sve.Contract.Models.Accounts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class TransactionDetail
    {
        public int Id { get; set; }
		public long? TransactionId { get; set; }
		public int? FromAccountId { get; set; }
		public int? ToAccountId { get; set; }
		public DateTime? ChequeDate { get; set; }
		public string ChequeNo { get; set; }
		public string Utrno { get; set; }
		public DateTime? TransactionDate { get; set; }
    }
}