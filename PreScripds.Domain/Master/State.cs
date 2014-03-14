using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Master
{
    public class State
    {
        public long StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public long CountryId { get; set; }
        public virtual ICollection<City> City { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
