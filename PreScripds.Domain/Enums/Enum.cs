using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Enums
{
    public enum Gender
    {
        [DataMember]
        Male = 1,
        [DataMember]
        Female = 2,
        [DataMember]
        Other = 3
    }

    public enum UserType
    {
        [DataMember]
        Organization = 1,
        [DataMember]
        Self = 2
    }

    public enum OrganizationType
    {
        [DataMember]
        New = 1,
        [DataMember]
        Registered = 2
    }

    public enum UserRoles
    {
        [DataMember]
        SuperAdmin = 1,
        [DataMember]
        Admin = 2,
        [DataMember]
        User = 3
    }

    public enum LoginType
    {
        [DataMember]
        IsEmail,
        [DataMember]
        IsMobile,
        [DataMember]
        IsUserName
    }
}
