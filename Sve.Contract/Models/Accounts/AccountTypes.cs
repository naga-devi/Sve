namespace Sve.Contract.Models.Accounts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class AccountType
    {
        public int AccountTypeId { get; set; }
		public string Name { get; set; }
    }
}