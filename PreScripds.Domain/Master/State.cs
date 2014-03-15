using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Master
{
    public class State
    {
        [DataMember]
        public long StateId { get; set; }
        [DataMember]
        public string StateName { get; set; }
        [DataMember]
        public string StateCode { get; set; }
        [DataMember]
        public long CountryId { get; set; }
        [DataMember]
        public virtual ICollection<City> City { get; set; }
        [DataMember]
        public virtual Country Country { get; set; }
        [DataMember]
        public virtual ICollection<User> Users { get; set; }
    }
}
