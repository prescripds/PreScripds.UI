using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class ModuleObjectMap : EntityTypeConfiguration<ModuleObjects>
    {
        public ModuleObjectMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ModuleObjectName)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("ModuleObject");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ModuleObjectName).HasColumnName("ModuleObjectName");
            this.Property(t => t.ModuleId).HasColumnName("ModuleId");

            // Relationships
            this.HasOptional(t => t.Module)
                .WithMany(t => t.ModuleObjects)
                .HasForeignKey(d => d.ModuleId);

        }
    }
}
