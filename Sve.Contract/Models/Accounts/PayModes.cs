
namespace Sve.Contract.Models.Accounts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class PayMode
    {
        public int PayModeId { get; set; }
		public short? Status { get; set; }
        public string Name { get; set; }
    }
}