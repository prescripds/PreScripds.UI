using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class ModuleInDepartmentMap : EntityTypeConfiguration<ModuleInDepartment>
    {
        public ModuleInDepartmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ModuleInDepartment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ModuleId).HasColumnName("ModuleId");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.Active).HasColumnName("Active");

            // Relationships
            this.HasOptional(t => t.Department)
                .WithMany(t => t.ModuleInDepartments)
                .HasForeignKey(d => d.DepartmentId);
            this.HasOptional(t => t.Module)
                .WithMany(t => t.ModuleInDepartments)
                .HasForeignKey(d => d.ModuleId);

        }
    }
}
