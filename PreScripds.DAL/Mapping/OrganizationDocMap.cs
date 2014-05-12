using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class OrganizationDocMap : EntityTypeConfiguration<OrganizationDoc>
    {
        public OrganizationDocMap()
        {
            // Primary Key
            this.HasKey(t => t.OrganizationDocId);

            // Properties
            this.Property(t => t.OrganizationDocName)
                .IsRequired()
                .HasMaxLength(450);

            this.Property(t => t.OrganizationDocument)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("organization_doc", "turtleinc");
            this.Property(t => t.OrganizationDocId).HasColumnName("organizationdoc_id");
            this.Property(t => t.OrganizationDocName).HasColumnName("organizationdoc_name");
            this.Property(t => t.OrganizationDocument).HasColumnName("organizationdoc");
            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.OrganizationId).HasColumnName("organization_id");
            this.Property(t => t.CreatedBy).HasColumnName("createdby");
            this.Property(t => t.IsActive).HasColumnName("isactive");

            // Relationships
            this.HasOptional(t => t.Organization)
                .WithMany(t => t.OrganizationDoc)
                .HasForeignKey(d => d.OrganizationId);
        }
    }
}
