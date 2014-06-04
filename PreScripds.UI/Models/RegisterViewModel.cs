using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PreScripds.Domain;
using PreScripds.Domain.Master;
using PreScripds.Infrastructure;

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

        [Required(ErrorMessage = "Date Of Birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        public DateTime? Dob { get; set; }
        public int? Age
        {
            get
            {
                var presentYear = DateTime.Now.Year;
                if (Dob.HasValue)
                {
                    var dob = Dob.Value.Year;
                    return (presentYear - dob);
                }
                return 0;
            }
        }

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

        public List<State> States { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }
        public List<Country> Countries { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }

        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public long? PinCode { get; set; }
        public bool IsHomeUrl { get; set; }
        public bool Active { get; set; }

        public string Message { get; set; }

        public string FullName
        {
            get
            {
                string secondPartName = string.Empty;
                if (MiddleName.IsEmpty())
                {
                    secondPartName = LastName;
                }
                else
                {
                    secondPartName = MiddleName + " " + LastName;
                }
                return "{0} {1}".ToFormat(FirstName, secondPartName);

            }
        }
        [Required(ErrorMessage = "User Type is required.")]
        public short? UserType { get; set; }

        [Required(ErrorMessage = "Terms and condition is required.")]
        public bool TermsCondition { get; set; }

        public bool CreationSuccessful { get; set; }
        public List<UserLoginViewModel> userLoginViewModel { get; set; }
        public List<UserHistoryViewModel> UserHistoryViewModel { get; set; }
        public long OrganizationId { get; set; }
        //public int OrganizationType { get; set; }
        public string CaptchaUserInput { get; set; }
        public string CaptchaValid { get; set; }
        public int? IsOrganization { get; set; }
        public string IpAddress { get; set; }

        public HttpPostedFileBase DisplayPicture { get; set; }
    }
}