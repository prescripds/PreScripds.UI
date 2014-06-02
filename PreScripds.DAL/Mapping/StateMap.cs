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
        public StateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.StateName)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("State");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StateName).HasColumnName("StateName");
            this.Property(t => t.CountryId).HasColumnName("CountryId");

            // Relationships
            this.HasOptional(t => t.Country)
                .WithMany(t => t.States)
                .HasForeignKey(d => d.CountryId);

        }
    }
}
