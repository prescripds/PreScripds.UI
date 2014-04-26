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
            this.HasKey(t => t.OrganizationId);

            // Properties
            this.Property(t => t.OrganizationName)
                .IsRequired()
                .HasMaxLength(450);

            this.Property(t => t.OrganizationAddress)
                .HasMaxLength(450);

            this.Property(t => t.OrganizationEmail)
                .IsRequired()
                .HasMaxLength(450);

            this.Property(t => t.OrganizationContact)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.ReferencedEmail)
                .HasMaxLength(450);

            this.Property(t => t.EmployeeIdOrg)
                .HasMaxLength(450);

            this.Property(t => t.ApproverId)
                .HasMaxLength(450);

            // Table & Column Mappings
            this.ToTable("organization", "turtleinc");
            this.Property(t => t.OrganizationId).HasColumnName("organization_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.OrganizationName).HasColumnName("organization_name");
            this.Property(t => t.OrganizationAddress).HasColumnName("organization_address");
            this.Property(t => t.OrganizationPhone).HasColumnName("organization_phone");
            this.Property(t => t.OrganizationMobile).HasColumnName("organization_mobile");
            this.Property(t => t.OrganizationEmail).HasColumnName("organization_email");
            this.Property(t => t.OrganizationContact).HasColumnName("organization_contact");
            this.Property(t => t.VerificationDate).HasColumnName("verification_date");
            this.Property(t => t.ReferencedEmail).HasColumnName("referenced_email");
            this.Property(t => t.EmployeeIdOrg).HasColumnName("employee_id_org");
            this.Property(t => t.OrganiztionIncorporation).HasColumnName("organiztion_incorporation");
            this.Property(t => t.Isactive).HasColumnName("isactive");
            this.Property(t => t.IsApproved).HasColumnName("isapproved");
            this.Property(t => t.ApproverId).HasColumnName("approver_id");
            this.Property(t => t.ApprovedDate).HasColumnName("approved_date");
        }
    }
}
