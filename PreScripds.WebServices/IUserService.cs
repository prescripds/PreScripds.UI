using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/Users", ResponseFormat = WebMessageFormat.Xml)]
        List<User> GetUsers();
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddUser", ResponseFormat = WebMessageFormat.Xml)]
        User AddUser(User model);

        [OperationContract]
        [WebInvoke(UriTemplate = "/User", ResponseFormat = WebMessageFormat.Xml)]
        User GetUserByUsername(string loginName);
        [OperationContract]
        [WebInvoke(UriTemplate = "/EmailExist", ResponseFormat = WebMessageFormat.Xml)]
        User CheckEmailExists(string email);
        [OperationContract]
        [WebInvoke(UriTemplate = "/EmailExist", ResponseFormat = WebMessageFormat.Xml)]
        bool CheckRoleExists(Role role);
        [OperationContract]
        [WebInvoke(UriTemplate = "/Role", ResponseFormat = WebMessageFormat.Xml)]
        List<Role> GetRole(long organizationId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/AddRole", ResponseFormat = WebMessageFormat.Xml)]
        Role AddRole(Role model);
    }
}
