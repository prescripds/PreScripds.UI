using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.user_id);

            // Properties
            this.Property(t => t.firstname)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.lastname)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.email)
                .IsRequired()
                .HasMaxLength(450);

            this.Property(t => t.address)
                .HasMaxLength(700);

            this.Property(t => t.password)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.recovery_email)
                .HasMaxLength(450);

            this.Property(t => t.username)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.login_ip)
                .HasMaxLength(500);

            this.Property(t => t.salt_key)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("user", "mydb");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.firstname).HasColumnName("firstname");
            this.Property(t => t.lastname).HasColumnName("lastname");
            this.Property(t => t.gender).HasColumnName("gender");
            this.Property(t => t.dob).HasColumnName("dob");
            this.Property(t => t.mobile).HasColumnName("mobile");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.address).HasColumnName("address");
            this.Property(t => t.roleid).HasColumnName("roleid");
            this.Property(t => t.isapproved).HasColumnName("isapproved");
            this.Property(t => t.active).HasColumnName("active");
            this.Property(t => t.password).HasColumnName("password");
            this.Property(t => t.recovery_email).HasColumnName("recovery_email");
            this.Property(t => t.recovery_phone).HasColumnName("recovery_phone");
            this.Property(t => t.created_date).HasColumnName("created_date");
            this.Property(t => t.updated_date).HasColumnName("updated_date");
            this.Property(t => t.approved_by).HasColumnName("approved_by");
            this.Property(t => t.last_login_date).HasColumnName("last_login_date");
            this.Property(t => t.username).HasColumnName("username");
            this.Property(t => t.login_ip).HasColumnName("login_ip");
            this.Property(t => t.department_id).HasColumnName("department_id");
            this.Property(t => t.salt_key).HasColumnName("salt_key");

            // Relationships
            //this.HasRequired(t => t.department)
            //    .WithMany(t => t.users)
            //    .HasForeignKey(d => d.department_id);
            this.HasRequired(t => t.role)
                .WithMany(t => t.users)
                .HasForeignKey(d => d.roleid);

        }
    }
}
