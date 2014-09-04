using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.BL.Interface
{
    public interface IOrganizationBl
    {
        Organization GetOrganizationById(long organizationId);
        LibraryFolder GetDocLibraryFolder(long organizationId);
        LibraryAsset AddDocLibraryAsset(LibraryAsset libraryAsset);
        LibraryAsset CheckDocExists(string docName);
        List<DepartmentInOrganization> GetDepartmentInOrganization(long organizationId);
        List<ModuleInDepartment> GetModuleInDepartment();
        void AddDepartmentInOrg(List<DepartmentInOrganization> deptInOrg);
        List<Department> GetDepartmentInOrg(long organizationId);
        void AddDepartment(Department department);
        Department GetDepartmentById(long departmentId);
        void AddModule(Module module);
        List<ModuleInDepartment> GetModuleInDepartment(long departmentId);
        Module GetModuleById(long moduleId);
        List<Module> GetAllModule(long departmentId);
        void AddModuleInDepartment(List<ModuleInDepartment> moduleInDepartment);
        void AddPermission(PermissionSet permissionSet);
        List<PermissionSet> GetAllPermissionSet(long organizationId);
        List<RoleInPermission> GetAllRoleInPermission();
        void AddRoleInPermission(RoleInPermission roleInPermission);
        List<UserInRole> GetUserInRole(long organizationId);
        void AddUserInRole(List<UserInRole> userInRole);
        void UpdateUserInRole(long id, long roleId);
        bool AddUserRoleDepartment(long id, long roleId, long departmentId);
        List<Organization> GetOrganizations();
        void UpdateDepartment(long id, bool status);
        void UpdateModule(long id, bool status);
        void UpdatePermissionSet(long id, bool status);
        void UpdateRole(long id, bool status);
        LibraryAsset GetLibraryAsset(long libAssetId);
    }
}
