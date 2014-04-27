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
    public class UserDepartmentMap : EntityTypeConfiguration<UserDepartmentMapping>
    {
        public UserDepartmentMap()
        {
            this.HasKey(t => t.UserDepartmentId);

            // Properties
            // Table & Column Mappings
            this.ToTable("userdepartmentmapping", "turtleinc");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.DepartmentId).HasColumnName("department_id");
            this.Property(t => t.UserDepartmentId).HasColumnName("userdepartment_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            // this.Property(t => t.IsActive).HasColumnName("isactive");
            // this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.RoleId).HasColumnName("role_id");

            // Relationships
            //this.HasOptional(t => t.Department)
            //    .WithMany(t => t.UserDepartmentMappings)
            //    .HasForeignKey(d => d.DepartmentId);
            //this.HasOptional(t => t.Role)
            //    .WithMany(t => t.UserDepartmentMappings)
            //    .HasForeignKey(d => d.RoleId);
            //this.HasOptional(t => t.User)
            //    .WithMany(t => t.UserDepartmentMappings)
            //    .HasForeignKey(d => d.UserId);
        }
    }
}
