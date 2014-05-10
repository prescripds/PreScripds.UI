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

namespace PreScripds.DAL.Repository
{
    public class UserRepository : RepositoryBase<User, PreScripdsDb>, IUserRepository
    {
        private readonly PreScripdsDb _dbContext;
        public UserRepository(PreScripdsDb context)
            : base(context)
        {
            _dbContext = context;
        }
        public List<User> GetUsers()
        {
            var userLst = ContextRep.users.Include(x => x.UserLogin).Include(x => x.UserHistory).ToList();
            return userLst;
        }

        public User AddUser(User user)
        {
            if (user.UserLogin.IsCollectionValid())
            {
                var saltKey = EncryptionExtensions.CreateSaltKey();
                foreach (var userLogin in user.UserLogin)
                {
                    userLogin.SaltKey = saltKey;
                    var encryptedPassword = EncryptionExtensions.CreatePasswordHash(userLogin.Password,
                                userLogin.SaltKey);
                    userLogin.Password = encryptedPassword;
                    var encryptedSecurityAnswer = EncryptionExtensions.CreatePasswordHash(userLogin.SecurityAnswer, userLogin.SaltKey);
                    userLogin.SecurityAnswer = encryptedSecurityAnswer;

                }
                foreach (var userHistory in user.UserHistory)
                {
                    userHistory.SaltKey = saltKey;
                    var encryptCaptcha = EncryptionExtensions.Encrypt(userHistory.Captcha);
                    var encryptedPasswordCap = EncryptionExtensions.CreatePasswordCapHash(user.UserLogin.First().Password, userHistory.SaltKey, encryptCaptcha);
                    userHistory.PasswordCap = encryptedPasswordCap;
                    userHistory.Captcha = encryptCaptcha;
                }
            }
            Insert(user);
            SaveChanges();
            var usrHstry = user.UserHistory.First();
            UpdateUserLogin(usrHstry);
            return user;
        }

        private void UpdateUserLogin(UserHistory userHistory)
        {
            if (userHistory != null)
            {
                var userLogin = ContextRep.user_login.First(x => x.UserId == userHistory.UserId);
                userLogin.PasswordCap = userHistory.PasswordCap;
                userLogin.Captcha = userHistory.Captcha;
                DbContextExtensions.Update<UserLogin>(this._dbContext, userLogin);
                SaveChanges();
            }
        }

        public UserHistory AddUserHistory(UserHistory userHistory)
        {
            var userHstryFmDb = ContextRep.UserHistory.FirstOrDefault(x => x.Captcha == userHistory.Captcha && x.IpAddress == userHistory.IpAddress);

            if (userHistory == null)
            {
                var userHstry = new UserHistory()
                    {
                        IpAddress = userHistory.IpAddress,
                        CreatedDate = DateTime.Now,
                        Captcha = userHistory.Captcha,
                        SaltKey = userHistory.SaltKey,
                        UserId = userHistory.UserId,
                        PasswordCap = userHistory.PasswordCap
                    };

                ContextRep.UserHistory.Add(userHstry);
                ContextRep.SaveChanges();
                UpdateUserLogin(userHstry);
            }
            return userHistory;
        }

        public Role AddRole(Role role)
        {
            ContextRep.role.Add(role);
            ContextRep.SaveChanges();
            return role;
        }

        public Organization AddOrganization(Organization organization)
        {
            ContextRep.organizations.Add(organization);
            ContextRep.SaveChanges();
            return organization;
        }

        public User GetUserByUsername(string loginName, LoginType loginType)
        {
            var users = ContextRep.users.Include(x => x.UserLogin).Include(x => x.UserHistory).Where(x => x.Active).ToList();
            if (users.IsCollectionValid())
            {

                User loginUser;
                switch (loginType)
                {
                    case LoginType.IsUserName:
                        loginUser = users.FirstOrDefault(x => x.UserLogin.First().UserName == loginName);
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

        public User GetUserByEmail(string loginName)
        {
            var users = ContextRep.users.Include(x => x.UserLogin).Include(x => x.UserHistory).Where(x => x.Active).ToList();
            if (users.IsCollectionValid())
            {
                var loginUser = users.Where(x => x.Email == loginName);
                if (loginUser != null)
                    return loginUser.FirstOrDefault();
            }
            return null;
        }

        public User GetUserByMobile(string loginName)
        {
            var users = ContextRep.users.Include(x => x.UserLogin).Include(x => x.UserHistory).Where(x => x.Active).ToList();
            if (users.IsCollectionValid())
            {
                var loginUser = users.Where(x => x.Mobile == loginName.As<long>());
                if (loginUser != null)
                    return loginUser.FirstOrDefault();
            }
            return null;
        }
        public User CheckEmailExists(string email)
        {
            var users = ContextRep.users.Include(x => x.UserLogin).Where(x => x.Active && x.Email == email).ToList();
            if (users.IsCollectionValid())
            {
                return users.First();
            }
            return null;
        }

        public bool CheckRoleExists(Role role)
        {
            var roleModel = ContextRep.role.FirstOrDefault(x => x.RoleName == role.RoleName);
            if (roleModel == null)
                return false;
            return true;
        }

        public List<Role> GetRole(long organizationId)
        {
            var role = ContextRep.role.ToList();
            if (role.IsCollectionValid())
                return role.Where(x => x.OrganizationId == organizationId).ToList();
            return null;
        }

        public List<Department> GetDepartment(long organizationId)
        {
            var department = ContextRep.departments.Where(x => x.OrganizationId == organizationId).ToList();
            if (department.IsCollectionValid())
                return department;
            return null;
        }

        public bool CheckOrganizationExist(string orgName)
        {
            var organization = ContextRep.organizations.FirstOrDefault(x => x.OrganizationName == orgName && x.IsActive);
            if (organization != null)
                return true;
            return false;
        }
    }
}
