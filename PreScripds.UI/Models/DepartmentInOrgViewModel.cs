using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class DepartmentInOrgViewModel
    {
        public List<Department> Department { get; set; }

        [Required(ErrorMessage = "Please select the Departments you would like to include for your Organization.")]
        public IEnumerable<string> Departments { get; set; }

        public List<DepartmentInOrganizationViewModel> DepartmentInOrganizationViewModel { get; set; }

        public bool CreationSuccessful { get; set; }
        public string Message { get; set; }
    }

    public class DepartmentInOrganizationViewModel
    {
        public long DepartmentId { get; set; }
        public long OrganizationId { get; set; }
        public bool Active { get; set; }
    }
}