using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class PermissionSet
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string PermissionSetName { get; set; }
        [DataMember]
        public virtual ICollection<PermissionInSet> PermissionInSets { get; set; }
    }
}
