using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class ModuleObjects
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string ModuleObjectName { get; set; }
        [DataMember]
        public long? ModuleId { get; set; }
        [DataMember]
        public virtual Module Module { get; set; }
    }
}
