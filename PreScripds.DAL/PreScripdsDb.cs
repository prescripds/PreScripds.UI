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

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentInOrganization> DepartmentInOrganizations { get; set; }
        public DbSet<LibraryAsset> LibraryAssets { get; set; }
        public DbSet<LibraryAssetFile> LibraryAssetFiles { get; set; }
        public DbSet<LibraryFolder> LibraryFolders { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleInDepartment> ModuleInDepartments { get; set; }
        public DbSet<ModuleObjects> ModuleObjects { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionInModule> PermissionInModules { get; set; }
        public DbSet<PermissionInSet> PermissionInSets { get; set; }
        public DbSet<PermissionSet> PermissionSets { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleInPermission> RoleInPermissions { get; set; }
        public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserHistory> UserHistories { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new DepartmentInOrganizationMap());
            modelBuilder.Configurations.Add(new LibraryAssetMap());
            modelBuilder.Configurations.Add(new LibraryAssetFileMap());
            modelBuilder.Configurations.Add(new LibraryFolderMap());
            modelBuilder.Configurations.Add(new ModuleMap());
            modelBuilder.Configurations.Add(new ModuleInDepartmentMap());
            modelBuilder.Configurations.Add(new ModuleObjectMap());
            modelBuilder.Configurations.Add(new OrganizationMap());
            modelBuilder.Configurations.Add(new PermissionMap());
            modelBuilder.Configurations.Add(new PermissionInModuleMap());
            modelBuilder.Configurations.Add(new PermissionInSetMap());
            modelBuilder.Configurations.Add(new PermissionSetMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new RoleInPermissionMap());
            modelBuilder.Configurations.Add(new SecurityQuestionMap());
            modelBuilder.Configurations.Add(new StateMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserHistoryMap());
            modelBuilder.Configurations.Add(new UserInRoleMap());
            modelBuilder.Configurations.Add(new UserLoginMap());
        }

    }
}
