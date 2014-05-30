using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain.Master;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class User
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public short Gender { get; set; }
        [DataMember]
        public DateTime? Dob { get; set; }
        [DataMember]
        public long Mobile { get; set; }
        [DataMember]
        public long? Phone { get; set; }
        [DataMember]
        public int? Age { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string AltEmail { get; set; }
        [DataMember]
        public long? AltMobile { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public long CityId { get; set; }
        [DataMember]
        public long StateId { get; set; }
        [DataMember]
        public long CountryId { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public bool AdminApprove { get; set; }
        [DataMember]
        public bool IsSuperAdmin { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }
        [DataMember]
        public bool IsHomeUrl { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public short? UserType { get; set; }
        [DataMember]
        public bool TermsCondition { get; set; }
        [DataMember]
        public long? ReferencedId { get; set; }
        [DataMember]
        public long? OrganizationId { get; set; }
        [DataMember]
        public string EmployeeId { get; set; }
        [DataMember]
        public string Designation { get; set; }
        [DataMember]
        public virtual ICollection<UserLogin> UserLogin { get; set; }
        [DataMember]
        public bool? IsOrganization { get; set; }
        [DataMember]
        public bool? IsEmailConfirmed { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public byte[] DisplayPic { get; set; }
        [DataMember]
        public virtual ICollection<UserHistory> UserHistory { get; set; }
    }
}
