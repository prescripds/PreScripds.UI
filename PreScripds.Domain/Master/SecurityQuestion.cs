﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Master
{
    public class SecurityQuestion
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string QuestionName { get; set; }
        [DataMember]
        public virtual ICollection<UserLogin> UserLogins { get; set; }
    }
}
