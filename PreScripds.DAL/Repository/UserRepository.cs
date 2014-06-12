using System;
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

namespace PreScripds.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PreScripdsDb _dbContext;
        public UserRepository(PreScripdsDb context)
        {
            _dbContext = context;
        }
        public List<User> GetUsers()
        {
            using (var uow = new UnitOfWork())
            {
                var userLst = uow.GetRepository<User>().Items.Include(x => x.UserLogins.SelectMany(y => y.UserHistories)).ToList();
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
                user.UserLogins.ToList().ForEach((x) =>
                                                    {
                                                        uow.GetRepository<UserLogin>().Insert(x);
                                                    });

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
                    //var userLogin = uow.GetRepository<UserHistory>().Items.FirstOrDefault(x => x.Id == userHistory.UserloginId);
                    //userLogin.PasswordCap = userHistory.PasswordCap;
                    //userLogin.Captcha = userHistory.Captcha;
                    //uow.GetRepository<UserLogin>().Update(userLogin);
                    //uow.SaveChanges();
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
                uow.GetRepository<Organization>().Insert(organization);
                uow.SaveChanges();
                return organization;
            }

        }

        public User GetUserByUsername(string loginName, LoginType loginType)
        {
            using (var uow = new UnitOfWork())
            {
                var users = uow.GetRepository<User>().Items.Include(x => x.UserLogins.SelectMany(y => y.UserHistories)).Where(x => x.Active).ToList();
                if (users.IsCollectionValid())
                {

                    User loginUser;
                    switch (loginType)
                    {
                        case LoginType.IsUserName:
                            loginUser = users.FirstOrDefault(x => x.UserLogins.First().UserName == loginName);
                            return loginUser;
                            break;
                        case LoginType.IsMobile:
                            loginUser = users.FirstOrDefault(x => x.Mobile == loginName.As<long>());
                            return loginUser;
                            break;
                        case LoginType.IsEmail:
                            loginUser = users.FirstOrDefault(x => x.Email == loginName);
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
                var users = uow.GetRepository<User>().Items.Include(x => x.UserLogins.SelectMany(y => y.UserHistories)).Where(x => x.Active && x.Email == email).ToList();
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
            //TODO:Get roles for a user from userinrole table.
            using (var uow = new UnitOfWork())
            {
                var role = uow.GetRepository<UserInRole>().Items.ToList();
                //if (role.IsCollectionValid())
                //{
                //    return role.Where(x => x.Organization.Id == organizationId).ToList();
                //}
                //
                return null;
            }

        }

        public List<Department> GetDepartment(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var department = uow.GetRepository<Department>().Items.Where(x => x.Id == organizationId).ToList();
                if (department.IsCollectionValid())
                    return department;
                return null;
            }

        }

        public bool CheckOrganizationExist(string orgName)
        {
            using (var uow = new UnitOfWork())
            {
                var organization = uow.GetRepository<Organization>().Items.FirstOrDefault(x => x.OrganizationName == orgName && x.Active);
                if (organization != null)
                    return true;
                return false;
            }

        }
    }
}
