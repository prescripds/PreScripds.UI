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
                var organization = uow.GetRepository<Organization>().Items.Include(s => s.LibraryFolders.Select(y => y.LibraryAssets)).FirstOrDefault(x => x.Id == organizationId);
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
                        permissionSets.Add(permSets);
                    });
                }
                return permissionSets;
            }
        }
    }
}
