
namespace Sve.Contract.Models.Iam
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class Permissions
    {
        public int PermissionId { get; set; }
		public string Name { get; set; }
		public int? PermissionLevel { get; set; }
		public string Title { get; set; }
		public int? ParentId { get; set; }
		public int? Status { get; set; }
    }
}