using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PreScripds.UI.Models
{
    public class UserHistoryViewModel
    {
        public long UserHistoryId { get; set; }
        public string Captcha { get; set; }
        public string PasswordCap { get; set; }
        public string SaltKey { get; set; }
        public string IpAddress { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long UserId { get; set; }
    }
}