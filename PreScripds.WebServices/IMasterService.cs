﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PreScripds.Domain;

namespace PreScripds.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMasterService" in both code and config file together.
    [ServiceContract]
    public interface IMasterService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/Departments", ResponseFormat = WebMessageFormat.Xml)]
        List<Department> GetDepartments();
    }
}