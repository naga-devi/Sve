namespace Sve.Service.Domain.Sales
{
    using JxNet.Extensions.EFCore.SqlServer;
    using System.Collections.Generic;

    internal class Customers : AuditEntityBase
    {
        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int? StateId { get; set; }
        public string Pan { get; set; }
        public string TinNo { get; set; }
        public string Cstno { get; set; }
        public int? Status { get; set; }

        public ICollection<CustomersInOrders> CustomersInOrders { get; set; }
    }
}
