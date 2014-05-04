using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class UserHistoryMap : EntityTypeConfiguration<UserHistory>
    {
        public UserHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.UserHistoryId);

            // Properties
            this.Property(t => t.Captcha)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.PasswordCap)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.SaltKey)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.IpAddress)
                .IsRequired()
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("user_history", "turtleinc");
            this.Property(t => t.UserHistoryId).HasColumnName("userhistory_id");
            this.Property(t => t.Captcha).HasColumnName("captcha");
            this.Property(t => t.PasswordCap).HasColumnName("password_cap");
            this.Property(t => t.SaltKey).HasColumnName("salt_key");
            this.Property(t => t.IpAddress).HasColumnName("ip_address");
            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.UserId).HasColumnName("user_id");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.UserHistory)
                .HasForeignKey(d => d.UserId);
        }
    }
}
