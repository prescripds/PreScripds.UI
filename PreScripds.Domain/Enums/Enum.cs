using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Enums
{
    public enum Gender
    {
        [DataMember]
        Male = 1,
        [DataMember]
        Female = 2,
        [DataMember]
        Other = 3
    }
}
