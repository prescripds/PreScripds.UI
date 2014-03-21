using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PreScripds.Infrastructure.Services;
using PreScripds.UI.Common.Automapper;

namespace PreScripds.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public WcfServiceInvoker _wcfService;
        protected void Application_Start()
        {
            _wcfService = new WcfServiceInvoker();
            Bootstrapper.ConfigureAutoMapper();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
