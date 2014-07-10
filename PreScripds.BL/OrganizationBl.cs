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
    }
}
