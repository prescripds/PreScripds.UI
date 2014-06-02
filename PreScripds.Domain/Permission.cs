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
        public long Id { get; set; }
        [DataMember]
        public string PermissionName { get; set; }
        [DataMember]
        public bool? Active { get; set; }
        [DataMember]
        public virtual ICollection<PermissionInModule> PermissionInModules { get; set; }
        [DataMember]
        public virtual ICollection<PermissionInSet> PermissionInSets { get; set; }
        [DataMember]
        public virtual ICollection<RoleInPermission> RoleInPermissions { get; set; }
    }
}
