using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PreScripds.Domain;
using PreScripds.Domain.Master;

namespace PreScripds.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMasterService" in both code and config file together.
    [ServiceContract]
    public interface IMasterService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/Permissions", ResponseFormat = WebMessageFormat.Xml)]
        List<Permission> GetPermission();

        [OperationContract]
        [WebGet(UriTemplate = "/Countries", ResponseFormat = WebMessageFormat.Xml)]
        List<Country> GetCountry();

        [OperationContract]
        [WebGet(UriTemplate = "/States", ResponseFormat = WebMessageFormat.Xml)]
        List<State> GetState();

        [OperationContract]
        [WebGet(UriTemplate = "/Department", ResponseFormat = WebMessageFormat.Xml)]
        List<Department> GetDepartment();

        [OperationContract]
        [WebGet(UriTemplate = "/SecurityQuestion", ResponseFormat = WebMessageFormat.Xml)]
        List<SecurityQuestion> GetSecurityQuestion();
        [OperationContract]
        [WebGet(UriTemplate = "/Module", ResponseFormat = WebMessageFormat.Xml)]
        List<Module> GetModule();
    }
}
