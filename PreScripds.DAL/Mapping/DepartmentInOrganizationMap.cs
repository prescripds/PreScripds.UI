using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class DepartmentInOrganizationMap : EntityTypeConfiguration<DepartmentInOrganization>
    {
        public DepartmentInOrganizationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("DepartmentInOrganization");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.OrganizationId).HasColumnName("OrganizationId");
            this.Property(t => t.Active).HasColumnName("Active");

            // Relationships
            this.HasOptional(t => t.Department)
                .WithMany(t => t.DepartmentInOrganizations)
                .HasForeignKey(d => d.DepartmentId);
            this.HasOptional(t => t.Organization)
                .WithMany(t => t.DepartmentInOrganizations)
                .HasForeignKey(d => d.OrganizationId);

        }
    }
}
