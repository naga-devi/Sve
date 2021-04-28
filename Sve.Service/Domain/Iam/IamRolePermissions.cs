using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Iam
{
    public partial class RolePermissions
    {
        public int RolePermissionId { get; set; }
        public int? RoleId { get; set; }
        public int? PermissionId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? Status { get; set; }

        public virtual Permissions Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
