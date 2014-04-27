using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PreScripds.UI.Common;

namespace PreScripds.UI.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        public ActionResult AccessDenied()
        {
            if (SessionContext.CurrentUser == null)
            {
                SignOut();
            }
            return View();
        }

        private void SignOut()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            SessionContext.LogOff(HttpContext);
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            FormsAuthentication.SignOut();
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
        }
    }
}