
namespace Sve.Contract.Models.Accounts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class AccountDetail
    {
        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public int? BankId { get; set; }
        public string AccountNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int? Status { get; set; }

        public virtual BankDetail Bank { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<TransactionDetail> FromAccounts { get; set; }
        public virtual ICollection<TransactionDetail> ToAccounts { get; set; }
    }
}