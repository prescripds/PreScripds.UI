using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain.Master;

namespace PreScripds.DAL.Mapping
{
    public class StateMap:EntityTypeConfiguration<State>
    {
        public stateMap()
        {
            // Primary Key
            this.HasKey(t => t.StateId);

            // Properties
            this.Property(t => t.StateName)
                .IsRequired()
                .HasMaxLength(450);

            this.Property(t => t.StateCode)
                .IsRequired()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("state", "turtleinc");
            this.Property(t => t.StateId).HasColumnName("state_id");
            this.Property(t => t.StateName).HasColumnName("state_name");
            this.Property(t => t.StateCode).HasColumnName("state_code");
            this.Property(t => t.CountryId).HasColumnName("country_id");

            // Relationships
            this.HasRequired(t => t.Country)
                .WithMany(t => t.States)
                .HasForeignKey(d => d.CountryId);

        }
    }
}
