using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Interfaces
{
    public interface IAuditable
    {
        [DataMember]
        long CreatedBy { get; set; }
        [DataMember]
        DateTime CreatedDate { get; set; }
        [DataMember]
        long UpdatedBy { get; set; }
        [DataMember]
        DateTime UpdatedDate { get; set; }
    }
}
