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
using System.Threading.Tasks;
using PreScripds.UI.Models;
using AutoMapper;


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
            var user = SessionContext.CurrentUser;
            if (user != null)
            {
                if (user.IsSuperAdmin == 1)
                    return View("AddRole", "Dashboard");
                if (user.IsAdmin == 1)
                    return View("Approvals", "Dashboard");
            }

            return View();
        }

        [HttpGet]
        public ActionResult AddRole()
        {
            var permissions = _wcfService.InvokeService<IMasterService, List<Permission>>((svc) => svc.GetPermission());
            if (!permissions.IsCollectionValid()) permissions = new List<Permission>();
            var roleViewModel = new RoleViewModel()
            {
                Permission = permissions
            };
            return View(roleViewModel);
        }

        [HttpPost]
        public ActionResult AddRole(RoleViewModel roleViewModel)
        {
            return View();
        }
    }
}