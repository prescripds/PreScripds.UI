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
        public long Id { get; set; }
        [DataMember]
        public string DepartmentName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string DepartmentDescription { get; set; }
        [DataMember]
        public virtual ICollection<DepartmentInOrganization> DepartmentInOrganizations { get; set; }
        [DataMember]
        public virtual ICollection<ModuleInDepartment> ModuleInDepartments { get; set; }
    }
}
