﻿using System;
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
using System.Configuration;
using Recaptcha;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public string GetUserDetails(string userName)
        {
            string isCaptchaDisplay = null;
            var loginType = CheckInputType(userName);
            var loginViewModel = new LoginViewModel();
            var user = _wcfService.InvokeService<IUserService, User>(svc => svc.GetUserByUsername(userName, loginType));
            if (user != null)
            {
                var ipAddress = GetClientIpAddress();
                var userHistory = user.UserLogins.Select(x => x.UserHistories.FirstOrDefault(y => y.IpAddress == ipAddress)).ToList();
                if (userHistory == null)
                {
                    isCaptchaDisplay = "False";
                    loginViewModel.IsCaptchaDisplay = isCaptchaDisplay.AsBool();
                    return isCaptchaDisplay;
                }
                else
                {
                    isCaptchaDisplay = "True";
                    loginViewModel.IsCaptchaDisplay = isCaptchaDisplay.AsBool();
                    return isCaptchaDisplay;
                }
            }
            //ModelState.AddModelError("UserName", "The user name is not correct.");
            return isCaptchaDisplay;
        }

        private LoginType CheckInputType(string userName)
        {
            string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                                    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                                    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                                    + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
            var isEmail = Regex.IsMatch(userName, MatchEmailPattern);
            if (isEmail)
                return LoginType.IsEmail;
            var mobilePatternMatch = @"^[0-9]+$";
            var isMobile = Regex.IsMatch(userName, mobilePatternMatch);
            if (isMobile)
                return LoginType.IsMobile;
            return LoginType.IsUserName;
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [RecaptchaControlMvc.CaptchaValidator]
        public ActionResult Login(LoginViewModel model, string returnUrl, bool captchaValid, string captchaErrorMessage)
        {
            if (ModelState.IsValid)
            {
                User user = new Domain.User();
                var loginType = CheckInputType(model.UserName);
                user = _wcfService.InvokeService<IUserService, User>(svc => svc.GetUserByUsername(model.UserName, loginType));
                if (user != null)
                {
                    var userLogin = user.UserLogins.FirstOrDefault();
                    if (!user.Active)
                        ModelState.AddModelError("", "Your account has been disabled. Please contact your administrator.");
                    var hashedPassword = Common.Common.CreatePasswordHash(model.Password, userLogin.saltkey);
                    if (hashedPassword.Equals(userLogin.Password))
                    {
                        var userHistry = user.UserLogins.Select(x => x.UserHistories.FirstOrDefault(y => y.IpAddress == GetClientIpAddress())).ToList();
                        var hashedPasswordCap = Common.Common.CreatePasswordCapHash(model.Password, userLogin.saltkey, userLogin.Captcha);
                        if (userHistry == null)
                        {
                            //if (!hashedPasswordCap.Equals(userLogin.PasswordCap))
                            //{
                            if (model.IsCaptchaDisplay.HasValue)
                            {
                                if (captchaValid)
                                {
                                    var encryptedCaptcha = EncryptionExtensions.Encrypt(model.CaptchaUserInput);
                                    model.Captcha = encryptedCaptcha;
                                    try
                                    {
                                        var userHistryViewModel = new UserHistoryViewModel()
                                        {
                                            Captcha = model.Captcha,
                                            IpAddress = GetClientIpAddress(),
                                            PasswordCap = hashedPasswordCap,
                                            SaltKey = user.UserLogins.First().saltkey,
                                            UserId = user.Id
                                        };
                                        var mappedModel = Mapper.Map<UserHistoryViewModel, UserHistory>(userHistryViewModel);
                                        var userHistory = _wcfService.InvokeService<IUserService, UserHistory>((svc) => svc.AddUserHistory(mappedModel));
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }
                            //}
                        }
                        else
                        {
                            var passwordCapFromDb = userHistry;
                            if (!passwordCapFromDb.Equals(hashedPasswordCap))
                            {
                                _wcfService.InvokeService<IUserService>((svc) => svc.UpdateUserLogin(userHistry.FirstOrDefault()));
                            }
                            else
                            {
                                SessionContext.SuspiciousErrorMessage = "NOTE: Please change your password immediately.";
                            }
                        }

                        AuthenticateUser(user, userLogin);

                        if (returnUrl != null)
                        {
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            if (user.OrganizationId == 0)
                                return RedirectToAction("Organization", "Dashboard");
                            else
                            {
                                if (user.OrganizationId.HasValue)
                                {
                                    var organization = _wcfService.InvokeService<IOrganizationService, Organization>((svc) => svc.GetOrganizationById(user.OrganizationId.Value));
                                    var docFolder = organization.LibraryFolders.FirstOrDefault(x => x.FolderName == "Documents");
                                    if (!docFolder.LibraryAssets.IsCollectionValid())
                                    {
                                        return RedirectToAction("OrganizationDocs", "Dashboard");
                                    }
                                    else if (!organization.Roles.IsCollectionValid())
                                    {
                                        return RedirectToAction("AddRole", "Dashboard");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Dashboard");
                                    }
                                }
                            }
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
        public ActionResult Profile(string ps = null)
        {
            var registerViewModel = new RegisterViewModel();
            BindDropDowns(registerViewModel);
            if (SessionContext.CurrentUser.IsNotNull())
            {
                var userId = SessionContext.CurrentUser.Id;
                var userProfileFromDb = _wcfService.InvokeService<IUserService, User>((svc) => svc.GetUserById(userId));
                var mappedProfile = Mapper.Map<User, RegisterViewModel>(userProfileFromDb);
                mappedProfile.Countries = registerViewModel.Countries;
                mappedProfile.States = registerViewModel.States;
                mappedProfile.userLoginViewModel.FirstOrDefault().SecurityQuestions = registerViewModel.userLoginViewModel.FirstOrDefault().SecurityQuestions;
                //mappedProfile.userLoginViewModel.FirstOrDefault().Password = registerViewModel.userLoginViewModel.FirstOrDefault().Password;
                //mappedProfile.userLoginViewModel.FirstOrDefault().ConfirmPassword = registerViewModel.userLoginViewModel.FirstOrDefault().Password;
                mappedProfile.SecurityQuestionId = mappedProfile.userLoginViewModel.FirstOrDefault().SecurityQuestionId;

                mappedProfile.IsUserProfile = true;
                return View(mappedProfile);
            }
            else
            {
                registerViewModel.CountryId = 1;
                registerViewModel.IsUserProfile = false;
                //registerViewModel.userLoginViewModel = new List<UserLoginViewModel>();

                if (ps.IsNotEmpty())
                {
                    registerViewModel.IsHomeUrl = true;
                }
                else
                {
                    registerViewModel.IsHomeUrl = false;
                }
            }
            return View(registerViewModel);
        }

        private void BindDropDowns(RegisterViewModel registerViewModel)
        {
            var countries = _wcfService.InvokeService<IMasterService, List<Country>>(svc => svc.GetCountry());
            var states = _wcfService.InvokeService<IMasterService, List<State>>(svc => svc.GetState());
            var securityQuestions = _wcfService.InvokeService<IMasterService, List<SecurityQuestion>>(svc => svc.GetSecurityQuestion());
            if (countries.IsCollectionValid())
                registerViewModel.Countries = countries;
            if (states.IsCollectionValid())
                registerViewModel.States = states;
            if (securityQuestions.IsCollectionValid())
            {
                registerViewModel.userLoginViewModel = new List<UserLoginViewModel>(){
                    new UserLoginViewModel{
                        SecurityQuestions = securityQuestions
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
        public async Task<ActionResult> Profile(RegisterViewModel model, bool captchaValid, string captchaErrorMessage)
        {
            if (!model.IsUserProfile)
            {
                VerifyCaptcha(model, captchaValid, captchaErrorMessage);
                ValidateUserLoginDetails(model.userLoginViewModel);
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

                        mappedUserProfile.UserLogins.FirstOrDefault().SecurityQuestionId = model.SecurityQuestionId;
                        if (mappedUserProfile.Id == 0)
                        {
                            mappedUserProfile.CreatedDate = DateTime.Now;
                            mappedUserProfile.CreatedBy = 0;
                            mappedUserProfile.UpdatedDate = DateTime.Now;
                            mappedUserProfile.UpdatedBy = 0;
                            mappedUserProfile.Active = true;
                            var userFromDb = _wcfService.InvokeService<IUserService, User>(svc => svc.AddUser(mappedUserProfile));
                            if (userFromDb != null)
                            {
                                model.CreationSuccessful = true;
                                model.Message = "Dear '{0}'. You have been registered successfully and a welcome email has been sent to '{1}' and a welcome sms is sent to '{2}'.".ToFormat(model.FullName, model.Email, model.Mobile);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("TermsCondition", "Terms and Conditions is required.");
                    }
                    model.userLoginViewModel = new List<UserLoginViewModel>();
                    model.IsUserProfile = false;
                    BindDropDowns(model);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var userFromDb = _wcfService.InvokeService<IUserService, User>((svc) => svc.GetUserById(model.Id));
                    model.userLoginViewModel.ForEach(x =>
                    {
                        x.Password = userFromDb.UserLogins.FirstOrDefault().Password;
                        x.SecurityAnswer = userFromDb.UserLogins.FirstOrDefault().SecurityAnswer;
                        x.Captcha = userFromDb.UserLogins.FirstOrDefault().Captcha;
                        x.UserHistoryViewModel = new List<UserHistoryViewModel>();
                        var userHstry = userFromDb.UserLogins.Select(z => z.UserHistories);
                        foreach (var item in userFromDb.UserLogins.FirstOrDefault().UserHistories)
                        {
                            UserHistoryViewModel userHistoryViewModel = new UserHistoryViewModel()
                            {
                                UserId = item.UserloginId,
                                UserHistoryId = item.Id,
                                SaltKey = item.saltkey,
                                PasswordCap = item.PasswordCap,
                                IpAddress = item.IpAddress,
                                CreatedDate = item.CreatedDate,
                                Captcha = item.Captcha
                            };
                            x.UserHistoryViewModel.Add(userHistoryViewModel);
                        }
                    });

                    var mappedUserProfile = Mapper.Map<RegisterViewModel, User>(model);

                    mappedUserProfile.UserLogins.FirstOrDefault().SecurityQuestionId = model.SecurityQuestionId;
                    mappedUserProfile.UserLogins.FirstOrDefault().PasswordCap = userFromDb.UserLogins.FirstOrDefault().PasswordCap;
                    mappedUserProfile.UserLogins.FirstOrDefault().saltkey = userFromDb.UserLogins.FirstOrDefault().saltkey;
                    mappedUserProfile.UserLogins.FirstOrDefault().Id = userFromDb.UserLogins.FirstOrDefault().Id;
                    mappedUserProfile.UpdatedDate = DateTime.Now;
                    mappedUserProfile.UpdatedBy = SessionContext.CurrentUser.Id;
                    var userProfile = _wcfService.InvokeService<IUserService, User>((svc) => svc.UpdateUserProfile(mappedUserProfile));
                    if (userProfile.IsNotNull())
                    {
                        var registerMappedProfile = Mapper.Map<User, RegisterViewModel>(userProfile);
                        registerMappedProfile.Message = "Saved Successfully.";
                        registerMappedProfile.CreationSuccessful = true;
                    }

                }
            }


            return View(model);
        }

        private void ValidateUserLoginDetails(List<UserLoginViewModel> userLoginViewModel)
        {
            if (userLoginViewModel.IsCollectionValid())
            {
                userLoginViewModel.ForEach(x =>
                {
                    if (x.Password.IsNull() || x.Password.IsEmpty())
                        ModelState.AddModelError("Password", "Password is mandatory.");
                    if (x.SecurityAnswer.IsNull() || x.SecurityAnswer.IsEmpty())
                        ModelState.AddModelError("SecurityAnswer", "Security Answer is mandatory.");
                });
            }
        }

        public void VerifyCaptcha(RegisterViewModel model, bool captchaValid, string captchaErrorMessage)
        {
            if (!captchaValid)
            {
                model.CaptchaUserInput = string.Empty;
                ModelState.AddModelError("recaptcha", captchaErrorMessage);
            }
            else
            {
                var userHistoryLst = new List<UserHistoryViewModel>();
                var userHistory = new UserHistoryViewModel();
                model.CaptchaValid = model.CaptchaUserInput;
                userHistory.Captcha = model.CaptchaValid;
                userHistory.CreatedDate = DateTime.Now;
                var ipAddress = GetClientIpAddress();
                model.IpAddress = userHistory.IpAddress = ipAddress;
                userHistoryLst.Add(userHistory);
                model.userLoginViewModel.FirstOrDefault().Captcha = model.CaptchaUserInput;
                model.userLoginViewModel.FirstOrDefault().UserHistoryViewModel = model.UserHistoryViewModel = userHistoryLst;
            }
        }

        private string GetClientIpAddress()
        {
            var ipAddress = string.Empty;
            var dnsHostName = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(dnsHostName);
            foreach (var ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipAddress = ip.ToString();
                }
            }
            return ipAddress;
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

        public ActionResult UserProfile()
        {
            if (SessionContext.CurrentUser.IsNotNull())
            {
                return RedirectToAction("Profile", "Account");
            }
            return RedirectToAction("Login", "Account");
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
            SessionContext.CurrentUser = null;
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel()
            {
                RecoveryModeViewModels = new List<RecoveryModeViewModel>()
            };
            PopulateRecoveryModes(forgotPasswordViewModel);
            return View(forgotPasswordViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [RecaptchaControlMvc.CaptchaValidator]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgotPaswordVm, string buttonType, bool captchaValid, string captchaErrorMessage)
        {
            PopulateRecoveryModes(forgotPaswordVm);
            var buttonValue = buttonType.Replace(" ", "");
            if (buttonValue == RecoveryMode.SendViaEmail.ToString())
            {
                if (forgotPaswordVm.UserInput.IsEmpty() || forgotPaswordVm.UserInput.IsNull())
                    ModelState.AddModelError("", "Please enter your email address.");
                else
                {
                    //TODO:Send mail with new password.
                    return View("Login", "Account");
                }
            }
            if (buttonValue == RecoveryMode.SendViaSMS.ToString())
            {
                if (forgotPaswordVm.UserInput.IsEmpty() || forgotPaswordVm.UserInput.IsNull())
                    ModelState.AddModelError("", "Please enter your mobile number.");
                else
                {
                    //TODO:Send verification code to mobile.
                    return View("Login", "Account");
                }
            }
            if (buttonValue == RecoveryMode.AnswerSecurityQuestion.ToString())
            {
                if (captchaValid)
                {
                    if (forgotPaswordVm.UserInput.IsEmpty() || forgotPaswordVm.UserInput.IsNull())
                        ModelState.AddModelError("", "Please enter your mobile number.");
                    else
                    {
                        //TODO:Send verification code to mobile.
                        return View("Login", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please fill the captcha.");
                }
            }
            return View(forgotPaswordVm);
        }


        private void PopulateRecoveryModes(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            forgotPasswordViewModel.RecoveryModeViewModels = new List<RecoveryModeViewModel>();
            if (!forgotPasswordViewModel.RecoveryModeViewModels.IsCollectionValid())
            {
                string[] recoveryModes = new string[]
                {
                    "Send Via Email",
                    "Send Via SMS",
                    "Answer Security Question",
                    "Send Via Alternate Email",
                    "Send Via Alternate Mobile SMS"
                };
                for (int i = 0; i < 5; i++)
                {
                    var recoveryViewModel = new RecoveryModeViewModel()
                    {
                        Id = i + 1,
                        Name = recoveryModes[i]
                    };
                    forgotPasswordViewModel.RecoveryModeViewModels.Add(recoveryViewModel);
                }

            }
        }
        [HttpPost]
        public bool CheckUserEmailExists(string email)
        {
            var userExists = CheckUserEmail(email);
            return userExists;
        }

        [HttpPost]
        public string GetSecurityQuestion(string email)
        {
            var user = _wcfService.InvokeService<IUserService, User>((svc) => svc.GetUserByUsername(email, LoginType.IsEmail));
            var securityQuestions = _wcfService.InvokeService<IMasterService, List<SecurityQuestion>>((svc) => svc.GetSecurityQuestion());

            if (user.IsNotNull())
            {
                long securityQuestionId = 0;
                var userLogin = user.UserLogins.ToList();
                userLogin.ForEach(x =>
                {
                    securityQuestionId = x.SecurityQuestionId.Value;
                });
                if (securityQuestionId != 0)
                {
                    if (securityQuestions.IsCollectionValid())
                    {
                        var secQuestName = securityQuestions.FirstOrDefault(x => x.Id == securityQuestionId).QuestionName;
                        return secQuestName;
                    }
                }
            }
            return "";

        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId)
        {
            var id = EncryptionExtensions.Decrypt(userId).As<long>();
            var manageUserViewModel = new ManageUserViewModel()
            {
                UserId = id
            };
            return View(manageUserViewModel);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ManageUserViewModel manageUserViewModel)
        {
            if (ModelState.IsValid)
            {
                return View("Login", "Account");
            }
            return View();
        }

        [HttpGet]
        [PreScripds.UI.Common.Authorize]
        public ActionResult ChangePassword()
        {
            return View("Manage");
        }

        [HttpPost]
        [PreScripds.UI.Common.Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ManageUserViewModel manageUserViewModel)
        {
            manageUserViewModel.UserId = SessionContext.CurrentUser.Id;
            if (ModelState.IsValid)
            {
                var userLogin = _wcfService.InvokeService<IUserService, UserLogin>((svc) => svc.GetUserLoginById(manageUserViewModel.UserId.Value));
                userLogin.Password = manageUserViewModel.NewPassword;
                var passwordChange = _wcfService.InvokeService<IUserService, string>((svc) => svc.ChangePassword(userLogin));
                if (passwordChange.Contains("Successfully saved."))
                    return RedirectToAction("Profile", "Account");
                else
                {
                    ModelState.AddModelError("NewPassword", passwordChange);
                }
            }
            return View("Manage");
        }

        [HttpGet]
        [PreScripds.UI.Common.Authorize]
        public ActionResult ChangeSecurityAnswer()
        {
            return View("SecurityAnswer");
        }

        [HttpPost]
        [PreScripds.UI.Common.Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSecurityAnswer(ManageUserSecurityViewModel manageUserSecurityViewModel)
        {
            manageUserSecurityViewModel.UserId = SessionContext.CurrentUser.Id;
            if (ModelState.IsValid)
            {
                var userLogin = _wcfService.InvokeService<IUserService, UserLogin>((svc) => svc.GetUserLoginById(manageUserSecurityViewModel.UserId.Value));
                userLogin.SecurityAnswer = manageUserSecurityViewModel.NewSecurityAnswer;
                var securityAnswerChange = _wcfService.InvokeService<IUserService, string>((svc) => svc.ChangeSecurityAnswer(userLogin));
                if (securityAnswerChange.Contains("Successfully Saved"))
                    return RedirectToAction("Profile", "Account");
                else
                {
                    ModelState.AddModelError("NewSecurityAnswer", securityAnswerChange);
                }
            }
            return View("SecurityAnswer");
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