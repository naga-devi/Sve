using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Iam
{
    public partial class Users
    {        
        public int? UserTypeId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string Contactno { get; set; }
        public string Password { get; set; }
        public int? Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<UserRoles> IamUserRoles { get; set; }
    }
}
