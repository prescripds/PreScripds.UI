using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class UserDepartmentMapping
    {
        public long? UserId { get; set; }
        public long? DepartmentId { get; set; }
        public long UserDepartmentId { get; set; }
        // public bool? IsActive { get; set; }
        //  public DateTime CreatedDate { get; set; }
        public long? RoleId { get; set; }
        // public virtual Department Department { get; set; }
        //public virtual Role Role { get; set; }
        //public virtual User User { get; set; }
    }
}
