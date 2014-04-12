using System;
using System.Collections.Generic;
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

            // Table & Column Mappings
            this.ToTable("organization", "turtleinc");
            this.Property(t => t.OrganizationId).HasColumnName("organization_id");
            this.Property(t => t.OrganizationName).HasColumnName("organization_name");
        }
    }
}
