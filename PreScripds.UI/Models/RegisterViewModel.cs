using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PreScripds.Domain;
using PreScripds.Domain.Master;

namespace PreScripds.UI.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int Gender { get; set; }


        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }
        [Display(Name = "Date Of Birth")]
        public int Age { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        [DataType(DataType.PhoneNumber)]
        public long? Mobile { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public long? Phone { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Aternate Email")]
        [DataType(DataType.EmailAddress)]
        public string AltEmail { get; set; }

        [Required]
        [Display(Name = "Alternate Mobile")]
        [DataType(DataType.PhoneNumber)]
        public string AltMobile { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        public List<City> City { get; set; }
        public int CityId { get; set; }

        public List<State> State { get; set; }
        public int StateId { get; set; }
        public List<Country> Country { get; set; }

        public int CountryId { get; set; }

        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public long PinCode { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<SecurityQuestion> SecurityQuestion { get; set; }
        public int SecurityQuestionId { get; set; }

        [Required]
        [Display(Name = "Security Answer")]
        public string SecurityAnswer { get; set; }

        public bool IsHomeUrl { get; set; }

        public int Active { get; set; }
    }
}