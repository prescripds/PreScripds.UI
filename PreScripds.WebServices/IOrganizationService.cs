using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Services;
using PreScripds.Domain;

namespace PreScripds.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOrganizationService" in both code and config file together.
    [ServiceContract]
    public interface IOrganizationService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetOrganizationById", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        Organization GetOrganizationById(long organizationId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDocumentFolder", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        LibraryFolder GetDocLibraryFolder(long organizationId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/AddDocLibraryAsset", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        LibraryAsset AddDocLibraryAsset(LibraryAsset libraryAsset);

        [OperationContract]
        [WebInvoke(UriTemplate = "/CheckDocExists", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        LibraryAsset CheckDocExists(string docName);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDepartmentInOrganization", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        List<DepartmentInOrganization> GetDepartmentInOrganization(long organizationId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetAllModuleInDepartment", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        List<ModuleInDepartment> GetAllModuleInDepartment();
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddDepartmentInOrg", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void AddDepartmentInOrg(List<DepartmentInOrganization> deptInOrg);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDepartmentInOrg", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        List<Department> GetDepartmentInOrg(long organizationId);
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddDepartment", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void AddDepartment(Department department);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDepartmentById", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        Department GetDepartmentById(long departmentId);
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddModule", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void AddModule(Module module);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetModuleInDepartment", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        List<ModuleInDepartment> GetModuleInDepartment(long departmentId);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetModuleById", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        Module GetModuleById(long moduleId);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetAllModule", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        List<Module> GetAllModule(long departmentId);
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddModuleInDepartment", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void AddModuleInDepartment(List<ModuleInDepartment> moduleInDepartment);
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddPermission", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void AddPermission(PermissionSet permissionSet);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetAllPermissionSet", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        List<PermissionSet> GetAllPermissionSet(long organizationId);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetAllRoleInPermission", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        List<RoleInPermission> GetAllRoleInPermission();
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddRoleInPermission", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void AddRoleInPermission(RoleInPermission roleInPermission);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetUserInRole", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        List<UserInRole> GetUserInRole(long organizationId);
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddUserInRole", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void AddUserInRole(List<UserInRole> userInRole);
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateUserInRole", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void UpdateUserInRole(long id, long roleId);
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddUserRoleDepartment", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        bool AddUserRoleDepartment(long id, long roleId, long departmentId);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetOrganizations", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        List<Organization> GetOrganizations();
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateDepartment", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void UpdateDepartment(long id, bool status);
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateModule", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void UpdateModule(long id, bool status);
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdatePermissionSet", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void UpdatePermissionSet(long id, bool status);
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateRole", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
        void UpdateRole(long id, bool status);
    }
}
