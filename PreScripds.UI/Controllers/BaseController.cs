using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PreScripds.UI.Common;
using PreScripds.UI.Models;

namespace PreScripds.UI.Controllers
{
    public class BaseController : Controller
    {
        public ViewResult AppSettings()
        {
            var appSettings = new AppSettingModel()
            {
                AppPath = GetAppPath()
            };
            return View(appSettings);
        }

        public string GetAppPath()
        {
            return Request.ApplicationPath == "/" ? Request.ApplicationPath : Request.ApplicationPath + "/";
        }

        public ActionResult CheckSessionContext(string returnUrl)
        {
            if (SessionContext.CurrentUser == null)
                return RedirectToAction("Login", "Account");
            else if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}