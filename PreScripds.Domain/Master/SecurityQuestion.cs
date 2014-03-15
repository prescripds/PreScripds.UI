using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Master
{
    public class SecurityQuestion
    {
        [DataMember]
        public long SecurityQuestionId { get; set; }
        [DataMember]
        public string SecurityQuestionName { get; set; }
        [DataMember]
        public virtual ICollection<UserLogin> UserLogin { get; set; }
    }
}
