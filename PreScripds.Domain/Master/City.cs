using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Master
{
    public class City
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public long? StateId { get; set; }
        [DataMember]
        public virtual State State { get; set; }
    }
}
