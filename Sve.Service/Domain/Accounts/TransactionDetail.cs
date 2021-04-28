
namespace Sve.Service.Domain.Accounts
{
	using System;
	using System.Collections.Generic;

    internal class TransactionDetail //: AuditEntityBase
    {
        public int Id { get; set; }
        public long? TransactionId { get; set; }
        public int? FromAccountId { get; set; }
        public int? ToAccountId { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string ChequeNo { get; set; }
        public string Utrno { get; set; }
        public DateTime? TransactionDate { get; set; }

        public virtual AccountDetail FromAccount { get; set; }
        public virtual AccountDetail ToAccount { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}