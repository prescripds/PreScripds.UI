using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class LibraryAssetViewModel
    {
       
        public long Id { get; set; }
       
        public string AssetName { get; set; }
       
        public long LibraryFolderId { get; set; }
       
        public string AssetPath { get; set; }
       
        public string AssetType { get; set; }
       
        public int? AssetSize { get; set; }
       
        public string AssetDescription { get; set; }
       
        public bool Active { get; set; }
       
        public byte[] AssetThumbnail { get; set; }
       
        public System.DateTime CreatedDate { get; set; }
       
        public virtual LibraryFolder LibraryFolder { get; set; }
       
        public virtual ICollection<LibraryAssetFile> LibraryAssetFiles { get; set; }
    }
}