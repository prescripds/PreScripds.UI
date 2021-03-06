﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.DAL.Interface;
using PreScripds.Domain;
using PreScripds.Infrastructure.Repositories;
using System.Data.Entity;
using PreScripds.Infrastructure.Security;
using PreScripds.Infrastructure;
using MySql.Data.MySqlClient;
using PreScripds.Infrastructure.Utilities;
using PreScripds.Domain.Enums;
using PreScripds.Infrastructure.UnitOfWork;
using System.IO;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


namespace PreScripds.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PreScripdsDb _dbContext;
        public UserRepository(PreScripdsDb context)
        {
            _dbContext = context;
        }
        public List<User> GetUsers(long organzationId)
        {
            using (var uow = new UnitOfWork())
            {
                var userLst = uow.GetRepository<User>().Items
                    .Include(x => x.UserLogins.Select(y => y.UserHistories))
                    .Include(y => y.UserInRoles.Select(z => z.Role))
                    .Where(x => x.OrganizationId == organzationId).ToList();
                return userLst;
            }

        }

        public User AddUser(User user)
        {
            using (var uow = new UnitOfWork())
            {
                if (user.UserLogins.IsCollectionValid())
                {
                    var saltKey = EncryptionExtensions.CreateSaltKey();
                    foreach (var userLogin in user.UserLogins)
                    {
                        userLogin.saltkey = saltKey;
                        var encryptedPassword = EncryptionExtensions.CreatePasswordHash(userLogin.Password,
                                    userLogin.saltkey);
                        userLogin.Password = encryptedPassword;
                        var encryptedSecurityAnswer = EncryptionExtensions.CreatePasswordHash(userLogin.SecurityAnswer, userLogin.saltkey);
                        userLogin.SecurityAnswer = encryptedSecurityAnswer;

                    }
                    foreach (var userHistory in user.UserLogins.Select(x => x.UserHistories.FirstOrDefault()))
                    {
                        userHistory.saltkey = saltKey;
                        var encryptCaptcha = EncryptionExtensions.Encrypt(userHistory.Captcha);
                        var encryptedPasswordCap = EncryptionExtensions.CreatePasswordCapHash(user.UserLogins.First().Password, userHistory.saltkey, encryptCaptcha);
                        userHistory.PasswordCap = encryptedPasswordCap;
                        userHistory.Captcha = encryptCaptcha;
                        userHistory.CreatedDate = DateTime.Now;
                    }
                }

                if (user.UserLogins.IsCollectionValid())
                {
                    user.UserLogins.SelectMany(x => x.UserHistories).Each(s => uow.GetRepository<UserHistory>().Items.ToList().Add(s));
                }
                uow.GetRepository<User>().Insert(user);
                uow.SaveChanges();
                var usrHstry = user.UserLogins.Select(x => x.UserHistories.FirstOrDefault()).FirstOrDefault();
                UpdateUserLogin(usrHstry);
                return user;
            }

        }

        public void UpdateUserLogin(UserHistory userHistory)
        {
            using (var uow = new UnitOfWork())
            {
                if (userHistory != null)
                {
                    var userLogin = uow.GetRepository<UserLogin>().Items.FirstOrDefault(x => x.Id == userHistory.UserloginId);
                    userLogin.PasswordCap = userHistory.PasswordCap;
                    userLogin.Captcha = userHistory.Captcha;
                    uow.GetRepository<UserLogin>().Update(userLogin);
                    uow.SaveChanges();
                }
            }

        }

        public UserHistory AddUserHistory(UserHistory userHistory)
        {
            using (var uow = new UnitOfWork())
            {
                var userHstryFmDb = uow.GetRepository<UserHistory>().Items.FirstOrDefault(x => x.Captcha == userHistory.Captcha && x.IpAddress == userHistory.IpAddress);

                if (userHstryFmDb == null)
                {
                    var userHstry = new UserHistory()
                    {
                        IpAddress = userHistory.IpAddress,
                        CreatedDate = DateTime.Now,
                        Captcha = userHistory.Captcha,
                        saltkey = userHistory.saltkey,
                        UserloginId = userHistory.UserloginId,
                        PasswordCap = userHistory.PasswordCap
                    };
                    uow.GetRepository<UserHistory>().Insert(userHstry);
                    uow.SaveChanges();
                    UpdateUserLogin(userHstry);
                }
                return userHistory;
            }

        }

        public Role AddRole(Role role)
        {
            using (var uow = new UnitOfWork())
            {
                uow.GetRepository<Role>().Insert(role);
                uow.SaveChanges();
                return role;
            }

        }

        public Organization AddOrganization(Organization organization)
        {
            using (var uow = new UnitOfWork())
            {

                if (organization.LibraryFolders != null)
                {
                    var libFolders = CreateDefaultFolder(organization.LibraryFolders);
                    organization.LibraryFolders = libFolders;
                    organization.LibraryFolders.SelectMany(x => x.LibraryAssets)
                        .Each(la => uow.GetRepository<LibraryAsset>().Items.ToList().Add(la));
                    organization.LibraryFolders.SelectMany(x => x.LibraryAssets)
                        .SelectMany(lf => lf.LibraryAssetFiles)
                        .Each(laf => uow.GetRepository<LibraryAssetFile>().Items.ToList().Add(laf));
                }
                uow.GetRepository<Organization>().Insert(organization);
                uow.SaveChanges();
                UpdateOrgInUser(organization);
                CreateFoldersOnDisk(organization);

                return organization;
            }

        }

        private void UpdateOrgInUser(Organization organization)
        {
            using (var uow = new UnitOfWork())
            {
                var userFromDb = uow.GetRepository<User>().Items.FirstOrDefault(x => x.Id == organization.CreatedBy);
                userFromDb.OrganizationId = organization.Id;
                userFromDb.UpdatedBy = organization.CreatedBy;
                userFromDb.UpdatedDate = DateTime.Now;
                userFromDb.IsOrgSuperAdmin = true;
                userFromDb.AdminApprove = true;
                uow.GetRepository<User>().Update(userFromDb);
                uow.SaveChanges();
            }
        }

        private void CreateFoldersOnDisk(Organization organization)
        {
            using (var uow = new UnitOfWork())
            {
                var baseDir = ConfigurationManager.AppSettings["AppAssetPath"];
                var libraryFolders = uow.GetRepository<LibraryFolder>().Items.Where(x => x.OrganizationId == organization.Id).ToList();

                if (!Directory.Exists(baseDir))
                {
                    FileServiceProvider.CreateDirectory(baseDir, organization.Id.ToString());
                }
                var baseAppPathForOrg = Path.Combine(baseDir, organization.Id.ToString());
                foreach (var libraryFolder in libraryFolders)
                {
                    var appAssetPath = Path.Combine(baseAppPathForOrg, libraryFolder.FolderName);
                    FileServiceProvider.CreateDirectory(appAssetPath);
                }
            }
        }

        private List<LibraryFolder> CreateDefaultFolder(ICollection<LibraryFolder> libraryFolders)
        {
            var libFldrs = libraryFolders.ToList();
            var libFldr = libFldrs.FirstOrDefault();
            libFldr.FolderName = "Assets";
            libFldr.FolderHierarchy = "/";
            libFldr.Createdate = DateTime.Now;
            libFldr.LibraryAssets = libFldrs.FirstOrDefault().LibraryAssets;

            var libraryDocFolder = new LibraryFolder()
            {
                FolderName = "Documents",
                FolderHierarchy = "Assets/Documents",
                Createdate = DateTime.Now,
                ParentFolderId = 1,
                LibraryAssets = new Collection<LibraryAsset>()
            };
            //libFldrs.Add(libFldr);
            libFldrs.Add(libraryDocFolder);


            return libFldrs;
        }

        public User GetUserByUsername(string loginName, LoginType loginType)
        {
            using (var uow = new UnitOfWork())
            {

                if (loginName.IsNotNull() && loginName.Trim().IsNotNull() && loginName.Trim().IsNotEmpty())
                {
                    long? mobileNumber = null;
                    if (loginType == LoginType.IsMobile)
                        mobileNumber = loginName.As<long>();
                    User loginUser;
                    switch (loginType)
                    {
                        case LoginType.IsUserName:
                            loginUser = uow.GetRepository<User>().Items.Include(x => x.UserLogins.Select(y => y.UserHistories))
                                        .Include(x => x.UserInRoles
                                        .Select(y => y.Role.RoleInPermissions.Select(z => z.PermissionSet)))
                                        .FirstOrDefault(a => a.UserLogins.FirstOrDefault().UserName == loginName);
                            return loginUser;
                            break;
                        case LoginType.IsMobile:
                            loginUser = uow.GetRepository<User>().Items.Include(x => x.UserLogins.Select(y => y.UserHistories))
                                        .Include(x => x.UserInRoles
                                        .Select(y => y.Role.RoleInPermissions.Select(z => z.PermissionSet)))
                                        .FirstOrDefault(x => x.Active && x.Mobile == mobileNumber);
                            return loginUser;
                            break;
                        case LoginType.IsEmail:
                            loginUser = uow.GetRepository<User>().Items.Include(x => x.UserLogins.Select(y => y.UserHistories))
                                        .Include(x => x.UserInRoles
                                        .Select(y => y.Role.RoleInPermissions.Select(z => z.PermissionSet)))
                                        .FirstOrDefault(x => x.Active && x.Email == loginName);
                            return loginUser;
                            break;
                    }
                }
                return null;
            }

        }

        public User GetUserByEmail(string loginName)
        {
            using (var uow = new UnitOfWork())
            {
                var users = uow.GetRepository<User>().Items.Include(x => x.UserLogins.SelectMany(y => y.UserHistories)).Where(x => x.Active).ToList();
                if (users.IsCollectionValid())
                {
                    var loginUser = users.Where(x => x.Email == loginName);
                    if (loginUser != null)
                        return loginUser.FirstOrDefault();
                }
                return null;
            }

        }

        public User GetUserByMobile(string loginName)
        {
            using (var uow = new UnitOfWork())
            {
                var users = uow.GetRepository<User>().Items.Include(x => x.UserLogins.SelectMany(y => y.UserHistories)).Where(x => x.Active).ToList();
                if (users.IsCollectionValid())
                {
                    var loginUser = users.Where(x => x.Mobile == loginName.As<long>());
                    if (loginUser != null)
                        return loginUser.FirstOrDefault();
                }
                return null;
            }

        }
        public User CheckEmailExists(string email)
        {
            using (var uow = new UnitOfWork())
            {
                var users = uow.GetRepository<User>().Items.Include(x => x.UserLogins).Where(x => x.Active && x.Email == email).ToList();
                if (users.IsCollectionValid())
                {
                    return users.First();
                }
                return null;
            }

        }

        public bool CheckRoleExists(Role role)
        {
            using (var uow = new UnitOfWork())
            {
                var roleModel = uow.GetRepository<Role>().Items.FirstOrDefault(x => x.RoleName == role.RoleName);
                if (roleModel == null)
                    return false;
                return true;
            }

        }

        public List<Role> GetRole(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var role = uow.GetRepository<Role>().Items
                    .Include(x => x.RoleInPermissions.Select(z => z.PermissionSet))
                    .Include(x => x.UserInRoles).Where(x => x.OrganizationId == organizationId).ToList();
                return role;
            }

        }

        public List<Department> GetAllDepartment()
        {
            using (var uow = new UnitOfWork())
            {
                var department = uow.GetRepository<Department>().Items.ToList();
                if (department.IsCollectionValid())
                    return department;
                return null;
            }

        }
        public List<Department> GetDepartmentInOrganization(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var departments = new List<Department>();
                var deptInOrg = uow.GetRepository<DepartmentInOrganization>().Items.Where(x => x.OrganizationId == organizationId).ToList();
                if (deptInOrg.IsCollectionValid())
                {
                    foreach (var dept in deptInOrg)
                    {
                        var department = uow.GetRepository<Department>().Items.FirstOrDefault(x => x.Id == dept.DepartmentId.Value);
                        departments.Add(department);
                    }
                    return departments;
                }
                return departments;
            }
        }

        public bool CheckOrganizationExist(string orgName)
        {
            using (var uow = new UnitOfWork())
            {
                var organization = uow.GetRepository<Organization>().Items.FirstOrDefault(x => x.OrganizationName == orgName);
                if (organization != null)
                    return true;
                return false;
            }

        }

        public void UpdateUserByAdmin(long id, bool isActive)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.GetRepository<User>().Items.FirstOrDefault(x => x.Id == id);
                user.AdminApprove = isActive;
                uow.GetRepository<User>().Update(user);
                uow.SaveChanges();
            }
        }

        public User GetUserById(long Id)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.GetRepository<User>().Items
                    .Include(s => s.UserInRoles)
                    .Include(s => s.UserLogins.Select(p => p.UserHistories))
                    .Include(s => s.UserLogins)
                    .Include(s => s.UserRoleDepartments)
                    .FirstOrDefault(x => x.Id == Id);
                return user;
            }
        }

        public User UpdateUserProfile(User user)
        {
            if (user.Id != 0 && user.IsNotNull())
            {
                using (var uow = new UnitOfWork())
                {
                    var userLogin = user.UserLogins.FirstOrDefault();
                    var userLoginFrmDb = uow.GetRepository<UserLogin>().Items.FirstOrDefault(x => x.Id == userLogin.Id);
                    if (userLoginFrmDb.IsNotNull())
                    {
                        userLoginFrmDb.UserName = userLogin.UserName;
                        userLoginFrmDb.SecurityQuestionId = userLogin.SecurityQuestionId;
                        uow.GetRepository<UserLogin>().Update(userLoginFrmDb);
                        var userFromDb = uow.GetRepository<User>().Items.FirstOrDefault(x => x.Id == user.Id);
                        var assignedUser = AssignUserToDbUser(user, userFromDb);
                        uow.GetRepository<User>().Update(assignedUser);
                        uow.SaveChanges();
                    }
                }
            }
            return new User();
        }

        private User AssignUserToDbUser(User user, User userFromDb)
        {
            AutoMapper.Mapper.CreateMap<User, User>();
            var mapedProfile = AutoMapper.Mapper.Map(user, userFromDb);
            // userFromDb = mapedProfile;
            return mapedProfile;
        }

        public UserLogin GetUserLoginById(long id)
        {
            using (var uow = new UnitOfWork())
            {
                var userLogin = uow.GetRepository<UserLogin>().Items.Include(x => x.UserHistories).FirstOrDefault(x => x.UserId == id);
                return userLogin;
            }
        }
        public string ChangePassword(UserLogin userLogin)
        {
            using (var uow = new UnitOfWork())
            {
                var userLoginFrmDb = uow.GetRepository<UserLogin>().Items.FirstOrDefault(x => x.Id == userLogin.Id);
                var encryptedPassword = EncryptionExtensions.CreatePasswordHash(userLogin.Password,
                                   userLogin.saltkey);
                if (userLoginFrmDb.Password.Equals(encryptedPassword))
                    return "Current Password and New Password should not be the same.";
                userLoginFrmDb.Password = userLogin.Password = encryptedPassword;
                var userHstryFrmDb = uow.GetRepository<UserHistory>().Items.Where(x => x.UserloginId == userLogin.Id);
                foreach (var item in userHstryFrmDb)
                {
                    var encryptedPasswordCap = EncryptionExtensions.CreatePasswordCapHash(userLogin.Password, item.saltkey, item.Captcha);
                    item.PasswordCap = encryptedPasswordCap;
                    uow.GetRepository<UserHistory>().Update(item);
                }
                userLoginFrmDb.PasswordCap = userHstryFrmDb.FirstOrDefault().PasswordCap;
                uow.GetRepository<UserLogin>().Update(userLoginFrmDb);
                uow.SaveChanges();
                return "Successfully Saved.";
            }
        }

        public string ChangeSecurityAnswer(UserLogin userLogin)
        {
            using (var uow = new UnitOfWork())
            {
                var userLoginFrmDb = uow.GetRepository<UserLogin>().Items.FirstOrDefault(x => x.Id == userLogin.Id);
                var encryptedSecurityAnswer = EncryptionExtensions.CreatePasswordHash(userLogin.SecurityAnswer, userLoginFrmDb.saltkey);
                if (userLoginFrmDb.SecurityAnswer.Equals(encryptedSecurityAnswer))
                    return "Current Security Answer and New Security Answer should not be the same.";
                userLoginFrmDb.SecurityAnswer = encryptedSecurityAnswer;
                uow.GetRepository<UserLogin>().Update(userLoginFrmDb);
                return "Successfully saved.";
            }
        }
    }
}
