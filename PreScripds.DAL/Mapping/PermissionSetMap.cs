using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class PermissionSetMap : EntityTypeConfiguration<PermissionSet>
    {
        public PermissionSetMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PermissionSetName)
                .HasMaxLength(250);
            this.Property(t => t.PermissionSetDescription)
                .HasMaxLength(550);

            // Table & Column Mappings
            this.ToTable("PermissionSet");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PermissionSetName).HasColumnName("PermissionSetName");
            this.Property(t => t.PermissionSetDescription).HasColumnName("PermissionSetDescription");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.ModuleId).HasColumnName("ModuleId");
            this.Property(t => t.Active).HasColumnName("Active");

            // Relationships
            this.HasOptional(t => t.Department)
                .WithMany(t => t.PermissionSets)
                .HasForeignKey(d => d.DepartmentId);
        }
    }
}
