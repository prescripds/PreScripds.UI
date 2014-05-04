using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class UserHistory
    {
        public long UserHistoryId { get; set; }
        public string Captcha { get; set; }
        public string PasswordCap { get; set; }
        public string SaltKey { get; set; }
        public string IpAddress { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
