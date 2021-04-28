
namespace Sve.Contract.Models.Accounts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class BankDetail
    {
        public int BankId { get; set; }
		public string BankName { get; set; }
		public string BranchName { get; set; }
		public string IFSC { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public string DDPayableAddress { get; set; }
		public short? Status { get; set; }
    }
}