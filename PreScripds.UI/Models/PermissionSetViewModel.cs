using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class PermissionSetViewModel
    {
        public long PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDesc { get; set; }
        public List<Permission> Permission { get; set; }
        public IEnumerable<string> PermissionSelected { get; set; }
        public List<Department> Department { get; set; }
        public List<Module> Module { get; set; }
        public long DepartmentId { get; set; }
        public long ModuleId { get; set; }
    }
}