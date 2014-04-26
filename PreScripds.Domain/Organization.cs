using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class Organization
    {
        [DataMember]
        public long OrganizationId { get; set; }
        [DataMember]
        public string OrganizationName { get; set; }
        [DataMember]
        public string OrganizationAddress { get; set; }
        [DataMember]
        public long? OrganizationPhone { get; set; }
        [DataMember]
        public long OrganizationMobile { get; set; }
        [DataMember]
        public string OrganizationEmail { get; set; }
        [DataMember]
        public string OrganizationContact { get; set; }
        [DataMember]
        public DateTime VerificationDate { get; set; }
        [DataMember]
        public string ReferencedEmail { get; set; }
        [DataMember]
        public string EmployeeIdOrg { get; set; }
        [DataMember]
        public DateTime? OrganiztionIncorporation { get; set; }
        [DataMember]
        public bool Isactive { get; set; }
        [DataMember]
        public bool? IsApproved { get; set; }
        [DataMember]
        public string ApproverId { get; set; }
        [DataMember]
        public DateTime? ApprovedDate { get; set; }
        [DataMember]
        public virtual ICollection<Department> Departments { get; set; }
    }
}
