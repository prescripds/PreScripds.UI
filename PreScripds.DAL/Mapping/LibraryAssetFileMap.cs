using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class LibraryAssetFileMap : EntityTypeConfiguration<LibraryAssetFile>
    {
        public LibraryAssetFileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Asset)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("LibraryAssetFile");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Asset).HasColumnName("Asset");
            this.Property(t => t.LibraryAssetId).HasColumnName("LibraryAssetId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

            // Relationships
            this.HasRequired(t => t.LibraryAsset)
                .WithMany(t => t.LibraryAssetFiles)
                .HasForeignKey(d => d.LibraryAssetId);

        }
    }
}
