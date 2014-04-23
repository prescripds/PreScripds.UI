using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {

            // Primary Key
            this.HasKey(t => t.RoleId);

            // Properties
            this.Property(t => t.RoleName)
                .IsRequired()
                .HasMaxLength(450);

            this.Property(t => t.RoleDesc)
                .HasMaxLength(450);

            // Table & Column Mappings
            this.ToTable("roles", "turtleinc");
            this.Property(t => t.RoleId).HasColumnName("role_id");
            this.Property(t => t.RoleName).HasColumnName("role_name");
            this.Property(t => t.RoleDesc).HasColumnName("role_desc");
            this.Property(t => t.PermissionId).HasColumnName("permission_id");
            this.Property(t => t.OrganizationId).HasColumnName("organization_id");
            this.Property(t => t.DepartmentId).HasColumnName("department_id");

            // Relationships
            this.HasRequired(t => t.Permission)
                .WithMany(t => t.Roles)
                .HasForeignKey(d => d.PermissionId);
        }
    }
}
