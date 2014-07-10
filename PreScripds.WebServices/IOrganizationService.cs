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
    }
}
