using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class LibraryFolderMap : EntityTypeConfiguration<LibraryFolder>
    {
        public LibraryFolderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FolderName)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("LibraryFolder");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FolderName).HasColumnName("FolderName");
            this.Property(t => t.FolderHierarchy).HasColumnName("FolderHierarchy");
            this.Property(t => t.ParentFolderId).HasColumnName("ParentFolderId");
            this.Property(t => t.Createdate).HasColumnName("Createdate");
            this.Property(t => t.OrganizationId).HasColumnName("OrganizationId");

            // Relationships
            this.HasRequired(t => t.Organization)
                .WithMany(t => t.LibraryFolders)
                .HasForeignKey(d => d.OrganizationId);

        }
    }
}
