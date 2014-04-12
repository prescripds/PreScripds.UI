using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class Permission
    {
        [DataMember]
        public long PermissionId { get; set; }
        [DataMember]
        public string PermissionName { get; set; }
        [DataMember]
        public string PermissionDesc { get; set; }
        [DataMember]
        public virtual ICollection<Role> Roles { get; set; }
    }
}
