﻿using System;
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
                if (user.OrganizationId == 0)
                    return View("Selfie", "Dashboard");
                if (user.OrganizationId == 1)
                {
                    if (user.IsSuperAdmin == 1)
                    {
                        long organizationId = 0;
                        if (user.OrganizationId.HasValue)
                        {
                            organizationId = user.OrganizationId.Value;
                        }
                        //TODO: Get organization details 
                        var role = _wcfService.InvokeService<IUserService, List<Role>>(svc => svc.GetRole(organizationId));
                        if (role == null)
                            return View("AddRole", "Dashboard");
                        return View("Approvals", "Dashboard");
                    }

                    if (user.IsAdmin == 1)
                        return View("Approvals", "Dashboard");
                    return View("Organization", "Dashboard");
                }
            }
            else
            {
                return CheckSessionContext();
            }
            return null;
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
            if (ModelState.IsValid)
            {
                var checkRoleNameExists = CheckRoleNameExists(roleViewModel);
                if (checkRoleNameExists)
                {
                    ModelState.AddModelError("RoleName", "Role Name already exists for your organization.");
                }
                else
                {
                    roleViewModel.OrganizationId = SessionContext.CurrentUser.OrganizationId.Value;
                    var mappedRoleModel = Mapper.Map<RoleViewModel, Role>(roleViewModel);
                    var roleModel = _wcfService.InvokeService<IUserService, PreScripds.Domain.Role>((svc) => svc.AddRole(mappedRoleModel));
                    if (roleModel.RoleId != 0)
                    {
                        roleViewModel.CreationSuccessful = true;
                        roleViewModel.Message = "Role {0} has been created successfully.".ToFormat(roleViewModel.RoleName);
                    }
                    else
                    {
                        ModelState.AddModelError("", "There was an error while saving your changes. Please re-enter the details.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("RoleName", "Role Name is mandatory.");
            }
            return View();

        }

        private bool CheckRoleNameExists(RoleViewModel roleViewModel)
        {
            var mappedRoleModel = Mapper.Map<RoleViewModel, Role>(roleViewModel);
            var roleCheck = _wcfService.InvokeService<IUserService, bool>((svc) => svc.CheckRoleExists(mappedRoleModel));
            return roleCheck;
        }
    }
}