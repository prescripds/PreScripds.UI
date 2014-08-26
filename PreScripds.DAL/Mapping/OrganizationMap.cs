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
    public class OrganizationMap : EntityTypeConfiguration<Organization>
    {
        public OrganizationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.OrganizationName)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.OrganizationAddress)
                .HasMaxLength(500);

            this.Property(t => t.OrganizationEmail)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.ContactPerson)
                .HasMaxLength(500);

            this.Property(t => t.Org_EmployeeId)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Organization");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrganizationName).HasColumnName("OrganizationName");
            this.Property(t => t.OrganizationAddress).HasColumnName("OrganizationAddress");
            this.Property(t => t.OrganizationPhone).HasColumnName("OrganizationPhone");
            this.Property(t => t.OrganizationMobile).HasColumnName("OrganizationMobile");
            this.Property(t => t.OrganizationEmail).HasColumnName("OrganizationEmail");
            this.Property(t => t.ContactPerson).HasColumnName("ContactPerson");
            this.Property(t => t.VerificationDate).HasColumnName("VerificationDate");
            this.Property(t => t.Org_EmployeeId).HasColumnName("Org_EmployeeId");
            this.Property(t => t.OrganizationIncorporation).HasColumnName("OrganizationIncorporation");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.Approved).HasColumnName("Approved");
            this.Property(t => t.ApproverId).HasColumnName("ApproverId");
            this.Property(t => t.ApprovedDate).HasColumnName("ApprovedDate");
            this.Property(t => t.IsQuickView).HasColumnName("IsQuickView");
            this.Property(t => t.IsSelfie).HasColumnName("IsSelfie");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.IsHomeOrg).HasColumnName("IsHomeOrg");
        }
    }
}
