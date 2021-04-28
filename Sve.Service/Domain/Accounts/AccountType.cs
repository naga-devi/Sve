
namespace Sve.Service.Domain.Accounts
{
	using System;
	using System.Collections.Generic;

    internal class AccountType //: AuditEntityBase
    {
        public AccountType()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int AccountTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}