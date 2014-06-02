using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class ModuleMap : EntityTypeConfiguration<Module>
    {
        public ModuleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ModuleName)
                .HasMaxLength(250);

            this.Property(t => t.ModuleDescription)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Module");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ModuleName).HasColumnName("ModuleName");
            this.Property(t => t.ModuleDescription).HasColumnName("ModuleDescription");
        }
    }
}
