using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class LibraryAssetMap : EntityTypeConfiguration<LibraryAsset>
    {
        public LibraryAssetMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AssetName)
                .HasMaxLength(500);

            this.Property(t => t.AssetPath)
                .IsRequired();

            this.Property(t => t.AssetType)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.AssetDescription)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("LibraryAsset");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AssetName).HasColumnName("AssetName");
            this.Property(t => t.LibraryFolderId).HasColumnName("LibraryFolderId");
            this.Property(t => t.AssetPath).HasColumnName("AssetPath");
            this.Property(t => t.AssetType).HasColumnName("AssetType");
            this.Property(t => t.AssetSize).HasColumnName("AssetSize");
            this.Property(t => t.AssetDescription).HasColumnName("AssetDescription");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.AssetThumbnail).HasColumnName("AssetThumbnail");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

            // Relationships
            this.HasRequired(t => t.LibraryFolder)
                .WithMany(t => t.LibraryAssets)
                .HasForeignKey(d => d.LibraryFolderId);

        }
    }
}
