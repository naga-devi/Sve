using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Iam
{
    public partial class UserRoles
    {
        public int UserRolesId { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? Status { get; set; }

        public virtual Role Role { get; set; }
        public virtual Users User { get; set; }
    }
}
