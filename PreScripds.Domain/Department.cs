using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class Department
    {
        [DataMember]
        public int DepartmentId { get; set; }
        [DataMember]
        public string DepartmentName { get; set; }
        [DataMember]
        public long OrganizationId { get; set; }
        [DataMember]
        public string DepartmentDesc { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        //[DataMember]
        //public virtual ICollection<UserDepartmentMapping> UserDepartmentMappings { get; set; }
        [DataMember]
        public virtual Organization Organization { get; set; }
    }
}
