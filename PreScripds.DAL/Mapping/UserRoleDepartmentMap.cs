using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class UserRoleDepartmentMap : EntityTypeConfiguration<UserRoleDepartment>
    {
        public UserRoleDepartmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserRoleDepartment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.Active).HasColumnName("Active");

            // Relationships
            this.HasOptional(t => t.Department)
                .WithMany(t => t.UserRoleDepartments)
                .HasForeignKey(d => d.DepartmentId);
            this.HasOptional(t => t.Role)
                .WithMany(t => t.UserRoleDepartments)
                .HasForeignKey(d => d.RoleId);
            this.HasOptional(t => t.User)
                .WithMany(t => t.UserRoleDepartments)
                .HasForeignKey(d => d.UserId);

        }
    }
}
