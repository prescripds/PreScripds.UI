using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using PreScripds.UI.Models;
using PreScripds.Infrastructure;
using PreScripds.Infrastructure.Services;
using PreScripds.WebServices;
using PreScripds.Domain.Master;
using AutoMapper;
using PreScripds.Domain;
using System.Data.Entity.Infrastructure;
using PreScripds.UI.Common;
using System.Web.Security;
using System.Security.Principal;
using PreScripds.Domain.Enums;
//using System.Web.Helpers;
//using Microsoft.Web.Helpers;
using System.Configuration;
using Recaptcha;
//using Recaptcha.Web.Mvc;
//using Recaptcha.Web;

namespace PreScripds.UI.Controllers
{

    public class AccountController : BaseController
    {
        private WcfServiceInvoker _wcfService;
        private SessionContext _sessionContext;
        public AccountController()
        {
            _wcfService = new WcfServiceInvoker();
            _sessionContext = new SessionContext();
        }

        //public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _wcfService.InvokeService<IUserService, User>(svc => svc.GetUserByUsername(model.UserName));

                if (user != null)
                {
                    var userLogin = user.UserLogin.FirstOrDefault();
                    if (!user.Active)
                        ModelState.AddModelError("", "Please enter a valid Username/Password");
                    var hashedPassword = Common.Common.CreatePasswordHash(model.Password, userLogin.SaltKey);
                    if (hashedPassword.Equals(userLogin.Password))
                    {
                        AuthenticateUser(user, userLogin);
                        if (returnUrl != null)
                        {
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please enter a valid Username/Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please enter a valid Username/Password");
                }
                //var user = await UserManager.FindAsync(model.UserName, model.Password);
                //if (user != null)
                //{

                //    await SignInAsync(user, false);
                //    return RedirectToLocal(returnUrl);
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Invalid username or password.");
                //}
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AuthenticateUser(Domain.User user, UserLogin userLogin)
        {
            SessionContext.CurrentUser = user;
            FormsAuthentication.SetAuthCookie(userLogin.UserName, false);
            var ticket = new FormsAuthenticationTicket(1, userLogin.UserName, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(System.Web.HttpContext.Current.Session.Timeout),
                false, user.ToString(), FormsAuthentication.FormsCookiePath);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { HttpOnly = true };
            authCookie.Expires = ticket.Expiration;
            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
            _sessionContext.SetUpSessionContext(HttpContext, SessionContext.CurrentUser);
            SetUserIdentity(SessionContext.LoggedOnUser);
        }

        private void SetUserIdentity(Domain.User user)
        {
            var identity = new GenericIdentity(user.Email);
            GenericPrincipal principal;
            string[] userRole = { UserRoles.SuperAdmin.ToString(), UserRoles.Admin.ToString(), UserRoles.User.ToString() };
            principal = new GenericPrincipal(identity, userRole);
            HttpContext.User = principal;
            SiteSession siteSession = new SiteSession(user);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string ps = null)
        {

            var registerViewModel = new RegisterViewModel()
            {
                CountryId = 1,
                userLoginViewModel = new List<UserLoginViewModel>()
            };
            BindDropDowns(registerViewModel);
            if (ps.IsNotEmpty())
            {
                registerViewModel.IsHomeUrl = true;
                return View(registerViewModel);
            }
            else
            {
                registerViewModel.IsHomeUrl = false;
                return View(registerViewModel);
            }
        }

        private void BindDropDowns(RegisterViewModel registerViewModel)
        {
            var countries = _wcfService.InvokeService<IMasterService, List<Country>>(svc => svc.GetCountry());
            var states = _wcfService.InvokeService<IMasterService, List<State>>(svc => svc.GetState());
            var securityQuestions = _wcfService.InvokeService<IMasterService, List<SecurityQuestion>>(svc => svc.GetSecurityQuestion());
            if (countries.IsCollectionValid())
                registerViewModel.Country = countries;
            if (states.IsCollectionValid())
                registerViewModel.State = states;
            if (securityQuestions.IsCollectionValid())
            {
                registerViewModel.userLoginViewModel = new List<UserLoginViewModel>(){
                    new UserLoginViewModel{
                        SecurityQuestion = securityQuestions
                    }
                };
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [RecaptchaControlMvc.CaptchaValidator]
        public async Task<ActionResult> Register(RegisterViewModel model, bool captchaValid, string captchaErrorMessage)
        {
            VerifyCaptcha(model, captchaValid, captchaErrorMessage);
            if (ModelState.IsValid)
            {
                model.Active = true;
                if (model.TermsCondition)
                {
                    if (model.UserType == (short)UserType.Self)
                        model.IsOrganization = 0;
                    if (model.UserType == (short)UserType.Organization)
                        model.IsOrganization = 1;
                    var mappedUserProfile = Mapper.Map<RegisterViewModel, User>(model);
                    var userFromDb = _wcfService.InvokeService<IUserService, User>(svc => svc.AddUser(mappedUserProfile));
                    if (userFromDb != null)
                    {
                        model.CreationSuccessful = true;
                        model.Message = "Dear {0}. You have been registered successfully and a welcome email has been sent to {1} and a welcome sms is sent to {2}".ToFormat(model.FullName, model.Email, model.Mobile);
                    }
                }
                else
                {
                    ModelState.AddModelError("TermsCondition", "Terms and Conditions is required.");
                }

            }
            return View(new RegisterViewModel { userLoginViewModel = new List<UserLoginViewModel>() });
        }

        public void VerifyCaptcha(RegisterViewModel model, bool captchaValid, string error)
        {
            if (!captchaValid)
            {
                model.CaptchaUserInput = string.Empty;
                ModelState.AddModelError("recaptcha", captchaErrorMessage);
            }
            else
            {
                model.CaptchaValid = model.CaptchaUserInput;
            }
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [AllowAnonymous]
        public bool CheckUserEmail(string username)
        {
            var user = _wcfService.InvokeService<IUserService, User>(svc => svc.CheckEmailExists(username));
            if (user == null)
            {
                return false;
            }
            return true;
        }
        ////
        //// GET: /Account/Manage
        //public ActionResult Manage(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
        //        : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
        //        : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //        : message == ManageMessageId.Error ? "An error has occurred."
        //        : "";
        //    ViewBag.HasLocalPassword = HasPassword();
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    return View();
        //}

        ////
        //// POST: /Account/Manage
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Manage(ManageUserViewModel model)
        //{
        //    bool hasPassword = HasPassword();
        //    ViewBag.HasLocalPassword = hasPassword;
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    if (hasPassword)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
        //            //if (result.Succeeded)
        //            //{
        //            //    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //            //}
        //            //else
        //            //{
        //            //    AddErrors(result);
        //            //}
        //        }
        //    }
        //    else
        //    {
        //        // User does not have a password so remove any validation errors caused by a missing OldPassword field
        //        ModelState state = ModelState["OldPassword"];
        //        if (state != null)
        //        {
        //            state.Errors.Clear();
        //        }

        //        //if (ModelState.IsValid)
        //        //{
        //        //    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
        //        //    if (result.Succeeded)
        //        //    {
        //        //        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //        //    }
        //        //    else
        //        //    {
        //        //        AddErrors(result);
        //        //    }
        //        //}
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // POST: /Account/LogOff

        [HttpGet]
        public ActionResult AccessDenied()
        {
            SignOut();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            SignOut();
            //AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public void SignOut()
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


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //#region Helpers
        //// Used for XSRF protection when adding external logins
        //private const string XsrfKey = "XsrfId";

        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}

        //private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //    var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        //    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        //}

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}

        //private bool HasPassword()
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        return user.PasswordHash != null;
        //    }
        //    return false;
        //}

        //public enum ManageMessageId
        //{
        //    ChangePasswordSuccess,
        //    SetPasswordSuccess,
        //    RemoveLoginSuccess,
        //    Error
        //}

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }

        //private class ChallengeResult : HttpUnauthorizedResult
        //{
        //    public ChallengeResult(string provider, string redirectUri)
        //        : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        LoginProvider = provider;
        //        RedirectUri = redirectUri;
        //        UserId = userId;
        //    }

        //    public string LoginProvider { get; set; }
        //    public string RedirectUri { get; set; }
        //    public string UserId { get; set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
        //        if (UserId != null)
        //        {
        //            properties.Dictionary[XsrfKey] = UserId;
        //        }
        //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //    }
        //}
        //#endregion
    }
}