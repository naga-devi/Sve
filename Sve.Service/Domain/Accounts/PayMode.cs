
namespace Sve.Service.Domain.Accounts
{
	using System;
	using System.Collections.Generic;

    internal class PayMode //: AuditEntityBase
    {
        public PayMode()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int PayModeId { get; set; }
        public string Name { get; set; }
        public short? Status { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}