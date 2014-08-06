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
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.MiddleName)
                .HasMaxLength(250);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Alt_Email)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Address)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("User");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.MiddleName).HasColumnName("MiddleName");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.Dob).HasColumnName("Dob");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Age).HasColumnName("Age");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Alt_Email).HasColumnName("Alt_Email");
            this.Property(t => t.Alt_Mobile).HasColumnName("Alt_Mobile");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.CityName).HasColumnName("CityName");
            this.Property(t => t.StateId).HasColumnName("StateId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.Zipcode).HasColumnName("Zipcode");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.AdminApprove).HasColumnName("AdminApprove");
            this.Property(t => t.IsHomeUrl).HasColumnName("IsHomeUrl");
            this.Property(t => t.UserType).HasColumnName("UserType");
            this.Property(t => t.TermsCondition).HasColumnName("TermsCondition");
            this.Property(t => t.OrganizationId).HasColumnName("OrganizationId");
            this.Property(t => t.IsOrganization).HasColumnName("IsOrganization");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.IsOrgSuperAdmin).HasColumnName("IsOrgSuperAdmin");
            // Relationships
            this.HasRequired(t => t.Country)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.CountryId);
            //this.HasRequired(t => t.Organization)
            //    .WithMany(t => t.Users)
            //    .HasForeignKey(d => d.OrganizationId);
        }
    }
}
