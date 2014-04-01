using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;
using PreScripds.Infrastructure;

namespace PreScripds.UI.Common
{
    public class SessionContext
    {
        public void SetUpSessionContext(HttpContextBase httpContext, User requestContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");
            if (requestContext == null) throw new ArgumentNullException("requestContext");

            if (httpContext.Session != null)
            {
                httpContext.Session[Constants.SiteSession] = requestContext;
            }
        }

        public static User CurrentUser { get; set; }

        public static void LogOff(HttpContextBase httpSession)
        {
            httpSession.Session[Constants.SiteSession] = null;
        }
    }
}