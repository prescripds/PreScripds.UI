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
        public List<ModuleInDepartment> GetModuleInDepartment()
        {
            var modInDept = _organizationBl.GetModuleInDepartment();
            return modInDept;
        }
    }
}
