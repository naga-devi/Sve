
namespace Sve.Contract.Models.Logs
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class ErrorLog
    {
        public int ErrorLogId { get; set; }
		public DateTime ErrorTime { get; set; }
		[Required]
		[StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string UserName { get; set; }
		public int ErrorNumber { get; set; }
		public int? ErrorSeverity { get; set; }
		public int? ErrorState { get; set; }
		public string ErrorProcedure { get; set; }
		public int? ErrorLine { get; set; }
		[Required]
		[StringLength(4000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string ErrorMessage { get; set; }
    }
}