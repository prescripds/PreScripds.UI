using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class RoleViewModel
    {
        public long RoleId { get; set; }
        //[Required(ErrorMessage = "Role Name is mandatory.")]
        public string RoleName { get; set; }
        //[Required(ErrorMessage = "Role Description is mandatory.")]
        public string RoleDesc { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationId { get; set; }
        public bool CreationSuccessful { get; set; }
        public string Message { get; set; }
        public List<RoleViewModel> RoleViewModels { get; set; }
    }
}