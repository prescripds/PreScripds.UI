using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class LibraryAsset
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string AssetName { get; set; }
        [DataMember]
        public long LibraryFolderId { get; set; }
        [DataMember]
        public string AssetPath { get; set; }
        [DataMember]
        public string AssetType { get; set; }
        [DataMember]
        public int? AssetSize { get; set; }
        [DataMember]
        public string AssetDescription { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public byte[] AssetThumbnail { get; set; }
        [DataMember]
        public System.DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual LibraryFolder LibraryFolder { get; set; }
        [DataMember]
        public virtual ICollection<LibraryAssetFile> LibraryAssetFiles { get; set; }
    }
}
