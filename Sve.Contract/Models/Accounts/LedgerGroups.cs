
namespace Sve.Contract.Models.Accounts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class LedgerGroup
    {
        public int GroupId { get; set; }
		public string Name { get; set; }
    }
}