using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PreScripds.UI.Models
{
    public class UserProfileViewModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public long Mobile { get; set; }
        public string AltEmail { get; set; }
        public long AltMobile { get; set; }
        public long SecurityQuestionId { get; set; }
        public string SecurityAnswer { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; }

    }
}