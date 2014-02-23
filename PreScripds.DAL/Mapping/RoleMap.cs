using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.role_id);

            // Properties
            this.Property(t => t.role_name)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.role_desc)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("role", "mydb");
            this.Property(t => t.role_id).HasColumnName("role_id");
            this.Property(t => t.role_name).HasColumnName("role_name");
            this.Property(t => t.role_desc).HasColumnName("role_desc");
        }
    }
}
