using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class ApprovalViewModel
    {
        public List<UserApprovalViewModel> UserApprovalViewModel { get; set; }
    }

    public class UserApprovalViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long RoleId { get; set; }
        public List<Role> Role { get; set; }
        public bool IsApproved { get; set; }

    }

    public class UserDepartmentRoleViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long DepartmentId { get; set; }
        public List<Department> Department { get; set; }
        public long RoleId { get; set; }
        public List<Role> Role { get; set; }

    }

    public class OrganizationApprovalViewModel
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public bool IsApproved { get; set; }

    }

    public class OrganizationUserViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        
    }
}