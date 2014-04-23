using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class Organization
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public long? OrganizationPhone { get; set; }
        public long OrganizationMobile { get; set; }
        public string OrganizationEmail { get; set; }
        public string OrganizationContact { get; set; }
        public System.DateTime VerificationDate { get; set; }
        public string ReferencedEmail { get; set; }
        public string EmployeeIdOrg { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}
