using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class OrganizationDoc
    {
        public long OrganizationDocId { get; set; }
        public string OrganizationDocName { get; set; }
        public byte[] OrganizationDocument { get; set; }
        public DateTime CreatedDate { get; set; }
        public long OrganizationId { get; set; }
        public long CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
