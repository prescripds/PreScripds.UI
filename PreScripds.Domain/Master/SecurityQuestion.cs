using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain.Master
{
    public class SecurityQuestion
    {
        public long SecurityQuestionId { get; set; }
        public string SecurityQuestionName { get; set; }
        public virtual ICollection<UserLogin> UserLogin { get; set; }
    }
}
