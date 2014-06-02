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
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Captcha)
                .IsRequired();

            this.Property(t => t.PasswordCap)
                .IsRequired();

            this.Property(t => t.saltkey)
                .IsRequired();

            this.Property(t => t.IpAddress)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("UserHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Captcha).HasColumnName("Captcha");
            this.Property(t => t.PasswordCap).HasColumnName("PasswordCap");
            this.Property(t => t.saltkey).HasColumnName("saltkey");
            this.Property(t => t.IpAddress).HasColumnName("IpAddress");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UserloginId).HasColumnName("UserloginId");

            // Relationships
            this.HasRequired(t => t.UserLogin)
                .WithMany(t => t.UserHistories)
                .HasForeignKey(d => d.UserloginId);
        }
    }
}
