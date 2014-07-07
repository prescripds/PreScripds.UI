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
using System.Collections.ObjectModel;
using System.IO;
using System.Configuration;


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
                if (user.UserType == (int)UserType.Self)
                    return View("Selfie", "Dashboard");
                if (user.UserType == (int)UserType.Organization)
                {
                    if (user != null)
                    {
                        if (user.OrganizationId == 0)
                        {
                            return RedirectToAction("Organization", "Dashboard");
                        }
                        else
                        {
                            if (user.OrganizationId.HasValue)
                            {
                                var organizationId = user.OrganizationId.Value;
                                var role = _wcfService.InvokeService<IUserService, List<Role>>(svc => svc.GetRole(organizationId));
                                if (role == null)
                                    return RedirectToAction("AddRole", "Dashboard");
                                var orgInDept = _wcfService.InvokeService<IOrganizationService, List<DepartmentInOrganization>>((svc) => svc.GetDepartmentInOrganization(user.OrganizationId.Value));
                                if (!orgInDept.IsCollectionValid())
                                    return RedirectToAction("DepartmentInOrg", "Dashboard");
                                var modInDept = _wcfService.InvokeService<IOrganizationService, List<ModuleInDepartment>>((svc) => svc.GetModuleInDepartment());
                                if (!modInDept.IsCollectionValid())
                                    return RedirectToAction("ModuleInDepartment", "Dashboard");
                                return View("Approvals", "Dashboard");
                            }
                        }
                    }
                    if (user != null)
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
        public ActionResult AddModule()
        {
            var moduleViewModel = new ModuleViewModel();
            var departmentInOrgId = _wcfService.InvokeService<IOrganizationService, List<DepartmentInOrganization>>((svc) => svc.GetDepartmentInOrganization(SessionContext.CurrentUser.OrganizationId.Value));
            moduleViewModel.DepartmentInOrg = departmentInOrgId;
            return View(moduleViewModel);
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult AddModule(ModuleViewModel moduleViewModel)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }

        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult AddDepartment()
        {
            return View();
        }


        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult DepartmentInOrg()
        {
            var deptInOrgViewModel = new DepartmentInOrgViewModel()
            {
                Department = new List<Department>(),
                DepartmentInOrganizationViewModel = new List<DepartmentInOrganizationViewModel>()
            };
            var departments = _wcfService.InvokeService<IUserService, List<Department>>((svc) => svc.GetAllDepartment());
            if (departments.IsCollectionValid())
                deptInOrgViewModel.Department = departments;
            return View(deptInOrgViewModel);
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult DepartmentInOrg(DepartmentInOrgViewModel deptInOrgViewModel, string buttonType)
        {
            if (ModelState.IsValid)
            {
                if (buttonType == "Add")
                    return RedirectToAction("AddDepartment", "Dashboard");

                deptInOrgViewModel.DepartmentInOrganizationViewModel = new List<DepartmentInOrganizationViewModel>();
                foreach (var department in deptInOrgViewModel.Departments)
                {
                    var deptInOrganization = new DepartmentInOrganizationViewModel();
                    deptInOrganization.DepartmentId = department.As<long>();
                    deptInOrganization.OrganizationId = SessionContext.CurrentUser.OrganizationId.Value;
                    deptInOrganization.Active = true;
                    deptInOrgViewModel.DepartmentInOrganizationViewModel.Add(deptInOrganization);
                }
                if (deptInOrgViewModel.DepartmentInOrganizationViewModel.IsCollectionValid())
                {
                    var mappedModel = Mapper.Map<List<DepartmentInOrganizationViewModel>, List<DepartmentInOrganization>>(deptInOrgViewModel.DepartmentInOrganizationViewModel);
                    _wcfService.InvokeService<IOrganizationService>((svc) => svc.AddDepartmentInOrg(mappedModel));
                    deptInOrgViewModel.CreationSuccessful = true;
                    deptInOrgViewModel.Message = "Thank you for choosing the Departments.";
                }

            }
            return View(deptInOrgViewModel);
        }

        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult AddRole()
        {
            var orgId = SessionContext.CurrentUser.OrganizationId.Value;
            var roles = _wcfService.InvokeService<IUserService, List<Role>>((svc) => svc.GetRole(orgId));
            var mappdRoleViewModel = Mapper.Map<List<Role>, List<RoleViewModel>>(roles);
            var roleViewModel = new RoleViewModel() { RoleViewModels = new List<RoleViewModel>() };
            if (mappdRoleViewModel.IsCollectionValid())
            {
                roleViewModel.RoleViewModels = mappdRoleViewModel;
            }
            return View("AddRole", roleViewModel);
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult AddRole(RoleViewModel roleViewModel, string buttonType)
        {
            ValidateRoleViewModel(roleViewModel);
            if (roleViewModel.RoleViewModels.IsCollectionValid())
            {
                if (buttonType == "Next")
                    return RedirectToAction("DepartmentInOrg", "Dashboard");
            }
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
                    roleViewModel.RoleViewModels = new List<RoleViewModel>();
                    var mappedRoleModel = Mapper.Map<RoleViewModel, Role>(roleViewModel);
                    mappedRoleModel.UpdatedDate = mappedRoleModel.CreatedDate = DateTime.Now;
                    mappedRoleModel.UpdatedBy = mappedRoleModel.CreatedBy = SessionContext.CurrentUser.Id;
                    mappedRoleModel.Active = true;
                    var roleModel = _wcfService.InvokeService<IUserService, PreScripds.Domain.Role>((svc) => svc.AddRole(mappedRoleModel));
                    if (roleModel.Id != 0)
                    {
                        roleViewModel.CreationSuccessful = true;
                        roleViewModel.Message = "Role {0} has been created successfully.".ToFormat(roleViewModel.RoleName);
                        roleViewModel.RoleViewModels.Add(roleViewModel);
                    }
                    else
                    {
                        ModelState.AddModelError("", "There was an error while saving your changes. Please re-enter the details.");
                    }
                }
            }
            return View(roleViewModel);
        }

        private void ValidateRoleViewModel(RoleViewModel roleViewModel)
        {
            if (roleViewModel.RoleName.Trim().IsEmpty())
                ModelState.AddModelError("RoleName", "Role Name is mandatory.");
            if (roleViewModel.RoleDesc.Trim().IsEmpty())
                ModelState.AddModelError("RoleDesc", "Role Description is mandatory.");
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
            var organizationViewModel = new OrganizationViewModel()
            {
                OrganizationDocumentViewModel = new OrganizationDocumentViewModel(),
                Department = GetDepartment(),
                OrganizationDocumentViewModels = new List<OrganizationDocumentViewModel>()
            };
            return View(organizationViewModel);
        }

        private List<Department> GetDepartment()
        {
            var department = _wcfService.InvokeService<IMasterService, List<Department>>((svc) => svc.GetDepartment());
            return department;
        }

        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult AddOrgDoc()
        {

            var organizationViewModel = new OrganizationDocumentViewModel()
            {
                OrganizationDocumentViewModels = new List<OrganizationDocumentViewModel>()
            };
            return View("OrganizationDocs", organizationViewModel);
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult AddOrgDoc(OrganizationDocumentViewModel orgDocViewModel, string buttonType)
        {
            if (buttonType.EqualsIgnoreCase("Next"))
                return RedirectToAction("AddRole", "Dashboard");
            else
            {
                ValidateOrgDocViewModel(orgDocViewModel);
                if (orgDocViewModel != null && ModelState.IsValid)
                {
                    var libraryFolder = _wcfService.InvokeService<IOrganizationService, LibraryFolder>((svc) => svc.GetDocLibraryFolder(SessionContext.CurrentUser.OrganizationId.Value));
                    orgDocViewModel.OrganizationDocumentViewModels = new List<OrganizationDocumentViewModel>();
                    var libAsset = GetLibraryAsset(orgDocViewModel.Document, orgDocViewModel.OrganizationDocumentName, orgDocViewModel.DocumentDescription, libraryFolder.Id);
                    var libraryAssetFromDb = _wcfService.InvokeService<IOrganizationService, LibraryAsset>((svc) => svc.AddDocLibraryAsset(libAsset));
                    orgDocViewModel.ImagePath = SaveImageToDisk(libraryFolder, orgDocViewModel.Document, libraryAssetFromDb.AssetName);
                    orgDocViewModel.CreatedDate = libraryAssetFromDb.CreatedDate;
                    orgDocViewModel.OrganizationDocumentViewModels.Add(orgDocViewModel);
                }
                return View("OrganizationDocs", orgDocViewModel);
            }
        }

        private void ValidateOrgDocViewModel(OrganizationDocumentViewModel orgDocViewModel)
        {
            if (orgDocViewModel.Document == null)
                ModelState.AddModelError("Document", "Please select a document to upload.");
            if (orgDocViewModel.OrganizationDocumentName.Trim().IsNotNull())
            {
                var orgDocName = _wcfService.InvokeService<IOrganizationService, LibraryAsset>((svc) => svc.CheckDocExists(orgDocViewModel.OrganizationDocumentName.Trim()));
                if (orgDocName.IsNotNull())
                    ModelState.AddModelError("OrganizationDocumentName", "Document name already exists.");
            }
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
                    orgViewModel.NoOfQuickView = 1;
                    orgViewModel.QuickViewEnd = false;
                }

                var mappedModel = Mapper.Map<OrganizationViewModel, Organization>(orgViewModel);
                mappedModel.CreatedDate = mappedModel.UpdatedDate = DateTime.Now;
                mappedModel.CreatedBy = mappedModel.UpdatedBy = SessionContext.CurrentUser.Id;
                mappedModel.LibraryFolders = new List<LibraryFolder>();
                var libFolder = new LibraryFolder();
                var libAsset = GetLibraryAsset(orgViewModel.DisplayPicture);
                ICollection<LibraryAsset> libAssetsColl = new Collection<LibraryAsset>();
                if (libAsset != null)
                    libAssetsColl.Add(libAsset);
                libFolder.LibraryAssets = libAssetsColl;
                mappedModel.LibraryFolders.Add(libFolder);
                mappedModel.Active = true;
                var organizationModel = _wcfService.InvokeService<IUserService, Organization>((svc) => svc.AddOrganization(mappedModel));
                if (organizationModel != null)
                {
                    var lFolder = organizationModel.LibraryFolders.FirstOrDefault(x => x.FolderName == "Assets");
                    SaveImageToDisk(lFolder, orgViewModel.DisplayPicture);
                    orgViewModel.CreationSuccessful = true;
                    orgViewModel.Message = "{0} saved successfully.Your account will be activated for use after the approval process is completed by us.".ToFormat(organizationModel.OrganizationName);
                    SessionContext.CurrentUser.OrganizationId = organizationModel.Id;
                }
            }
            else
            {
                ModelState.AddModelError("", "There was an error while saving. Please try again.");

            }
            return View(orgViewModel);
        }

        private string SaveImageToDisk(LibraryFolder libFolder, HttpPostedFileBase httpPostedFileBase, string userfileName = null)
        {
            if (httpPostedFileBase.ContentLength > 0)
            {
                var appBasePath = ConfigurationManager.AppSettings["AppAssetPath"];
                var appVirtuaPath = @"~\ResizedImages";
                var orgPath = Path.Combine(libFolder.OrganizationId.ToString(), libFolder.FolderName);
                var assetPath = Path.Combine(appBasePath, orgPath);
                string fileName = null;
                if (userfileName == null)
                {
                    fileName = Path.GetFileName(httpPostedFileBase.FileName);
                }
                else
                {
                    string[] splitter = { "." };
                    var name = httpPostedFileBase.FileName.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    var extns = name[1];
                    fileName = string.Concat(userfileName, ".", extns);
                }
                var path = Path.Combine(assetPath, fileName);
                httpPostedFileBase.SaveAs(path);

                var request = System.Web.HttpContext.Current.Request;
                var urlHelper = new UrlHelper(request.RequestContext);
                var rootPath = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + urlHelper.Content("~/");
                var appPath = appVirtuaPath.Replace(@"~\", "");
                var virtualOrgPath = orgPath.Replace("\\", "/");
                var servePath = @"{0}{1}/{2}/{3}".ToFormat(rootPath, appPath, virtualOrgPath, fileName);

                return servePath;
            }
            return null;
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
                    ModelState.AddModelError("OrganizationContact", "Email Of Contact-Person is mandatory.");
                if (!orgViewModel.VerificationDate.HasValue && !orgViewModel.IsSelfie)
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
                if (orgViewModel.DepartmentId.HasValue)
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

        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult OrganizationDocs()
        {
            var orgDocViewModel = new OrganizationDocumentViewModel() { OrganizationDocumentViewModels = new List<OrganizationDocumentViewModel>() };
            return View(orgDocViewModel);
        }

        private bool CheckOrganizationExists(string orgName)
        {
            var organization = _wcfService.InvokeService<IUserService, bool>((svc) => svc.CheckOrganizationExist(orgName));
            return organization;
        }

        private bool CheckEmailExists(string referencedEmail)
        {
            var user = _wcfService.InvokeService<IUserService, User>((svc) => svc.CheckEmailExists(referencedEmail));
            if (user != null)
                return true;
            return false;
        }
    }
}