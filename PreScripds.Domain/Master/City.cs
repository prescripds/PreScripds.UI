using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Master
{
    public class City
    {
        public long CityId { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public long StateId { get; set; }
        public virtual State State { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
