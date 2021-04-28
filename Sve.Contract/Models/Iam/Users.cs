
namespace Sve.Contract.Models.Iam
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class Users
    {
		public int? UserTypeId { get; set; }
        public int UserId { get; set; }
		[Required]
		[StringLength(56, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string UserName { get; set; }
		public string FullName { get; set; }
		public string EmailId { get; set; }
		public string Contactno { get; set; }
		public string Password { get; set; }
		public int? Createdby { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? Status { get; set; }
    }
}