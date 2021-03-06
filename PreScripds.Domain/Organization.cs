﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain.Interfaces;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class Organization : IAuditable
    {
        [DataMember]
        public long Id { get; set; }
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
        public string ContactPerson { get; set; }
        [DataMember]
        public System.DateTime? VerificationDate { get; set; }
        [DataMember]
        public string Org_EmployeeId { get; set; }
        [DataMember]
        public System.DateTime? OrganizationIncorporation { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public bool Approved { get; set; }
        [DataMember]
        public long? ApproverId { get; set; }
        [DataMember]
        public System.DateTime? ApprovedDate { get; set; }
        [DataMember]
        public bool IsQuickView { get; set; }
        [DataMember]
        public bool IsSelfie { get; set; }
        [DataMember]
        public virtual ICollection<DepartmentInOrganization> DepartmentInOrganizations { get; set; }
        [DataMember]
        public virtual ICollection<LibraryFolder> LibraryFolders { get; set; }
        [DataMember]
        public virtual ICollection<Role> Roles { get; set; }
        //[DataMember]
        //public virtual ICollection<User> Users { get; set; }
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public long UpdatedBy { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public bool? IsHomeOrg { get; set; }
    }
}
