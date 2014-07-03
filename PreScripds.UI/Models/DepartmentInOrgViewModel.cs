using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class DepartmentInOrgViewModel
    {
        public List<Department> Department { get; set; }
        public IEnumerable<string> Departments { get; set; }
    }
}