using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class RoleInPermissionViewModel
    {
        public long RoleId { get; set; }
        public List<Role> Roles { get; set; }
        public long PermissionSetId { get; set; }
        public List<PermissionSet> PermissionSets { get; set; }
        public string PermissionSetName { get; set; }
        public string RoleName { get; set; }
        public List<RoleInPermissionViewModel> RoleInPermissionViewModels { get; set; }
        public bool IsActive { get; set; }
        public bool CreationSuccessful { get; set; }
        public string Message { get; set; }
    }
}