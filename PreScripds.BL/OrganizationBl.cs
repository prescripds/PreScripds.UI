using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.BL.Interface;
using PreScripds.DAL;
using PreScripds.DAL.Interface;
using PreScripds.DAL.Repository;
using PreScripds.Domain;

namespace PreScripds.BL
{
    public class OrganizationBl : IOrganizationBl
    {
        private IOrganizationRepository _organizationRepository;
        //private PreScripdsDb context;
        public OrganizationBl(PreScripdsDb context)
        {
            _organizationRepository = new OrganizationRepository(context);
        }

        public Organization GetOrganizationById(long organizationId)
        {
            var orgFromDb = _organizationRepository.GetOrganizationById(organizationId);
            return orgFromDb;
        }
        public LibraryFolder GetDocLibraryFolder(long organizationId)
        {
            var libFldr = _organizationRepository.GetDocLibraryFolder(organizationId);
            return libFldr;
        }

        public LibraryAsset AddDocLibraryAsset(LibraryAsset libraryAsset)
        {
            var libAsst = _organizationRepository.AddDocLibraryAsset(libraryAsset);
            return libAsst;
        }

        public LibraryAsset CheckDocExists(string docName)
        {
            var libAsst = _organizationRepository.CheckDocExists(docName);
            return libAsst;
        }

        public List<DepartmentInOrganization> GetDepartmentInOrganization(long organizationId)
        {
            var deptInOrg = _organizationRepository.GetDepartmentInOrganization(organizationId);
            return deptInOrg;
        }
        public List<ModuleInDepartment> GetModuleInDepartment()
        {
            var modInDept = _organizationRepository.GetModuleInDepartment();
            return modInDept;
        }

        public void AddDepartmentInOrg(List<DepartmentInOrganization> deptInOrg)
        {
            _organizationRepository.AddDepartmentInOrg(deptInOrg);
        }

        public List<Department> GetDepartmentInOrg(long organizationId)
        {
            var depts = _organizationRepository.GetDepartmentInOrg(organizationId);
            return depts;
        }

        public void AddDepartment(Department department)
        {
            _organizationRepository.AddDepartment(department);
        }

        public Department GetDepartmentById(long departmentId)
        {
            var department = _organizationRepository.GetDepartmentById(departmentId);
            return department;
        }
        public void AddModule(Module module)
        {
            _organizationRepository.AddModule(module);
        }
        public List<ModuleInDepartment> GetModuleInDepartment(long departmentId)
        {
            var modInDept = _organizationRepository.GetModuleInDepartment(departmentId);
            return modInDept;
        }

        public Module GetModuleById(long moduleId)
        {
            var module = _organizationRepository.GetModuleById(moduleId);
            return module;
        }

        public List<Module> GetAllModule(long departmentId)
        {
            var modules = _organizationRepository.GetAllModule(departmentId);
            return modules;
        }

        public void AddModuleInDepartment(List<ModuleInDepartment> moduleInDepartment)
        {
            _organizationRepository.AddModuleInDepartment(moduleInDepartment);
        }

        public void AddPermission(PermissionSet permissionSet)
        {
            _organizationRepository.AddPermission(permissionSet);
        }

        public List<PermissionSet> GetAllPermissionSet(long organizationId)
        {
            var permInSets = _organizationRepository.GetAllPermissionSet(organizationId);
            return permInSets;
        }

        public List<RoleInPermission> GetAllRoleInPermission()
        {
            return _organizationRepository.GetAllRoleInPermission();
        }

        public void AddRoleInPermission(RoleInPermission roleInPermission)
        {
            _organizationRepository.AddRoleInPermission(roleInPermission);
        }

        public void AddUserInRole(List<UserInRole> userInRole)
        {
            _organizationRepository.AddUserInRole(userInRole);
        }

        public List<UserInRole> GetUserInRole(long organizationId)
        {
            var userInRoles = _organizationRepository.GetUserInRole(organizationId);
            return userInRoles;
        }

        public void UpdateUserInRole(long id, long roleId)
        {
            _organizationRepository.UpdateUserInRole(id, roleId);
        }

        public bool AddUserRoleDepartment(long id, long roleId, long departmentId)
        {
            var userRoleDept = _organizationRepository.AddUserRoleDepartment(id, roleId, departmentId);
            return userRoleDept;
        }
        public List<Organization> GetOrganizations()
        {
            var organizations = _organizationRepository.GetOrganizations();
            return organizations;
        }

        public void UpdateDepartment(long id, bool status)
        {
            _organizationRepository.UpdateDepartment(id, status);
        }
        public void UpdateModule(long id, bool status)
        {
            _organizationRepository.UpdateModule(id, status);
        }
        public void UpdatePermissionSet(long id, bool status)
        {
            _organizationRepository.UpdatePermissionSet(id, status);
        }
        public void UpdateRole(long id, bool status)
        {
            _organizationRepository.UpdateRole(id, status);
        }

    }
}
