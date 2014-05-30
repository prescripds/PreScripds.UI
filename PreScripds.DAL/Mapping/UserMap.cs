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
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(450);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(450);

            this.Property(t => t.MiddleName)
                .HasMaxLength(45);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(800);

            this.Property(t => t.AltEmail)
                .HasMaxLength(800);

            this.Property(t => t.Address)
                .HasMaxLength(750);

            this.Property(t => t.ZipCode)
                .HasMaxLength(45);

            this.Property(t => t.CityName)
               .HasMaxLength(250);

            this.Property(t => t.EmployeeId)
                .HasMaxLength(250);

            this.Property(t => t.Designation)
                .HasMaxLength(450);

            // Table & Column Mappings
            this.ToTable("user", "turtleinc");
            this.Property(t => t.UserId).HasColumnName("user_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.FirstName).HasColumnName("firstname");
            this.Property(t => t.LastName).HasColumnName("lastname");
            this.Property(t => t.MiddleName).HasColumnName("middlename");
            this.Property(t => t.Gender).HasColumnName("gender");
            this.Property(t => t.Dob).HasColumnName("dob");
            this.Property(t => t.Mobile).HasColumnName("mobile");
            this.Property(t => t.Phone).HasColumnName("phone");
            this.Property(t => t.Age).HasColumnName("age");
            this.Property(t => t.Email).HasColumnName("email");
            this.Property(t => t.AltEmail).HasColumnName("alt_email");
            this.Property(t => t.AltMobile).HasColumnName("alt_mobile");
            this.Property(t => t.Address).HasColumnName("address");
            this.Property(t => t.CityId).HasColumnName("city_id");
            this.Property(t => t.StateId).HasColumnName("state_id");
            this.Property(t => t.CountryId).HasColumnName("country_id");
            this.Property(t => t.ZipCode).HasColumnName("zipcode");
            this.Property(t => t.Active).HasColumnName("active");
            this.Property(t => t.AdminApprove).HasColumnName("adminapprove");
            this.Property(t => t.IsSuperAdmin).HasColumnName("issuperadmin");
            this.Property(t => t.IsAdmin).HasColumnName("isadmin");
            this.Property(t => t.IsHomeUrl).HasColumnName("ishomeurl");
            this.Property(t => t.CityName).HasColumnName("city_name");
            this.Property(t => t.UserType).HasColumnName("user_type");
            this.Property(t => t.TermsCondition).HasColumnName("terms_condition");
            this.Property(t => t.ReferencedId).HasColumnName("referenced_id");
            this.Property(t => t.OrganizationId).HasColumnName("organization_id");
            this.Property(t => t.EmployeeId).HasColumnName("employee_id");
            this.Property(t => t.Designation).HasColumnName("designation");
            this.Property(t => t.IsOrganization).HasColumnName("isorganization");
            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.IsEmailConfirmed).HasColumnName("isemailconfirmed");
            this.Property(t => t.DisplayPic).HasColumnName("displaypic");
        }
    }
}
