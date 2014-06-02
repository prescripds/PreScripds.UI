using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain.Master;

namespace PreScripds.DAL.Mapping
{
    public class CityMap : EntityTypeConfiguration<City>
    {
        public CityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CityName)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("City");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CityName).HasColumnName("CityName");
            this.Property(t => t.StateId).HasColumnName("StateId");

            // Relationships
            this.HasOptional(t => t.State)
                .WithMany(t => t.Cities)
                .HasForeignKey(d => d.StateId);
        }
    }
}
