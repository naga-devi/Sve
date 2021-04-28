
namespace Sve.Contract.Models.Iam
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class UserRoles
    {
        public int UserRolesId { get; set; }
		public int? UserId { get; set; }
		public int? RoleId { get; set; }
		public int? Status { get; set; }
    }
}