﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class RoleInPermission
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long? RoleId { get; set; }
        [DataMember]
        public long? PermissionId { get; set; }
        [DataMember]
        public virtual Permission Permission { get; set; }
        [DataMember]
        public virtual Role Role { get; set; }
    }
}