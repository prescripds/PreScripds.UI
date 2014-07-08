using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PreScripds.UI.Models
{
    public class DepartmentViewModel
    {

        public long Id { get; set; }
        [Required(ErrorMessage = "Department Name is mandatory.")]
        public string DepartmentName { get; set; }

        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Department Description is mandatory.")]
        public string DepartmentDescription { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public List<DepartmentViewModel> DepartmentViewModels { get; set; }

        public bool CreationSuccessful { get; set; }
        public string Message { get; set; }
    }
}