using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PreScripds.Infrastructure.Services;
using PreScripds.UI.Common;
using PreScripds.WebServices;
using PreScripds.Domain;
using PreScripds.Infrastructure;


namespace PreScripds.UI.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private WcfServiceInvoker _wcfService;
        private SessionContext _sessionContext;
        public DashboardController()
        {
            _wcfService = new WcfServiceInvoker();
            _sessionContext = new SessionContext();
        }
        //
        // GET: /Dashboard/
        public ActionResult Index()
        {
            
            //var users = _wcfService.InvokeService<IUserService, List<Domain.User>>((svc) => svc.GetUsers());
            //if (users.IsCollectionValid())
            //{
            //    var user = users.Where(x => x.UserLogin.First().UserName.EqualsIgnoreCase(SessionContext.CurrentUser.UserLogin.First().UserName)).FirstOrDefault();
            //    if (user.IsAdmin == 1)
            //        return View("");
            //}
            return View();
        }
    }
}