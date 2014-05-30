using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain.Master;

namespace PreScripds.Domain
{
    public class UserLogin
    {
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string SaltKey { get; set; }
        [DataMember]
        public long SecurityQuestionId { get; set; }
        [DataMember]
        public string SecurityAnswer { get; set; }
        [DataMember]
        public long UserLoginId { get; set; }
        [DataMember]
        public string Captcha { get; set; }
        [DataMember]
        public string PasswordCap { get; set; }
        [DataMember]
        public virtual SecurityQuestion SecurtiyQuestion { get; set; }
        [DataMember]
        public virtual User User { get; set; }
    }
}
