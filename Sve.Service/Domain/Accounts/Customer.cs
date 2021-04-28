
namespace Sve.Service.Domain.Accounts
{
	using System;
	using System.Collections.Generic;

    internal class Customer //: AuditEntityBase
    {
        public Customer()
        {
            AccountDetails = new HashSet<AccountDetail>();
            Transactions = new HashSet<Transaction>();
        }

        public int? GroupId { get; set; }
        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public bool? IsParentCompany { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int? StateId { get; set; }
        public string Pan { get; set; }
        public string TinNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int? Status { get; set; }

        public virtual LedgerGroup Group { get; set; }
        public virtual ICollection<AccountDetail> AccountDetails { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}