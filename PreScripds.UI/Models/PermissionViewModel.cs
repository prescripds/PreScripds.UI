using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class PermissionViewModel
    {
        public long PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDesc { get; set; }
        public long OrganizationId { get; set; }
        public List<Department> Department { get; set; }
        public long DepartmentId { get; set; }
        public string Permission { get; set; }
    }
}