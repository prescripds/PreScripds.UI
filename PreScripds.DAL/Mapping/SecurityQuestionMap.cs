using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain.Master;

namespace PreScripds.DAL.Mapping
{
    public class SecurityQuestionMap:EntityTypeConfiguration<SecurityQuestion>
    {
        public SecurityQuestionMap()
        {
            // Primary Key
            this.HasKey(t => t.SecurityQuestionId);

            // Properties
            this.Property(t => t.SecurityQuestionName)
                .IsRequired()
                .HasMaxLength(750);

            // Table & Column Mappings
            this.ToTable("securtiyquestion", "turtleinc");
            this.Property(t => t.SecurityQuestionId).HasColumnName("securityquestion_id");
            this.Property(t => t.SecurityQuestionName).HasColumnName("securityquestion_name");
        }
    }
}
