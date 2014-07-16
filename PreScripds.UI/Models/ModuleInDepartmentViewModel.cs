using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class ModuleInDepartmentViewModel
    {
        public long DepartmentId { get; set; }
        public string ModuleName { get; set; }
        public List<Department> Department { get; set; }
        public List<ModuleInDeptVM> ModuleVm { get; set; }
        public IEnumerable<string> Modules { get; set; }
        public bool IsActive { get; set; }
    }

    public class ModuleInDeptVM
    {
        public long Id { get; set; }
        public string ModuleName { get; set; }
    }
}