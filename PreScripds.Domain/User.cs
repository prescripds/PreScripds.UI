using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain.Interfaces;
using PreScripds.Domain.Master;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class User : IAuditable
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public bool Gender { get; set; }
        [DataMember]
        public System.DateTime Dob { get; set; }
        [DataMember]
        public long Mobile { get; set; }
        [DataMember]
        public long Phone { get; set; }
        [DataMember]
        public int? Age { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Alt_Email { get; set; }
        [DataMember]
        public long Alt_Mobile { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public long CityId { get; set; }
        [DataMember]
        public long StateId { get; set; }
        [DataMember]
        public long CountryId { get; set; }
        [DataMember]
        public int? Zipcode { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public bool? AdminApprove { get; set; }
        [DataMember]
        public bool IsHomeUrl { get; set; }
        [DataMember]
        public int UserType { get; set; }
        [DataMember]
        public bool TermsCondition { get; set; }
        [DataMember]
        public long OrganizationId { get; set; }
        [DataMember]
        public bool IsOrganization { get; set; }
        [DataMember]
        public System.DateTime CreatedDate { get; set; }
        [DataMember]
        public System.DateTime UpdatedDate { get; set; }
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public long UpdatedBy { get; set; }
        [DataMember]
        public virtual Country Country { get; set; }
        [DataMember]
        public virtual Organization Organization { get; set; }
        [DataMember]
        public virtual ICollection<UserInRole> UserInRoles { get; set; }
        [DataMember]
        public virtual ICollection<UserLogin> UserLogins { get; set; }

    }
}
