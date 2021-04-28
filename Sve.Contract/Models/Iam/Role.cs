
namespace Sve.Contract.Models.Iam
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class Role
    {
        public int RoleId { get; set; }
		[Required]
		[StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string RoleName { get; set; }
		public bool? IsCoreRole { get; set; }
		public int? Status { get; set; }
    }
}