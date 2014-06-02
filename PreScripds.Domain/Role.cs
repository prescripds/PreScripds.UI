using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class Role
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public System.DateTime CreatedDate { get; set; }
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public long UpdatedBy { get; set; }
        [DataMember]
        public System.DateTime UpdatedDate { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public long OrganizationId { get; set; }
        [DataMember]
        public virtual ICollection<RoleInPermission> RoleInPermissions { get; set; }
        [DataMember]
        public virtual Organization Organization { get; set; }
        [DataMember]
        public virtual ICollection<UserInRole> UserInRoles { get; set; }
    }
}
