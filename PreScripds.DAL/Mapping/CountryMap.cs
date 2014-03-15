﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain.Master;

namespace PreScripds.DAL.Mapping
{
    public class CountryMap:EntityTypeConfiguration<Country>
    {
        public CountryMap()
        {
            // Primary Key
            this.HasKey(t => t.CountryId);

            // Properties
            this.Property(t => t.CountryName)
                .IsRequired()
                .HasMaxLength(450);

            this.Property(t => t.CountryCode)
                .IsRequired()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("country", "turtleinc");
            this.Property(t => t.CountryId).HasColumnName("country_id");
            this.Property(t => t.CountryName).HasColumnName("country_name");
            this.Property(t => t.CountryCode).HasColumnName("country_code");
        }
    }
}
