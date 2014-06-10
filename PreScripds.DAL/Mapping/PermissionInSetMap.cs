using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class PermissionInSetMap : EntityTypeConfiguration<PermissionInSet>
    {
        public PermissionInSetMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("PermissionInSet");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PermissionId).HasColumnName("PermissionId");
            this.Property(t => t.PermissionSetId).HasColumnName("PermissionSetId");

            // Relationships
            this.HasOptional(t => t.Permission)
                .WithMany(t => t.PermissionInSets)
                .HasForeignKey(d => d.PermissionId);
            this.HasOptional(t => t.PermissionSet)
                .WithMany(t => t.PermissionInSets)
                .HasForeignKey(d => d.PermissionSetId);

        }
    }
}
