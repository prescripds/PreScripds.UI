using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class DepartmentInOrganization
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long? DepartmentId { get; set; }
        [DataMember]
        public long? OrganizationId { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public virtual Department Department { get; set; }
        [DataMember]
        public virtual Organization Organization { get; set; }
    }
}
