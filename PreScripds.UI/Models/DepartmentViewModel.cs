using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PreScripds.UI.Models
{
    public class DepartmentViewModel
    {

        public long Id { get; set; }

        public string DepartmentName { get; set; }

        public bool IsActive { get; set; }

        public string DepartmentDescription { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}