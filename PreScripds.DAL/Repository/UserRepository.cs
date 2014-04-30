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
            var userLst = ContextRep.users.Include(x => x.UserLogin).ToList();
            return userLst;
        }

        public User AddUser(User user)
        {
            if (user.UserLogin.IsCollectionValid())
            {
                foreach (var userLogin in user.UserLogin)
                {
                    var saltKey = EncryptionExtensions.CreateSaltKey();
                    userLogin.SaltKey = saltKey;
                    var encryptedPassword = EncryptionExtensions.CreatePasswordHash(userLogin.Password,
                                userLogin.SaltKey);
                    userLogin.Password = encryptedPassword;
                    var encryptedSecurityAnswer = EncryptionExtensions.CreatePasswordHash(userLogin.SecurityAnswer, userLogin.SaltKey);
                    userLogin.SecurityAnswer = encryptedSecurityAnswer;
                }
            }
            ContextRep.users.Add(user);
            ContextRep.SaveChangesAsync();
            return user;
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

        public User GetUserByUsername(string loginName)
        {
            var users = ContextRep.users.Include(x => x.UserLogin).Where(x => x.Active).ToList();
            if (users.IsCollectionValid())
            {
                var loginUser = users.Where(x => x.UserLogin.First().UserName == loginName);
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
