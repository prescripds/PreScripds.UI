using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class UserInRoleMap : EntityTypeConfiguration<UserInRole>
    {
        public UserInRoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserInRole");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.Active).HasColumnName("Active");

            // Relationships
            this.HasOptional(t => t.Role)
                .WithMany(t => t.UserInRoles)
                .HasForeignKey(d => d.RoleId);
            this.HasOptional(t => t.User)
                .WithMany(t => t.UserInRoles)
                .HasForeignKey(d => d.UserId);

        }
    }
}
