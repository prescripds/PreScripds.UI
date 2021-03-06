﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Mapping
{
    public class PermissionMap : EntityTypeConfiguration<Permission>
    {
        public PermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PermissionName)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("Permission");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PermissionName).HasColumnName("PermissionName");
            this.Property(t => t.Active).HasColumnName("Active");
        }
    }
}
