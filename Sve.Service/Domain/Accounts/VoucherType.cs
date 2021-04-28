
namespace Sve.Service.Domain.Accounts
{
	using System;
	using System.Collections.Generic;

    internal class VoucherType //: AuditEntityBase
    {
        public VoucherType()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int VoucherTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}