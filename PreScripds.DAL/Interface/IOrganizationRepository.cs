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
    }
}
