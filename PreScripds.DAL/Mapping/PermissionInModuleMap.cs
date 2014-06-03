using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class PermissionInModuleMap : EntityTypeConfiguration<PermissionInModule>
    {
        public PermissionInModuleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("PermissionInModule");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PermissionId).HasColumnName("PermissionId");
            this.Property(t => t.ModuleId).HasColumnName("ModuleId");
            this.Property(t => t.Active).HasColumnName("Active");
            // Relationships
            this.HasOptional(t => t.Module)
                .WithMany(t => t.PermissionInModules)
                .HasForeignKey(d => d.ModuleId);
            this.HasOptional(t => t.Permission)
                .WithMany(t => t.PermissionInModules)
                .HasForeignKey(d => d.PermissionId);

        }
    }
}
