using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class UserInRole
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long? UserId { get; set; }
        [DataMember]
        public long? RoleId { get; set; }
        [DataMember]
        public bool? Active { get; set; }
        [DataMember]
        public virtual Role Role { get; set; }
        [DataMember]
        public virtual User User { get; set; }
    }
}
