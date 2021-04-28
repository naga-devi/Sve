
namespace Sve.Service.Domain.Accounts
{
	using System;
	using System.Collections.Generic;

    internal class LedgerGroup //: AuditEntityBase
    {
        public LedgerGroup()
        {
            Customers = new HashSet<Customer>();
        }

        public int GroupId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}