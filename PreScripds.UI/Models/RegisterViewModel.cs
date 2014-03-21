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
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public int Gender { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime? Dob { get; set; }
        public int? Age { get; set; }

        [Required(ErrorMessage = "Mobile is required.")]
        [Display(Name = "Mobile")]
        [DataType(DataType.PhoneNumber)]
        public long? Mobile { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public long? Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Aternate Email is required.")]
        [Display(Name = "Aternate Email")]
        [DataType(DataType.EmailAddress)]
        public string AltEmail { get; set; }

        [Required(ErrorMessage = "Aternate Mobile is required.")]
        [Display(Name = "Alternate Mobile")]
        [DataType(DataType.PhoneNumber)]
        public string AltMobile { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        //public List<City> City { get; set; }
        //public int? CityId { get; set; }

        public string CityName { get; set; }

        public List<State> State { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }
        public List<Country> Country { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }

        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public long? PinCode { get; set; }
        public short IsHomeUrl { get; set; }
        public int Active { get; set; }
        public List<UserLoginViewModel> userLoginViewModel { get; set; }
    }
}