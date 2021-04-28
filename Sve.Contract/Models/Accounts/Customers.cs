
namespace Sve.Contract.Models.Accounts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class Customer
    {
		public int? GroupId { get; set; }
        public int CustomerId { get; set; }
		public string CompanyName { get; set; }
		public bool? IsParentCompany { get; set; }
		public string Email { get; set; }
		public string PhoneNo { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string ZipCode { get; set; }
		public int? StateId { get; set; }
		public string Pan { get; set; }
		public string TinNo { get; set; }
		public int? Status { get; set; }
    }
}