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
            this.HasKey(t => t.UserLoginId);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(750);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.SaltKey)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.SecurityAnswer)
                .IsRequired()
                .HasMaxLength(450);

            // Table & Column Mappings
            this.ToTable("user_login", "turtleinc");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.UserName).HasColumnName("username");
            this.Property(t => t.Password).HasColumnName("password");
            this.Property(t => t.SaltKey).HasColumnName("saltkey");
            this.Property(t => t.SecurityQuestionId).HasColumnName("securityquestion_id");
            this.Property(t => t.SecurityAnswer).HasColumnName("security_answer");
            this.Property(t => t.UserLoginId).HasColumnName("userlogin_id");

            // Relationships

            this.HasRequired(t => t.User)
                .WithMany(t => t.UserLogin)
                .HasForeignKey(d => d.UserId);

        }
    }
}
