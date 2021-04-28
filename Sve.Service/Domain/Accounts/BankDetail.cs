
namespace Sve.Service.Domain.Accounts
{
	using System;
	using System.Collections.Generic;

    internal class BankDetail //: AuditEntityBase
    {
        public BankDetail()
        {
            AccountDetails = new HashSet<AccountDetail>();
        }

        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string Ifsc { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string DdpayableAddress { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public short? Status { get; set; }

        public virtual ICollection<AccountDetail> AccountDetails { get; set; }
    }
}