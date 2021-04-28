
namespace Sve.Contract.Models.Accounts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class VoucherType
    {
        public int VoucherTypeId { get; set; }
		public string Name { get; set; }
    }
}