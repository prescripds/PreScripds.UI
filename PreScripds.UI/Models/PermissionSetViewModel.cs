using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class PermissionSetViewModel
    {
        public long PermissionSetId { get; set; }
        public string PermissionSetName { get; set; }
        public string PermissionSetDescription { get; set; }
        public List<Permission> Permission { get; set; }
        public IEnumerable<string> PermissionSelected { get; set; }
        public List<Department> Department { get; set; }
        public List<Module> Module { get; set; }
        public long? DepartmentId { get; set; }
        public long? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string DepartmentName { get; set; }
        public string PermissionName { get; set; }
        public bool IsActive { get; set; }
        public List<PermissionSetViewModel> PermissionSetViewModels { get; set; }
        public bool CreationSuccessful { get; set; }
        public string Message { get; set; }

    }
}