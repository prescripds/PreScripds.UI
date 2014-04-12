using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class PermissionMap : EntityTypeConfiguration<Permission>
    {
        public PermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.PermissionId);

            // Properties
            this.Property(t => t.PermissionName)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.PermissionDesc)
                .IsRequired()
                .HasMaxLength(450);

            // Table & Column Mappings
            this.ToTable("permission", "turtleinc");
            this.Property(t => t.PermissionId).HasColumnName("permission_id");
            this.Property(t => t.PermissionName).HasColumnName("permission_name");
            this.Property(t => t.PermissionDesc).HasColumnName("permission_desc");
        }
    }
}
