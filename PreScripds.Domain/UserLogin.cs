using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain.Master;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class UserLogin
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string saltkey { get; set; }
        [DataMember]
        public long? SecurityQuestionId { get; set; }
        [DataMember]
        public string SecurityAnswer { get; set; }
        [DataMember]
        public long? UserId { get; set; }
        [DataMember]
        public string Captcha { get; set; }
        [DataMember]
        public string PasswordCap { get; set; }
        [DataMember]
        public virtual SecurityQuestion SecurityQuestion { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [DataMember]
        public virtual ICollection<UserHistory> UserHistories { get; set; }
    }
}
