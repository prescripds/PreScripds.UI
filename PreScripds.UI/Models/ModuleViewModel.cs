using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class ModuleViewModel
    {
        public long ModuleId { get; set; }
        public long DepartmentInOrgId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDesc { get; set; }
        public List<ModuleInDepartment> ModuleInDepartments { get; set; }
        public bool IsActive { get; set; }
        public List<Department> Departments { get; set; }
        public List<DepartmentInOrganization> DepartmentInOrg { get; set; }
        public bool CreationSuccessful { get; set; }
        public string Message { get; set; }
        public List<ModuleViewModel> ModuleViewModels { get; set; }

    }
}