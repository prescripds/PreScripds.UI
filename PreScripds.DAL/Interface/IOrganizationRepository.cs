using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Interface
{
    public interface IOrganizationRepository
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
    }
}
