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
    [PreScripds.UI.Common.Authorize]
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
        [PreScripds.UI.Common.Authorize]
        public ActionResult Index()
        {
            var user = SessionContext.CurrentUser;
            if (user != null)
            {
                if (user.IsOrganization.HasValue && !user.IsOrganization.Value)
                    return View("Selfie", "Dashboard");
                if (user.IsOrganization.HasValue && user.IsOrganization.Value)
                {
                    if (user.IsSuperAdmin)
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
                    if (user.IsAdmin)
                    {
                        return View("Approvals", "Dashboard");
                    }
                    return RedirectToAction("Organization", "Dashboard");
                }
            }
            else
            {
                return CheckSessionContext();
            }
            return null;
        }
        [PreScripds.UI.Common.Authorize]
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
        [PreScripds.UI.Common.Authorize]
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
        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult Organization()
        {
            var organizationViewModel = new OrganizationViewModel();
            return View(organizationViewModel);
        }
        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult Organization(OrganizationViewModel orgViewModel)
        {
            if (orgViewModel.OrganizationType != 0)
                ValidateViewModel(orgViewModel, orgViewModel.OrganizationType);
            else
                ModelState.AddModelError("OrganizationType", "Please select the organization type.");
            if (ModelState.IsValid)
            {
                if (orgViewModel.IsQuickView)
                {
                    orgViewModel.IsQuickViewTime = DateTime.Now;
                    orgViewModel.QuickViewEndTime = DateTime.Now.AddMinutes(15);

                }
                var mappedModel = Mapper.Map<OrganizationViewModel, Organization>(orgViewModel);
                mappedModel.CreatedDate = DateTime.Now;
                var organizationModel = _wcfService.InvokeService<IUserService, Organization>((svc) => svc.AddOrganization(mappedModel));
                if (organizationModel != null)
                {
                    orgViewModel.CreationSuccessful = true;
                    orgViewModel.Message = "{0} saved successfully.Your account will be activated for use after the approval process is completed by us.";
                }
                else
                {
                    ModelState.AddModelError("", "There was an error while saving. Please try again.");
                }
            }
            return View(orgViewModel);
        }

        private void ValidateViewModel(OrganizationViewModel orgViewModel, int orgType)
        {
            if (orgType == (int)OrganizationType.New)
            {
                if (!orgViewModel.OrganizationName.Clean().IsNotEmpty())
                    ModelState.AddModelError("OrganizationName", "Organization Name is mandatory.");
                if (!orgViewModel.OrganizationAddress.Clean().IsNotEmpty())
                    ModelState.AddModelError("OrganizationAddress", "Organization Address is mandatory.");
                if (!orgViewModel.OrganizationMobile.HasValue)
                    ModelState.AddModelError("OrganizationMobile", "Organization Mobile is mandatory.");
                if (!orgViewModel.OrganizationEmail.Clean().IsNotEmpty())
                    ModelState.AddModelError("OrganizationEmail", "Organization Email is mandatory.");
                if (!orgViewModel.OrganizationContact.Clean().IsNotEmpty())
                    ModelState.AddModelError("OrganizationContact", "Organization Contact is mandatory.");
                if (!orgViewModel.VerificationDate.HasValue)
                    ModelState.AddModelError("VerificationDate", "Verification Date is mandatory.");
                if (orgViewModel.OrganizationName.Clean().IsNotEmpty())
                {
                    var isExist = CheckOrganizationExists(orgViewModel.OrganizationName.Clean());
                    if (isExist)
                        ModelState.AddModelError("OrganizationName", "Organization name '{0}' is already present.".ToFormat(orgViewModel.OrganizationName));
                }
            }
            if (orgType == (int)OrganizationType.Registered)
            {
                if (!orgViewModel.ReferencedEmail.Clean().IsNotEmpty())
                    ModelState.AddModelError("ReferencedEmail", "Referenced Email is mandatory.");
                if (!orgViewModel.DepartmentId.HasValue)
                    ModelState.AddModelError("DepartmentId", "Please select a department.");
                if (!orgViewModel.EmployeeIdOrg.Clean().IsNotEmpty())
                    ModelState.AddModelError("EmployeeIdOrg", "Employee Id is mandatory.");
                if (orgViewModel.ReferencedEmail.Clean().IsNotEmpty())
                {
                    var isPresent = CheckEmailExists(orgViewModel.ReferencedEmail);
                    if (isPresent)
                        ModelState.AddModelError("ReferencedEmail", "The referenced email you entered does not exist.");
                }
            }
        }

        private bool CheckOrganizationExists(string orgName)
        {
            var organization = _wcfService.InvokeService<IUserService, bool>((svc) => svc.CheckOrganizationExist(orgName));
            return organization;
        }

        private bool CheckEmailExists(string referencedEmail)
        {
            var user = _wcfService.InvokeService<IUserService, User>((svc) => svc.CheckEmailExists(referencedEmail));
            if (user != null && user.IsSuperAdmin)
                return true;
            return false;
        }
    }
}