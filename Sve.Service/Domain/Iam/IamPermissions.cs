using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Iam
{
    public partial class Permissions
    {
        public Permissions()
        {
            IamRolePermissions = new HashSet<RolePermissions>();
        }

        public int PermissionId { get; set; }
        public string Name { get; set; }
        public int? PermissionLevel { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<RolePermissions> IamRolePermissions { get; set; }
    }
}
