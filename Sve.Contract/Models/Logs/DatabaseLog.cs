
namespace Sve.Contract.Models.Logs
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class DatabaseLog
    {
        public int DatabaseLogId { get; set; }
		public DateTime PostTime { get; set; }
		[Required]
		[StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string DatabaseUser { get; set; }
		[Required]
		[StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string Event { get; set; }
		public string Schema { get; set; }
		public string Object { get; set; }
		[Required]
		[StringLength(2147483647, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string TSQL { get; set; }
		[Required]
		[StringLength(2147483647, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string XmlEvent { get; set; }
    }
}