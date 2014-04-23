using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Entity;
using PreScripds.DAL.Mapping;
using PreScripds.Domain;
using PreScripds.Domain.Master;

namespace PreScripds.DAL
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class PreScripdsDb : DbContext
    {
        public PreScripdsDb()
            : base("Name=PreScripdsDb")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<City> cities { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<SecurityQuestion> securtiyquestions { get; set; }
        public DbSet<State> states { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<UserLogin> user_login { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<Permission> permissions { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Organization> organizations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new SecurityQuestionMap());
            modelBuilder.Configurations.Add(new StateMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserLoginMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new PermissionMap());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new OrganizationMap());
        }

    }
}
