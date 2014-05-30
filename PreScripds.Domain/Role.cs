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
        public long RoleId { get; set; }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public string RoleDesc { get; set; }
        [DataMember]
        public long PermissionId { get; set; }
        [DataMember]
        public long OrganizationId { get; set; }
        [DataMember]
        public long? DepartmentId { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public virtual Permission Permission { get; set; }
        //[DataMember]
        //public virtual ICollection<UserDepartmentMapping> UserDepartmentMappings { get; set; }
    }
}
