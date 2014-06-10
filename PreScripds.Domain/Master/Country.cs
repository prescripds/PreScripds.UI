using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace PreScripds.Domain.Master
{
    [DataContract(IsReference = true)]
    public class Country
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public virtual ICollection<State> States { get; set; }
        [DataMember]
        public virtual ICollection<User> Users { get; set; }
    }
}
