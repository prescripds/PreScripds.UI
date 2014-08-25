using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.DAL.Interface;
using PreScripds.Domain;
using PreScripds.Infrastructure.UnitOfWork;
using PreScripds.Infrastructure.Utilities;
using PreScripds.Infrastructure;

namespace PreScripds.DAL.Repository
{
    using System.Data.Entity;
    using PreScripds.Infrastructure.Utilities;
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly PreScripdsDb _dbContext;
        public OrganizationRepository(PreScripdsDb context)
        {
            _dbContext = context;
        }
        public Organization GetOrganizationById(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var organization = uow.GetRepository<Organization>().Items.
                    Include(z => z.Roles.Select(s => s.UserInRoles)).
                    Include(z => z.Roles.Select(s => s.RoleInPermissions)).
                    Include(s => s.LibraryFolders.Select(y => y.LibraryAssets))
                    .FirstOrDefault(x => x.Id == organizationId);
                return organization;
            }
        }

        public LibraryFolder GetDocLibraryFolder(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var libFldr = uow.GetRepository<LibraryFolder>().Items.Include(s => s.LibraryAssets).FirstOrDefault(x => x.FolderName == "Documents" && x.OrganizationId == organizationId);
                return libFldr;
            }
        }

        public LibraryAsset CheckDocExists(string docName)
        {
            using (var uow = new UnitOfWork())
            {
                var libAsset = uow.GetRepository<LibraryAsset>().Items.FirstOrDefault(x => x.AssetName == docName);
                return libAsset;
            }
        }

        public LibraryAsset AddDocLibraryAsset(LibraryAsset libraryAsset)
        {
            using (var uow = new UnitOfWork())
            {
                if (libraryAsset.LibraryAssetFiles.IsCollectionValid())
                {
                    uow.GetRepository<LibraryAsset>().Items
                        .SelectMany(x => x.LibraryAssetFiles)
                        .Each(s => uow.GetRepository<LibraryAssetFile>().Items.ToList().Add(s));
                }
                uow.GetRepository<LibraryAsset>().Insert(libraryAsset);
                uow.SaveChanges();
                return libraryAsset;
            }
        }
        public List<DepartmentInOrganization> GetDepartmentInOrganization(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var deptInOrg = uow.GetRepository<DepartmentInOrganization>().Items.Where(x => x.OrganizationId == organizationId).ToList();
                return deptInOrg;
            }
        }

        public List<ModuleInDepartment> GetModuleInDepartment()
        {
            using (var uow = new UnitOfWork())
            {
                var modInDept = uow.GetRepository<ModuleInDepartment>().Items.ToList();
                return modInDept;
            }
        }

        public void AddDepartmentInOrg(List<DepartmentInOrganization> deptInOrg)
        {
            using (var uow = new UnitOfWork())
            {
                foreach (var departmentInOrg in deptInOrg)
                {
                    var checkDeptExists = uow.GetRepository<DepartmentInOrganization>().Items.FirstOrDefault(x => x.DepartmentId == departmentInOrg.DepartmentId);
                    if (checkDeptExists.IsNull())
                    {
                        uow.GetRepository<DepartmentInOrganization>().Insert(departmentInOrg);
                    }
                }
                uow.SaveChanges();
            }
        }

        public List<Department> GetDepartmentInOrg(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var deptInOrgs = GetDepartmentInOrganization(organizationId);
                var depts = new List<Department>();
                foreach (var deptInOrg in deptInOrgs)
                {
                    var depmts = uow.GetRepository<Department>().Items.FirstOrDefault(x => x.Id == deptInOrg.DepartmentId);
                    depts.Add(depmts);
                }
                return depts;
            }
        }

        public void AddDepartment(Department department)
        {
            using (var uow = new UnitOfWork())
            {
                uow.GetRepository<Department>().Items.SelectMany(x => x.DepartmentInOrganizations).Each(s => uow.GetRepository<DepartmentInOrganization>().Items.ToList().Add(s));
                uow.GetRepository<Department>().Insert(department);
                uow.SaveChanges();
            }
        }

        public Department GetDepartmentById(long departmentId)
        {
            using (var uow = new UnitOfWork())
            {
                var department = uow.GetRepository<Department>().Items.Include(x => x.ModuleInDepartments).FirstOrDefault(x => x.Id == departmentId);
                return department;
            }
        }

        public void AddModule(Module module)
        {
            using (var uow = new UnitOfWork())
            {
                uow.GetRepository<Module>().Items.SelectMany(x => x.ModuleInDepartments).Each(s => uow.GetRepository<ModuleInDepartment>().Items.ToList().Add(s));
                uow.GetRepository<Module>().Insert(module);
                uow.SaveChanges();
            }
        }

        public List<ModuleInDepartment> GetModuleInDepartment(long departmentId)
        {
            using (var uow = new UnitOfWork())
            {
                var modInDept = uow.GetRepository<ModuleInDepartment>().Items.Where(x => x.DepartmentId == departmentId && x.Active).ToList();
                return modInDept;
            }
        }

        public Module GetModuleById(long moduleId)
        {
            using (var uow = new UnitOfWork())
            {
                var module = uow.GetRepository<Module>().Items.FirstOrDefault(x => x.Id == moduleId);
                return module;
            }
        }

        public List<Module> GetAllModule(long departmentId)
        {
            using (var uow = new UnitOfWork())
            {
                var moduleList = new List<Module>();
                if (departmentId != 0)
                {
                    moduleList = uow.GetRepository<Module>().Items.Where(x => x.DepartmentId == departmentId).ToList();
                    if (!moduleList.IsCollectionValid())
                    {
                        var modules = uow.GetRepository<Module>().Items.Where(x => x.DepartmentId == null).ToList();
                        foreach (var mod in modules)
                        {
                            moduleList.Add(mod);
                        }
                    }
                }

                return moduleList;
            }
        }

        public void AddModuleInDepartment(List<ModuleInDepartment> moduleInDepartment)
        {
            using (var uow = new UnitOfWork())
            {
                if (moduleInDepartment.IsCollectionValid())
                {
                    moduleInDepartment.Each(x =>
                    {
                        var moduleInDept = uow.GetRepository<ModuleInDepartment>().Items.Where(y => y.ModuleId == x.ModuleId).ToList();
                        if (!moduleInDept.IsCollectionValid())
                            uow.GetRepository<ModuleInDepartment>().Insert(x);
                    });
                    uow.SaveChanges();
                }
            }
        }
        public void AddPermission(PermissionSet permissionSet)
        {
            using (var uow = new UnitOfWork())
            {
                if (permissionSet.PermissionInSets.IsCollectionValid())
                {
                    foreach (var permissionInSet in permissionSet.PermissionInSets)
                    {
                        uow.GetRepository<PermissionInSet>().Items.ToList().Add(permissionInSet);
                    }
                }
                uow.GetRepository<PermissionSet>().Insert(permissionSet);
                uow.SaveChanges();
            }
        }

        public List<PermissionSet> GetAllPermissionSet(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var depts = uow.GetRepository<DepartmentInOrganization>().Items.Where(x => x.OrganizationId == organizationId).ToList();
                List<PermissionSet> permissionSets = new List<PermissionSet>();
                if (depts.IsCollectionValid())
                {
                    depts.ForEach(x =>
                    {
                        var permSets = uow.GetRepository<PermissionSet>().Items.Include(s => s.PermissionInSets).FirstOrDefault(y => y.DepartmentId == x.DepartmentId);
                        if (permSets.IsNotNull())
                            permissionSets.Add(permSets);
                    });
                }
                return permissionSets;
            }
        }

        public List<RoleInPermission> GetAllRoleInPermission()
        {
            using (var uow = new UnitOfWork())
            {
                var roleInPerm = uow.GetRepository<RoleInPermission>().Items.Include(x => x.Role).ToList();
                return roleInPerm;
            }
        }

        public void AddRoleInPermission(RoleInPermission roleInPermission)
        {
            using (var uow = new UnitOfWork())
            {
                uow.GetRepository<RoleInPermission>().Insert(roleInPermission);
                uow.SaveChanges();
            }
        }
        public List<UserInRole> GetUserInRole(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var userInRoles = uow.GetRepository<UserInRole>().Items.Include(x => x.Role).Where(x => x.Role.OrganizationId == organizationId).ToList();
                return userInRoles;
            }
        }
        public void AddUserInRole(List<UserInRole> userInRoles)
        {
            using (var uow = new UnitOfWork())
            {
                foreach (var usr in userInRoles)
                {
                    var userInRole = uow.GetRepository<UserInRole>().Items.Where(x => x.RoleId == usr.RoleId && x.UserId == usr.UserId).ToList();
                    if (!userInRole.IsCollectionValid())
                        uow.GetRepository<UserInRole>().Insert(usr);
                }
                uow.SaveChanges();
            }
        }

        public void UpdateUserInRole(long id, long roleId)
        {
            using (var uow = new UnitOfWork())
            {
                List<UserInRole> userInRoles = new List<UserInRole>();
                var userInRole = uow.GetRepository<UserInRole>().Items.FirstOrDefault(x => x.UserId == id && roleId == roleId);
                if (userInRole.IsNotNull())
                    return;
                var userInRoleModel = new UserInRole()
                {
                    UserId = id,
                    RoleId = roleId,
                    Active = true
                };
                userInRoles.Add(userInRoleModel);
                AddUserInRole(userInRoles);

            }
        }

        public bool AddUserRoleDepartment(long id, long roleId, long departmentId)
        {
            using (var uow = new UnitOfWork())
            {
                var userRoleDeptFrmDb = uow.GetRepository<UserRoleDepartment>().Items
                    .Where(x => x.UserId == id && x.RoleId == roleId && x.DepartmentId == departmentId).ToList();
                if (!userRoleDeptFrmDb.IsCollectionValid())
                {
                    var userRoleDeptEntity = new UserRoleDepartment()
                    {
                        DepartmentId = departmentId,
                        RoleId = roleId,
                        UserId = id
                    };
                    uow.GetRepository<UserRoleDepartment>().Insert(userRoleDeptEntity);
                    uow.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
