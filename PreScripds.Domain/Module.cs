using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class Module
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string ModuleName { get; set; }
        [DataMember]
        public string ModuleDescription { get; set; }
        [DataMember]
        public virtual ICollection<ModuleInDepartment> ModuleInDepartments { get; set; }
        [DataMember]
        public virtual ICollection<ModuleObjects> ModuleObjects { get; set; }
        [DataMember]
        public virtual ICollection<PermissionInModule> PermissionInModules { get; set; }
        [DataMember]
        public long? DepartmentId { get; set; }
        [DataMember]
        public virtual Department Department { get; set; }
    }
}
