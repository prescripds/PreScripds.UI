using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class UserLoginMap : EntityTypeConfiguration<UserLogin>
    {
        public UserLoginMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserName)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("UserLogin");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.saltkey).HasColumnName("saltkey");
            this.Property(t => t.SecurityQuestionId).HasColumnName("SecurityQuestionId");
            this.Property(t => t.SecurityAnswer).HasColumnName("SecurityAnswer");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Captcha).HasColumnName("Captcha");
            this.Property(t => t.PasswordCap).HasColumnName("PasswordCap");

            // Relationships
            this.HasOptional(t => t.SecurityQuestion)
                .WithMany(t => t.UserLogins)
                .HasForeignKey(d => d.SecurityQuestionId);
            this.HasOptional(t => t.User)
                .WithMany(t => t.UserLogins)
                .HasForeignKey(d => d.UserId);

        }
    }
}
