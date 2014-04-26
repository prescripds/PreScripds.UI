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
using PreScripds.Domain.Enums;


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
        [Authorize]
        public ActionResult Index()
        {
            var user = SessionContext.CurrentUser;
            if (user != null)
            {
                if (user.IsOrganization.HasValue && user.IsOrganization.Value == true)
                    return View("Selfie", "Dashboard");
                if (user.IsOrganization.HasValue && user.IsOrganization.Value == true)
                {
                    if (user.IsSuperAdmin == 1)
                    {
                        if (!user.OrganizationId.HasValue)
                        {
                            return RedirectToAction("Organization", "Dashboard");
                        }
                        else
                        {
                            var organizationId = user.OrganizationId.Value;
                            var role = _wcfService.InvokeService<IUserService, List<Role>>(svc => svc.GetRole(organizationId));
                            if (role == null)
                                return RedirectToAction("AddRole", "Dashboard");
                            return View("Approvals", "Dashboard");
                        }
                    }
                    if (user.IsAdmin == 1)
                    {
                        return View("Approvals", "Dashboard");
                    }
                    return View("Organization", "Dashboard");
                }
            }
            else
            {
                return CheckSessionContext();
            }
            return null;
        }
        [Authorize]
        [HttpGet]
        public ActionResult AddRole()
        {
            var permissions = _wcfService.InvokeService<IMasterService, List<Permission>>((svc) => svc.GetPermission());
            var orgId = SessionContext.CurrentUser.OrganizationId.Value;
            var departments = _wcfService.InvokeService<IUserService, List<Department>>((svc) => svc.GetDepartment(orgId));
            if (!departments.IsCollectionValid()) departments = new List<Department>();
            if (!permissions.IsCollectionValid()) permissions = new List<Permission>();
            var roleViewModel = new RoleViewModel()
            {
                Permission = permissions,
                Department = departments
            };
            return View(roleViewModel);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddRole(RoleViewModel roleViewModel)
        {
            roleViewModel.Permission = _wcfService.InvokeService<IMasterService, List<Permission>>((svc) => svc.GetPermission());
            if (ModelState.IsValid)
            {
                if (roleViewModel.SelectedPermission == 0)
                    ModelState.AddModelError("IsPermissionCheckd", "Please choose a permission for the role {0}".ToFormat(roleViewModel.RoleName));

                var checkRoleNameExists = CheckRoleNameExists(roleViewModel);
                if (checkRoleNameExists)
                {
                    ModelState.AddModelError("RoleName", "Role Name already exists for your organization.");
                }
                else
                {
                    roleViewModel.OrganizationId = SessionContext.CurrentUser.OrganizationId.Value;
                    roleViewModel.PermissionId = roleViewModel.SelectedPermission;
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
            return View(roleViewModel);

        }

        private bool CheckRoleNameExists(RoleViewModel roleViewModel)
        {
            var mappedRoleModel = Mapper.Map<RoleViewModel, Role>(roleViewModel);
            var roleCheck = _wcfService.InvokeService<IUserService, bool>((svc) => svc.CheckRoleExists(mappedRoleModel));
            return roleCheck;
        }
        [HttpGet]
        public ActionResult Organization()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Organization(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.OrganizationType == (int)OrganizationType.New)
            {
                ValidateViewModel(registerViewModel);
            }
            else if (registerViewModel.OrganizationType == (int)OrganizationType.Registered)
            {
                ValidateViewModel(registerViewModel);
            }
            else
            {
                ModelState.AddModelError("OrganizationType", "Please select an organization Type.");
            }
            return View(registerViewModel);
        }

        private void ValidateViewModel(RegisterViewModel registerViewModel)
        {
            throw new NotImplementedException();
        }
    }
}