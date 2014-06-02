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
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DepartmentName)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.DepartmentDescription)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Department");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DepartmentName).HasColumnName("DepartmentName");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.DepartmentDescription).HasColumnName("DepartmentDescription");
        }
    }
}
