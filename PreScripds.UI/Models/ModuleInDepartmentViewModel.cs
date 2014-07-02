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
        public long ModuleId { get; set; }
        public List<Department> Department { get; set; }
        public List<Module> Module { get; set; }
        public bool IsActive { get; set; }
    }
}