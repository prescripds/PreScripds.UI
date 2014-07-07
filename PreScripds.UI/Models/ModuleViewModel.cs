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
        [Required(ErrorMessage = "Department is required.")]
        public long DepartmentInOrgId { get; set; }
        [Required(ErrorMessage = "Module Name is required.")]
        public string ModuleName { get; set; }
        [Required(ErrorMessage = "Module Description is required.")]
        public string ModuleDesc { get; set; }
        public List<DepartmentInOrganization> DepartmentInOrg { get; set; }
        public bool IsActive { get; set; }
    }
}