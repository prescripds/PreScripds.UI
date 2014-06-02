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
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SecurityQuestion");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.QuestionName).HasColumnName("QuestionName");
        }
    }
}
