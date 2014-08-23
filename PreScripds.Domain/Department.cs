using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using PreScripds.Domain.Interfaces;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class Department : IAuditable
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
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public long UpdatedBy { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public virtual ICollection<Module> Modules { get; set; }
        [DataMember]
        public virtual ICollection<PermissionSet> PermissionSets { get; set; }
        [DataMember]
        public virtual ICollection<UserRoleDepartment> UserRoleDepartments { get; set; }
    }
}
