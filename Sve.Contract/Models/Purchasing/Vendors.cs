
namespace Sve.Contract.Models.Purchasing
{
    using System;

    public class Vendors
    {
        public int VendorId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Pan { get; set; }
        public string TinNo { get; set; }
        public string Cstno { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int? Status { get; set; }
    }
}