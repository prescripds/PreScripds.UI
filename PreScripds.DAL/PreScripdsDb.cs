using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.DAL.Mapping;
using PreScripds.Domain;

namespace PreScripds.DAL
{
    public class PreScripdsDb : DbContext
    {
        static PreScripdsDb()
        {
            Database.SetInitializer<PreScripdsDb>(null);
        }
        public PreScripdsDb()
            : base("Name=PreScripdsDb")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Department> departments { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new RoleMap());
            //modelBuilder.Configurations.Add(new organizationMap());
            //modelBuilder.Configurations.Add(new organization_departmentMap());
            //modelBuilder.Configurations.Add(new organization_department_masterMap());
            //modelBuilder.Configurations.Add(new organization_roleMap());
            //modelBuilder.Configurations.Add(new organization_statusMap());
            //modelBuilder.Configurations.Add(new organization_userMap());
            //modelBuilder.Configurations.Add(new organizationsocial_detailMap());
            //modelBuilder.Configurations.Add(new organizationsocial_masterMap());
            //modelBuilder.Configurations.Add(new organizationtype_masterMap());
            //modelBuilder.Configurations.Add(new patient_detailMap());
            //modelBuilder.Configurations.Add(new patient_folderMap());
            //modelBuilder.Configurations.Add(new patient_organizationMap());
            //modelBuilder.Configurations.Add(new patientlibrary_assetMap());
            //modelBuilder.Configurations.Add(new roleMap());
            //modelBuilder.Configurations.Add(new userMap());
            //modelBuilder.Configurations.Add(new valueadded_serviceMap());
        }

    }
}
