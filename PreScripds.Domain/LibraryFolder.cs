using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class LibraryFolder
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string FolderName { get; set; }
        [DataMember]
        public string FolderHierarchy { get; set; }
        [DataMember]
        public long? ParentFolderId { get; set; }
        [DataMember]
        public System.DateTime Createdate { get; set; }
        [DataMember]
        public long OrganizationId { get; set; }
        [DataMember]
        public virtual ICollection<LibraryAsset> LibraryAssets { get; set; }
        [DataMember]
        public virtual Organization Organization { get; set; }
    }
}
