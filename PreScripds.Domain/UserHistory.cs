using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class UserHistory
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Captcha { get; set; }
        [DataMember]
        public string PasswordCap { get; set; }
        [DataMember]
        public string saltkey { get; set; }
        [DataMember]
        public string IpAddress { get; set; }
        [DataMember]
        public System.DateTime CreatedDate { get; set; }
        [DataMember]
        public long UserloginId { get; set; }
        [DataMember]
        public virtual UserLogin UserLogin { get; set; }
    }
}
