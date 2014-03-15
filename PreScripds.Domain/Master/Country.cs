using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Master
{
    public class Country
    {
        [DataMember]
        public long CountryId { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public virtual ICollection<State> States { get; set; }
        [DataMember]
        public virtual ICollection<User> Users { get; set; }
    }
}
