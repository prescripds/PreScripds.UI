using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class RoleInPermissionMap : EntityTypeConfiguration<RoleInPermission>
    {
        public RoleInPermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("RoleInPermission");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.PermissionId).HasColumnName("PermissionId");
            this.Property(t => t.Active).HasColumnName("Active");

            // Relationships
            this.HasOptional(t => t.Permission)
                .WithMany(t => t.RoleInPermissions)
                .HasForeignKey(d => d.PermissionId);
            this.HasOptional(t => t.Role)
                .WithMany(t => t.RoleInPermissions)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
