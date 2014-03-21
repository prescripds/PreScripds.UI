using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PreScripds.Domain.Master;

namespace PreScripds.UI.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "User name is required.")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<SecurityQuestion> SecurityQuestion { get; set; }
        [Required(ErrorMessage = "Security Question is required.")]
        [Display(Name = "Security Question")]
        public int SecurityQuestionId { get; set; }

        [Required(ErrorMessage = "Security Answer is required.")]
        [Display(Name = "Security Answer")]
        public string SecurityAnswer { get; set; }

    }
}