
namespace Sve.Contract.Models.Iam
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class RolePermissions
    {
        public int RolePermissionId { get; set; }
		public int? RoleId { get; set; }
		public int? PermissionId { get; set; }
		public int? Status { get; set; }
    }
}