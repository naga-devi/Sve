namespace Sve.Contract.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class EFMigrationsHistory
    {
		[Required]
		[StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string MigrationId { get; set; }
		[Required]
		[StringLength(32, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string ProductVersion { get; set; }
    }
}