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
                            var modInDept = _wcfService.InvokeService<IOrganizationService, List<ModuleInDepartment>>((svc) => svc.GetAllModuleInDepartment());
                            if (!modInDept.IsCollectionValid())
                                return RedirectToAction("ModuleInDepartment", "Dashboard");
                            return RedirectToAction("Approvals", "Dashboard");
                        }
                    }
                    // return RedirectToAction("Index", "Dashboard");
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
            var moduleViewModel = new ModuleViewModel() { ModuleViewModels = new List<ModuleViewModel>() };
            var departmentInOrgId = _wcfService.InvokeService<IOrganizationService, List<DepartmentInOrganization>>((svc) => svc.GetDepartmentInOrganization(SessionContext.CurrentUser.OrganizationId.Value));
            moduleViewModel.DepartmentInOrg = departmentInOrgId;
            moduleViewModel.Departments = new List<Department>();
            foreach (var department in departmentInOrgId)
            {
                var departmentFromDb = _wcfService.InvokeService<IOrganizationService, Department>((svc) => svc.GetDepartmentById(department.DepartmentId.Value));
                moduleViewModel.Departments.Add(departmentFromDb);
                moduleViewModel.ModuleInDepartments = departmentFromDb.ModuleInDepartments.ToList();
                foreach (var mod in moduleViewModel.ModuleInDepartments)
                {
                    var module = _wcfService.InvokeService<IOrganizationService, Module>((svc) => svc.GetModuleById(mod.ModuleId.Value));
                    var mappedModule = Mapper.Map<Module, ModuleViewModel>(module);
                    moduleViewModel.ModuleViewModels.Add(mappedModule);
                }
            }
            return View(moduleViewModel);
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult AddModule(ModuleViewModel moduleViewModel)
        {
            ValidateModuleViewModel(moduleViewModel);
            if (ModelState.IsValid)
            {
                moduleViewModel.ModuleInDepartments = new List<ModuleInDepartment>();
                moduleViewModel.ModuleViewModels = new List<ModuleViewModel>();
                var deptInOrg = new ModuleInDepartment()
                {
                    DepartmentId = moduleViewModel.DepartmentInOrgId,
                    Active = true
                };
                moduleViewModel.ModuleInDepartments.Add(deptInOrg);
                moduleViewModel.IsActive = true;
                var mappedModule = Mapper.Map<ModuleViewModel, Module>(moduleViewModel);
                _wcfService.InvokeService<IOrganizationService>((svc) => svc.AddModule(mappedModule));
                moduleViewModel.CreationSuccessful = true;
                moduleViewModel.Message = "The Module '{0}' is saved successfully.".ToFormat(moduleViewModel.ModuleName);
                var moduleInDept = _wcfService.InvokeService<IOrganizationService, List<ModuleInDepartment>>((svc) => svc.GetModuleInDepartment(moduleViewModel.DepartmentInOrgId));
                foreach (var mod in moduleInDept)
                {
                    var module = _wcfService.InvokeService<IOrganizationService, Module>((svc) => svc.GetModuleById(mod.ModuleId.Value));
                    var mappedModuleViewModel = Mapper.Map<Module, ModuleViewModel>(module);
                    moduleViewModel.ModuleViewModels.Add(mappedModuleViewModel);
                }
            }
            return View(moduleViewModel);
        }

        private void ValidateModuleViewModel(ModuleViewModel moduleViewModel)
        {
            if (moduleViewModel.DepartmentInOrgId == 0)
                ModelState.AddModelError("", "Please select a department.");
            if (moduleViewModel.ModuleName.IsEmpty() || moduleViewModel.ModuleName.IsNull())
                ModelState.AddModelError("ModuleName", "Module Name is mandatory.");
            if (moduleViewModel.ModuleDesc.IsEmpty() || moduleViewModel.ModuleDesc.IsNull())
                ModelState.AddModelError("ModuleDesc", "Module Description is mandatory.");
        }

        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult ModuleInDepartment()
        {
            var moduleInDept = new ModuleInDepartmentViewModel();
            var deptInOrg = _wcfService.InvokeService<IOrganizationService, List<Department>>((svc) => svc.GetDepartmentInOrg(SessionContext.CurrentUser.OrganizationId.Value));
            moduleInDept.Department = deptInOrg;
            moduleInDept.ModuleVm = new List<ModuleInDeptVM>();
            return View(moduleInDept);
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult ModuleInDepartment(ModuleInDepartmentViewModel moduleInDeptViewModel, string buttonType)
        {
            if (buttonType == "Add")
                return RedirectToAction("AddModule", "Dashboard");
            ValidateModuleInDepartmentVM(moduleInDeptViewModel);
            if (ModelState.IsValid)
            {
                var modInDeptLst = new List<ModuleInDepartment>();

                foreach (var mod in moduleInDeptViewModel.Modules)
                {
                    var modInDept = new ModuleInDepartment()
                    {
                        DepartmentId = moduleInDeptViewModel.DepartmentId,
                        ModuleId = mod.As<long>(),
                        Active = true
                    };
                    modInDeptLst.Add(modInDept);
                }
                _wcfService.InvokeService<IOrganizationService>((svc) => svc.AddModuleInDepartment(modInDeptLst));
                moduleInDeptViewModel.Message = "Thank you for choosing the modules.";
                moduleInDeptViewModel.CreationSuccessful = true;
            }
            return View(moduleInDeptViewModel);
        }

        private void ValidateModuleInDepartmentVM(ModuleInDepartmentViewModel moduleInDeptViewModel)
        {
            if (moduleInDeptViewModel.DepartmentId == 0)
                ModelState.AddModelError("DepartmentId", "Please slect a Department.");
            if (!moduleInDeptViewModel.Modules.IsCollectionValid())
                ModelState.AddModelError("Modules", "Please select a Module.");

        }

        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult AddDepartment()
        {
            var departmentViewModel = new DepartmentViewModel() { DepartmentViewModels = new List<DepartmentViewModel>() };
            var departmentInOrg = _wcfService.InvokeService<IOrganizationService, List<Department>>((svc) => svc.GetDepartmentInOrg(SessionContext.CurrentUser.OrganizationId.Value));
            var mappedDepartmentVM = Mapper.Map<List<Department>, List<DepartmentViewModel>>(departmentInOrg);
            if (departmentInOrg.IsCollectionValid())
                departmentViewModel.DepartmentViewModels = mappedDepartmentVM;
            return View(departmentViewModel);
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult AddDepartment(DepartmentViewModel departmentViewModel, string buttonType)
        {
            if (buttonType == "Next")
                return RedirectToAction("AddModule", "Dashboard");
            ValidateDepartmentViewModel(departmentViewModel);
            if (ModelState.IsValid)
            {
                departmentViewModel.DepartmentInOrganizationViewModels = new List<DepartmentInOrganizationViewModel>();
                var departmentInOrg = new DepartmentInOrganizationViewModel() { OrganizationId = SessionContext.CurrentUser.OrganizationId.Value, Active = true };
                departmentViewModel.DepartmentInOrganizationViewModels.Add(departmentInOrg);
                departmentViewModel.IsActive = true;
                departmentViewModel.CreatedBy = departmentViewModel.UpdatedBy = SessionContext.CurrentUser.Id;
                departmentViewModel.CreatedDate = departmentViewModel.UpdatedDate = DateTime.Now;
                departmentViewModel.DepartmentViewModels = new List<DepartmentViewModel>();

                var mappedDepartModel = Mapper.Map<DepartmentViewModel, Department>(departmentViewModel);
                _wcfService.InvokeService<IOrganizationService>((svc) => svc.AddDepartment(mappedDepartModel));

                var departmentsInOrg = _wcfService.InvokeService<IOrganizationService, List<Department>>((svc) => svc.GetDepartmentInOrg(SessionContext.CurrentUser.OrganizationId.Value));
                var mappedDeptInOrg = Mapper.Map<List<Department>, List<DepartmentViewModel>>(departmentsInOrg);

                if (mappedDeptInOrg.IsCollectionValid())
                    departmentViewModel.DepartmentViewModels = mappedDeptInOrg;

                departmentViewModel.CreationSuccessful = true;
                departmentViewModel.Message = "The Department '{0}' is saved successfully.".ToFormat(departmentViewModel.DepartmentName);
            }
            return View(departmentViewModel);
        }

        private void ValidateDepartmentViewModel(DepartmentViewModel departmentViewModel)
        {
            if (departmentViewModel.DepartmentName.Trim().IsEmpty() || departmentViewModel.DepartmentName.IsNull())
                ModelState.AddModelError("DepartmentName", "Department Name is mandatory.");
            if (departmentViewModel.DepartmentDescription.Trim().IsEmpty() || departmentViewModel.DepartmentDescription.IsNull())
                ModelState.AddModelError("DepartmentDescription", "Department Description is mandatory.");
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
            var deptInOrg = _wcfService.InvokeService<IOrganizationService, List<DepartmentInOrganization>>((svc) => svc.GetDepartmentInOrganization(SessionContext.CurrentUser.OrganizationId.Value));
            var mappedDeptInOrg = Mapper.Map<List<DepartmentInOrganization>, List<DepartmentInOrganizationViewModel>>(deptInOrg);
            if (departments.IsCollectionValid())
                deptInOrgViewModel.Department = departments;
            if (deptInOrg.IsCollectionValid())
                deptInOrgViewModel.DepartmentInOrganizationViewModel = mappedDeptInOrg;
            return View(deptInOrgViewModel);
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult DepartmentInOrg(DepartmentInOrgViewModel deptInOrgViewModel, string buttonType)
        {
            if (buttonType == "Add")
                return RedirectToAction("AddDepartment", "Dashboard");
            ValidateDepartInOrgViewModel(deptInOrgViewModel);
            if (ModelState.IsValid)
            {
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
                    //var checkDeptExists
                    _wcfService.InvokeService<IOrganizationService>((svc) => svc.AddDepartmentInOrg(mappedModel));
                    deptInOrgViewModel.CreationSuccessful = true;
                    deptInOrgViewModel.Message = "Thank you for choosing the Departments.";
                }

            }
            return View(deptInOrgViewModel);
        }

        private void ValidateDepartInOrgViewModel(DepartmentInOrgViewModel deptInOrgViewModel)
        {
            if (!deptInOrgViewModel.Departments.IsCollectionValid())
                ModelState.AddModelError("", " Please select the Departments you would like to include for your Organization.");
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
            if (buttonType == "Next")
                return RedirectToAction("DepartmentInOrg", "Dashboard");

            ValidateRoleViewModel(roleViewModel);
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
            if (roleViewModel.RoleName.Trim().IsEmpty() || roleViewModel.RoleName.Trim().IsNull())
                ModelState.AddModelError("RoleName", "Role Name is mandatory.");
            if (roleViewModel.RoleDesc.Trim().IsEmpty() || roleViewModel.RoleDesc.Trim().IsNull())
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
                mappedModel.IsHomeOrg = false;
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

        public JsonResult GetModules(long departmentId)
        {
            var modules = _wcfService.InvokeService<IOrganizationService, List<Module>>((svc) => svc.GetAllModule(departmentId));
            var moduleInVmLst = new List<ModuleInDeptVM>();
            if (modules.IsCollectionValid())
            {
                foreach (var module in modules)
                {
                    var moduleInVm = new ModuleInDeptVM() { Id = module.Id, ModuleName = module.ModuleName };
                    moduleInVmLst.Add(moduleInVm);
                }
                return Json(moduleInVmLst, JsonRequestBehavior.AllowGet);
            }
            return Json(moduleInVmLst, JsonRequestBehavior.AllowGet);
        }

        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult AddPermission()
        {
            var permissionViewModel = new PermissionSetViewModel()
            {
                Department = new List<Department>(),
                Permission = new List<Permission>(),
                Module = new List<Module>(),
                PermissionSetViewModels = new List<PermissionSetViewModel>()
            };
            AssignPermissionViewModel(permissionViewModel);
            return View(permissionViewModel);
        }

        private void AssignPermissionViewModel(PermissionSetViewModel permissionViewModel)
        {
            var department = _wcfService.InvokeService<IOrganizationService, List<DepartmentInOrganization>>((svc) => svc.GetDepartmentInOrganization(SessionContext.CurrentUser.OrganizationId.Value));
            if (department.IsCollectionValid())
            {
                foreach (var dept in department)
                {
                    var departmentInOrg = _wcfService.InvokeService<IOrganizationService, Department>((svc) => svc.GetDepartmentById(dept.DepartmentId.Value));
                    permissionViewModel.Department.Add(departmentInOrg);
                }
            }
            var module = _wcfService.InvokeService<IMasterService, List<Module>>((svc) => svc.GetModule());
            var permission = _wcfService.InvokeService<IMasterService, List<Permission>>((svc) => svc.GetPermission());
            if (permission.IsCollectionValid())
                permissionViewModel.Permission = permission;
            var permInSets = _wcfService.InvokeService<IOrganizationService, List<PermissionSet>>((svc) =>
                   svc.GetAllPermissionSet(SessionContext.CurrentUser.OrganizationId.Value));
            if (permInSets.IsCollectionValid())
            {
                foreach (var perSet in permInSets)
                {
                    permissionViewModel.PermissionSetName = perSet.PermissionSetName;
                    permissionViewModel.PermissionSetId = perSet.Id;
                    permissionViewModel.PermissionSetDescription = perSet.PermissionSetDescription;
                    permissionViewModel.DepartmentName = permissionViewModel.Department.FirstOrDefault(x => x.Id == perSet.DepartmentId.Value).DepartmentName;
                    permissionViewModel.ModuleName = module.FirstOrDefault(x => x.Id == perSet.ModuleId.Value).ModuleName;
                    List<long> permIds = new List<long>();
                    List<string> perNames = new List<string>();
                    foreach (var permId in perSet.PermissionInSets)
                    {
                        permIds.Add(permId.PermissionId.Value);
                    }
                    foreach (var id in permIds)
                    {
                        var permissionName = permission.FirstOrDefault(x => x.Id == id).PermissionName;
                        perNames.Add(permissionName);
                    }
                    permissionViewModel.PermissionName = string.Join(",", perNames.ToArray());
                    permissionViewModel.IsActive = perSet.Active.Value;
                    permissionViewModel.PermissionSetViewModels.Add(permissionViewModel);
                }
            }
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult AddPermission(PermissionSetViewModel permissionViewModel, string buttonType)
        {
            if (buttonType == "Next")
                return RedirectToAction("RoleInPermission", "Dashboard");
            ValidatePermissionSetViewModel(permissionViewModel);
            if (ModelState.IsValid)
            {
                permissionViewModel.IsActive = true;
                permissionViewModel.Department = new List<Department>();
                permissionViewModel.Module = new List<Module>();
                permissionViewModel.PermissionSetViewModels = new List<PermissionSetViewModel>();
                AssignPermissionViewModel(permissionViewModel);
                ValidateInputPermissionSet(permissionViewModel);
                if (ModelState.IsValid)
                {
                    var mappedPermissionSetModel = Mapper.Map<PermissionSetViewModel, PermissionSet>(permissionViewModel);
                    _wcfService.InvokeService<IOrganizationService>((svc) => svc.AddPermission(mappedPermissionSetModel));
                    permissionViewModel.CreationSuccessful = true;
                    permissionViewModel.Message = "The permission set '{0}' is saved successfully.".ToFormat(permissionViewModel.PermissionSetName);
                }
            }
            return View(permissionViewModel);
        }

        private void ValidateInputPermissionSet(PermissionSetViewModel permissionViewModel)
        {
            foreach (var permSet in permissionViewModel.PermissionSetViewModels)
            {
                if (permissionViewModel.PermissionSetName.Equals(permSet.PermissionSetName))
                    ModelState.AddModelError("PermissionSetName", "Permission Set Name '{0}' already exists.".ToFormat(permissionViewModel.PermissionSetName));
                var permInSet = permissionViewModel.PermissionSetViewModels
                    .Where(x => x.DepartmentId == permissionViewModel.DepartmentId && x.ModuleId == permissionViewModel.ModuleId
                    && x.IsActive && x.PermissionSetName == permissionViewModel.PermissionSetName).ToList();
                if (permInSet.IsCollectionValid())
                    ModelState.AddModelError("", "The permission set name '{0}' with the same department and module already exists.".ToFormat(permissionViewModel.PermissionSetName));
            }
        }

        private void ValidatePermissionSetViewModel(PermissionSetViewModel permissionViewModel)
        {
            if (permissionViewModel.PermissionSetName.Trim().IsEmpty() || permissionViewModel.PermissionSetName.Trim().IsNull())
                ModelState.AddModelError("PermissionName", "Permission Name is mandatory.");
            if (permissionViewModel.DepartmentId == 0)
                ModelState.AddModelError("DepartmentId", "Please select a Department.");
            if (permissionViewModel.ModuleId == 0)
                ModelState.AddModelError("ModuleId", "Please select a Module.");
            if (!permissionViewModel.PermissionSelected.IsCollectionValid())
                ModelState.AddModelError("PermissionSelected", "Please select a Permission.");
        }

        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult RoleInPermission()
        {
            var roleInPermissionVM = new RoleInPermissionViewModel()
            {
                RoleInPermissionViewModels = new List<RoleInPermissionViewModel>(),
                Roles = new List<Role>(),
                PermissionSets = new List<PermissionSet>()
            };
            GetRoleInPermissionFromDb(roleInPermissionVM);
            return View(roleInPermissionVM);
        }

        private void GetRoleInPermissionFromDb(RoleInPermissionViewModel roleInPermissionVM)
        {
            var role = _wcfService.InvokeService<IUserService, List<Role>>((svc) => svc.GetRole(SessionContext.CurrentUser.OrganizationId.Value));
            var permissionSet = _wcfService.InvokeService<IOrganizationService, List<PermissionSet>>((svc) => svc.GetAllPermissionSet(SessionContext.CurrentUser.OrganizationId.Value));
            var roleInPermissions = _wcfService.InvokeService<IOrganizationService, List<RoleInPermission>>((svc) => svc.GetAllRoleInPermission());
            if (roleInPermissions.IsCollectionValid())
            {
                var rolPerms = roleInPermissions.Where(x => x.Role.OrganizationId == SessionContext.CurrentUser.OrganizationId.Value).ToList();
                var mappedRolePermVm = Mapper.Map<List<RoleInPermissionViewModel>>(rolPerms);
                foreach (var roleInPerms in mappedRolePermVm)
                {
                    roleInPerms.PermissionSetName = permissionSet.FirstOrDefault(x => x.Id == roleInPerms.PermissionSetId).PermissionSetName;
                    roleInPerms.RoleName = role.FirstOrDefault(x => x.Id == roleInPerms.RoleId).RoleName;
                }
                roleInPermissionVM.RoleInPermissionViewModels = mappedRolePermVm;
            }
            roleInPermissionVM.PermissionSets = permissionSet;
            roleInPermissionVM.Roles = role;
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult RoleInPermission(RoleInPermissionViewModel roleInPermVm, string buttonType)
        {
            if (buttonType == "Next")
                return RedirectToAction("UserInRole", "Dashboard");
            ValidateRoleInPermissionVM(roleInPermVm);
            if (ModelState.IsValid)
            {
                var mappedModel = Mapper.Map<RoleInPermissionViewModel, RoleInPermission>(roleInPermVm);
                mappedModel.Active = true;
                _wcfService.InvokeService<IOrganizationService>((svc) => svc.AddRoleInPermission(mappedModel));
                roleInPermVm.CreationSuccessful = true;
                roleInPermVm.Message = "Thank you for choosing Role-Permission Mapping.";
                GetRoleInPermissionFromDb(roleInPermVm);
            }
            return View(roleInPermVm);
        }

        private void ValidateRoleInPermissionVM(RoleInPermissionViewModel roleInPermVm)
        {
            if (roleInPermVm.RoleId == 0)
                ModelState.AddModelError("RoleId", "Please select a Role.");
            if (roleInPermVm.PermissionSetId == 0)
                ModelState.AddModelError("PermissionSetId", "Please select a Permission.");
            var roleInPermissionFromDb = _wcfService.InvokeService<IOrganizationService, List<RoleInPermission>>((svc) => svc.GetAllRoleInPermission());
            if (roleInPermissionFromDb.IsCollectionValid())
            {
                var roleInPermByOrg = roleInPermissionFromDb.Where(x => x.Role.OrganizationId == SessionContext.CurrentUser.OrganizationId.Value).ToList();
                var roleInPermExist = roleInPermByOrg.Where(x => x.RoleId.Value.Equals(roleInPermVm.RoleId) && x.PermissionId.Value.Equals(roleInPermVm.PermissionSetId)).ToList();
                if (roleInPermExist.IsCollectionValid())
                    ModelState.AddModelError("", "Role and Permission already exists.");
            }
        }

        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult UserInRole()
        {
            var userInRoleVM = new UserInRoleViewModel()
            {
                Users = new List<User>(),
                Roles = new List<Role>(),
                UserInRoleViewModels = new List<UserInRoleViewModel>()
            };
            GetUserInRoleFromDb(userInRoleVM);
            return View(userInRoleVM);
        }

        private void GetUserInRoleFromDb(UserInRoleViewModel userInRoleVM)
        {
            var roles = _wcfService.InvokeService<IUserService, List<Role>>((svc) => svc.GetRole(SessionContext.CurrentUser.OrganizationId.Value));
            var usersFrmDb = _wcfService.InvokeService<IUserService, List<User>>((svc) => svc.GetUsers(SessionContext.CurrentUser.OrganizationId.Value));
            if (usersFrmDb.IsCollectionValid())
            {
                userInRoleVM.Users = usersFrmDb;
            }
            userInRoleVM.Roles = roles;
            var userInRoleFrmDb = _wcfService.InvokeService<IOrganizationService, List<UserInRole>>((svc) => svc.GetUserInRole(SessionContext.CurrentUser.OrganizationId.Value));
            if (userInRoleFrmDb.IsCollectionValid())
            {
                foreach (var userInRole in userInRoleFrmDb)
                {
                    var user = usersFrmDb.FirstOrDefault(x => x.Id == userInRole.UserId.Value);
                    var firstName = (user.FirstName.IsNotNull()) ? user.FirstName : string.Empty;
                    var lastName = (user.LastName.IsNotNull()) ? user.LastName : string.Empty;
                    var middleName = (user.MiddleName.IsNotNull()) ? user.MiddleName : string.Empty;
                    var userName = "{0} .{1}{2}".ToFormat(firstName, middleName, lastName);
                    userInRoleVM.UserName = userName;
                    userInRoleVM.RoleName = roles.FirstOrDefault(x => x.Id == userInRole.RoleId.Value).RoleName;
                    userInRoleVM.Id = userInRole.Id;
                    userInRoleVM.IsActive = userInRole.Active.Value;
                    userInRoleVM.UserInRoleViewModels.Add(userInRoleVM);
                }

            }
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public ActionResult UserInRole(UserInRoleViewModel userInRoleVM, string buttonType)
        {
            if (buttonType == "Next")
                return RedirectToAction("Index", "Dashboard");
            ValidateUserInRoleVM(userInRoleVM);
            List<UserInRole> userInRoleLst = new List<UserInRole>();
            if (ModelState.IsValid)
            {
                foreach (var userId in userInRoleVM.UsersSelected)
                {
                    var userInRole = new UserInRole()
                    {
                        RoleId = userInRoleVM.RoleId,
                        UserId = userId.As<long>(),
                        Active = true
                    };
                    userInRoleLst.Add(userInRole);
                }
                _wcfService.InvokeService<IOrganizationService>((svc) => svc.AddUserInRole(userInRoleLst));
                userInRoleVM.CreationSuccessful = true;
                userInRoleVM.Message = "Thank you for choosing the user(s) for the role '{0}'".ToFormat(userInRoleVM.RoleName);
            }
            return View(userInRoleVM);
        }

        private void ValidateUserInRoleVM(UserInRoleViewModel userInRoleVM)
        {
            if (userInRoleVM.RoleId == 0)
                ModelState.AddModelError("RoleId", "Please select a Role.");
            if (!userInRoleVM.UsersSelected.IsCollectionValid())
                ModelState.AddModelError("UsersSelected", "Please select the User.");
        }

        [PreScripds.UI.Common.Authorize]
        [HttpGet]
        public ActionResult Approvals()
        {
            var ApprovalViewModel = new ApprovalViewModel()
            {
                UserApprovalViewModel = new List<UserApprovalViewModel>(),
                OrganizationApprovalViewModel = new List<OrganizationApprovalViewModel>()
            };

            var userApprovedVM = new UserApprovalViewModel() { Role = new List<Role>(), Department = new List<Department>() };
            var users = _wcfService.InvokeService<IUserService, List<User>>((svc) => svc.GetUsers(SessionContext.CurrentUser.OrganizationId.Value));
            var roles = _wcfService.InvokeService<IUserService, List<Role>>((svc) => svc.GetRole(SessionContext.CurrentUser.OrganizationId.Value));
            var departments = _wcfService.InvokeService<IUserService, List<Department>>((svc) => svc.GetDepartmentInOrganization(SessionContext.CurrentUser.OrganizationId.Value));

            if (SessionContext.CurrentUser.IsHomeUrl && SessionContext.CurrentUser.IsHomeSuperAdmin.Value == true)
            {
                var organizations = _wcfService.InvokeService<IOrganizationService, List<Organization>>((svc) => svc.GetOrganizations());
                var organizationsEntity = new List<OrganizationApprovalViewModel>();
                foreach (var org in organizations)
                {
                    var organizationApprovalVM = new OrganizationApprovalViewModel();
                    var orgSuperAdmin = users.FirstOrDefault(x => x.IsOrgSuperAdmin.Value == true);
                    var userName = GetUserName(orgSuperAdmin);
                    organizationApprovalVM.UserName = userName;
                    organizationApprovalVM.OrganizationName = org.OrganizationName;
                    organizationApprovalVM.OrganizationId = org.Id;
                    organizationApprovalVM.IsApproved = org.Approved;
                    ApprovalViewModel.OrganizationApprovalViewModel.Add(organizationApprovalVM);
                }
            }

            foreach (var user in users)
            {
                userApprovedVM = new UserApprovalViewModel();
                userApprovedVM.UserId = user.Id;
                var userName = GetUserName(user);
                userApprovedVM.UserName = userName;
                userApprovedVM.IsApproved = (user.AdminApprove.HasValue) ? user.AdminApprove.Value : false;
                userApprovedVM.Role = roles;
                userApprovedVM.Department = departments;
                ApprovalViewModel.UserApprovalViewModel.Add(userApprovedVM);
            }
            return View(ApprovalViewModel);
        }

        private static string GetUserName(Domain.User user)
        {
            var middleName = string.Empty;
            if (!user.MiddleName.IsNull())
                middleName = user.MiddleName;
            var userName = string.Format("{0} {1}.{2}".ToFormat(user.FirstName, middleName, user.LastName));
            return userName;
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public void UserApprovals(long id, bool isActive)
        {
            if (id != 0)
            {
                _wcfService.InvokeService<IUserService>((svc) => svc.UpdateUserByAdmin(id, isActive));
            }
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public void UserRoleApproval(long id, long roleId)
        {
            if (roleId != 0)
            {
                _wcfService.InvokeService<IOrganizationService>((svc) => svc.UpdateUserInRole(id, roleId));
            }
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public void UserRoleDepartment(long id, long roleId, long departmentId)
        {
            if (departmentId != 0)
            {
                var userRoleDept = _wcfService.InvokeService<IOrganizationService, bool>((svc) => svc.AddUserRoleDepartment(id, roleId, departmentId));
            }
        }

        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public void UpdateDepartment(long id, bool status)
        {
            _wcfService.InvokeService<IOrganizationService>((svc) => svc.UpdateDepartment(id, status));
        }
        [PreScripds.UI.Common.Authorize]
        [HttpPost]
        public void UpdateModule(long id, bool status)
        {
            _wcfService.InvokeService<IOrganizationService>((svc) => svc.UpdateModule(id, status));
        }
    }
}