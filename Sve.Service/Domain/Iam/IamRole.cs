using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Iam
{
    public partial class Role
    {
        public Role()
        {
            IamRolePermissions = new HashSet<RolePermissions>();
            IamUserRoles = new HashSet<UserRoles>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsCoreRole { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<RolePermissions> IamRolePermissions { get; set; }
        public virtual ICollection<UserRoles> IamUserRoles { get; set; }
    }
}
