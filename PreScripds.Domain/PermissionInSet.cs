using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class PermissionInSet
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long? PermissionId { get; set; }
        [DataMember]
        public int? PermissionSetId { get; set; }
        [DataMember]
        public virtual Permission Permission { get; set; }
        [DataMember]
        public virtual PermissionSet PermissionSet { get; set; }
        [DataMember]
        public virtual ICollection<PermissionInModule> PermissionInModules { get; set; }
    }
}
