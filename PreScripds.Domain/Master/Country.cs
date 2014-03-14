using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Master
{
    public class Country
    {
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public virtual ICollection<State> States { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
