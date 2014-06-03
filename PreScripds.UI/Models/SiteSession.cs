using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;
using PreScripds.Infrastructure;

namespace PreScripds.UI.Models
{
    public class SiteSession
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public int RoleId { get; set; }

        public SiteSession(User user)
        {
            Email = user.Email;
            UserName = user.UserLogins.First().UserName;
            //UserRole = user.UserRole.RoleName;            
        }
        public static void LogOff(HttpSessionStateBase httpSession)
        {
            //
            // Write in the event log the message about the user's Log Off.
            // Note that could be situations that this code was invoked from "Error" page 
            // after the current user session has expired, or before the user to login!
            //
            SiteSession siteSession = (httpSession[Constants.SiteSession] == null ? null : (SiteSession)httpSession[Constants.SiteSession]);

            //
            // Log Off the curent user and clear its site session cache.
            //            
            httpSession[Constants.SiteSession] = null;
        }
    }
}