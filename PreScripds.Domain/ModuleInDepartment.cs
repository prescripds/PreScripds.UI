using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class ModuleInDepartment
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long? ModuleId { get; set; }
        [DataMember]
        public long? DepartmentId { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public virtual Department Department { get; set; }
        [DataMember]
        public virtual Module Module { get; set; }
    }
}
