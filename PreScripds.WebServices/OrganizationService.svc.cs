using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PreScripds.BL;
using PreScripds.BL.Interface;
using PreScripds.DAL;
using PreScripds.Domain;

namespace PreScripds.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OrganizationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OrganizationService.svc or OrganizationService.svc.cs at the Solution Explorer and start debugging.
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationBl _organizationBl;
        private PreScripdsDb _context;
        public OrganizationService(PreScripdsDb context)
        {
            _organizationBl = new OrganizationBl(context);
        }
        public OrganizationService()
        {
            _context = new PreScripdsDb();
            _organizationBl = new OrganizationBl(_context);
        }

        public Organization GetOrganizationById(long organizationId)
        {
            var orgFromDb = _organizationBl.GetOrganizationById(organizationId);
            return orgFromDb;
        }

        public LibraryFolder GetDocLibraryFolder(long organizationId)
        {
            var libFldr = _organizationBl.GetDocLibraryFolder(organizationId);
            return libFldr;
        }

        public LibraryAsset AddDocLibraryAsset(LibraryAsset libraryAsset)
        {
            var libAsst = _organizationBl.AddDocLibraryAsset(libraryAsset);
            return libAsst;
        }

        public LibraryAsset CheckDocExists(string docName)
        {
            var libAsst = _organizationBl.CheckDocExists(docName);
            return libAsst;
        }
        public List<DepartmentInOrganization> GetDepartmentInOrganization(long organizationId)
        {
            var deptInOrg = _organizationBl.GetDepartmentInOrganization(organizationId);
            return deptInOrg;
        }
        public List<ModuleInDepartment> GetAllModuleInDepartment()
        {
            var modInDept = _organizationBl.GetModuleInDepartment();
            return modInDept;
        }

        public void AddDepartmentInOrg(List<DepartmentInOrganization> deptInOrg)
        {
            _organizationBl.AddDepartmentInOrg(deptInOrg);
        }

        public List<Department> GetDepartmentInOrg(long organizationId)
        {
            var depts = _organizationBl.GetDepartmentInOrg(organizationId);
            return depts;
        }

        public void AddDepartment(Department department)
        {
            _organizationBl.AddDepartment(department);
        }

        public Department GetDepartmentById(long departmentId)
        {
            var department = _organizationBl.GetDepartmentById(departmentId);
            return department;
        }

        public void AddModule(Module module)
        {
            _organizationBl.AddModule(module);
        }

        public List<ModuleInDepartment> GetModuleInDepartment(long departmentId)
        {
            var modInDept = _organizationBl.GetModuleInDepartment(departmentId);
            return modInDept;
        }

        public Module GetModuleById(long moduleId)
        {
            var module = _organizationBl.GetModuleById(moduleId);
            return module;
        }

        public List<Module> GetAllModule(long departmentId)
        {
            var modules = _organizationBl.GetAllModule(departmentId);
            return modules;
        }

        public void AddModuleInDepartment(List<ModuleInDepartment> moduleInDepartment)
        {
            _organizationBl.AddModuleInDepartment(moduleInDepartment);
        }

        public void AddPermission(PermissionSet permissionSet)
        {
            _organizationBl.AddPermission(permissionSet);
        }

        public List<PermissionSet> GetAllPermissionSet(long organizationId)
        {
            var permInSets = _organizationBl.GetAllPermissionSet(organizationId);
            return permInSets;
        }

        public List<RoleInPermission> GetAllRoleInPermission()
        {
            return _organizationBl.GetAllRoleInPermission();
        }
        public void AddRoleInPermission(RoleInPermission roleInPermission)
        {
            _organizationBl.AddRoleInPermission(roleInPermission);
        }
        public void AddUserInRole(List<UserInRole> userInRole)
        {
            _organizationBl.AddUserInRole(userInRole);
        }
        public List<UserInRole> GetUserInRole(long organizationId)
        {
            var userInRole = _organizationBl.GetUserInRole(organizationId);
            return userInRole;
        }

        public void UpdateUserInRole(long id, long roleId)
        {
            _organizationBl.UpdateUserInRole(id, roleId);
        }
    }
}
