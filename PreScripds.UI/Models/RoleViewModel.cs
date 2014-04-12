using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class RoleViewModel
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public List<Permission> Permission { get; set; }
        public long PermissionId { get; set; }
    }
}