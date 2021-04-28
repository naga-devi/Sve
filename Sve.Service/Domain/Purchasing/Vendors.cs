namespace Sve.Service.Domain.Purchasing
{
    using JxNet.Extensions.EFCore.SqlServer;
    using System;
    using System.Collections.Generic;

    internal partial class Vendors: AuditEntityBase
    {
        public Vendors()
        {
            OrderHeader = new HashSet<PurchaseOrderHeader>();
        }

        public int VendorId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Pan { get; set; }
        public string TinNo { get; set; }
        public string Cstno { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<PurchaseOrderHeader> OrderHeader { get; set; }
    }
}
