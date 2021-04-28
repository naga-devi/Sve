
namespace Sve.Service.Domain.Accounts
{
    using JxNet.Extensions.EFCore.SqlServer;
    using System;
	using System.Collections.Generic;

    internal class Transaction : AuditEntityBase
    {
        public Transaction()
        {
            TransactionDetails = new HashSet<TransactionDetail>();
        }

        public int? VoucherTypeId { get; set; }
        public int? AccountTypeId { get; set; }
        public int? CustomerId { get; set; }
        public int? PayModeId { get; set; }
        public long TransactionId { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Remarks { get; set; }
        public short? Status { get; set; }

        public virtual AccountType AccountType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual PayMode PayMode { get; set; }
        public virtual VoucherType VoucherType { get; set; }
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}