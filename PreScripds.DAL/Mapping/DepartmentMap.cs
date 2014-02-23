using System;
using System.Collections.Generic;
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
            this.HasKey(t => t.department_id);

            // Properties
            this.Property(t => t.department_name)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.department_desc)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("department", "mydb");
            this.Property(t => t.department_id).HasColumnName("department_id");
            this.Property(t => t.department_name).HasColumnName("department_name");
            this.Property(t => t.department_desc).HasColumnName("department_desc");
        }
    }
}
