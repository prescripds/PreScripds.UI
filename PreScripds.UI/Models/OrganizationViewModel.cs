using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PreScripds.UI.Models
{
    public class OrganizationViewModel
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public long? OrganizationPhone { get; set; }
        public long OrganizationMobile { get; set; }
        public string OrganizationEmail { get; set; }
        public string OrganizationContact { get; set; }
        public DateTime VerificationDate { get; set; }
        public string ReferencedEmail { get; set; }
        public string EmployeeIdOrg { get; set; }
        public DateTime? OrganiztionIncorporation { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public string ApproverId { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}