﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    [DataContract(IsReference = true)]
    public class RoleInPermission
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long? RoleId { get; set; }
        [DataMember]
        public long? PermissionId { get; set; }
        [DataMember]
        public virtual PermissionSet PermissionSet { get; set; }
        [DataMember]
        public virtual Role Role { get; set; }
        [DataMember]
        public bool? Active { get; set; }
    }
}
