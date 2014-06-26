﻿using System;
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
    }
}
