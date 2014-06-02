using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Domain
{
    public class LibraryAssetFile
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public byte[] Asset { get; set; }
        [DataMember]
        public long LibraryAssetId { get; set; }
        [DataMember]
        public System.DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual LibraryAsset LibraryAsset { get; set; }
    }
}
