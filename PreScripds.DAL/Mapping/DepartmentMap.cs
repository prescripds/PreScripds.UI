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
    public class DepartmentMap : EntityTypeConfiguration<Department>
    {
        public DepartmentMap()
        {
            // Primary Key
            this.HasKey(t => t.DepartmentId);

            // Properties
            this.Property(t => t.DepartmentName)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.DepartmentDesc)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("department", "turtleinc");
            this.Property(t => t.DepartmentId).HasColumnName("department_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.DepartmentName).HasColumnName("department_name");
            this.Property(t => t.DepartmentDesc).HasColumnName("department_desc");
            this.Property(t => t.OrganizationId).HasColumnName("fk_organization_id");
            this.Property(t => t.IsActive).HasColumnName("isactive");

            // Relationships
            this.HasRequired(t => t.Organization)
                .WithMany(t => t.Departments)
                .HasForeignKey(d => d.OrganizationId);
        }
    }
}
