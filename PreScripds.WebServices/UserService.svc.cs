using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PreScripds.Domain;
using System.ServiceModel.Activation;
using PreScripds.BL.Interface;
using PreScripds.BL;
using PreScripds.DAL;
using System.Data.Entity;
using System.Threading.Tasks;
using PreScripds.Domain.Enums;

namespace PreScripds.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class UserService : IUserService
    {
        /// <summary>
        /// BL interface will be called here. 
        /// Or BL methods will be exposed here.
        /// </summary>
        private readonly IUserBl _userBl;
        private PreScripdsDb _context;
        public UserService(PreScripdsDb context)
        {
            _userBl = new UserBl(context);
        }
        public UserService()
        {
            _context = new PreScripdsDb();
            _userBl = new UserBl(_context);
        }
        public List<User> GetUsers()
        {
            var users = _userBl.GetUsers();
            return users;
        }

        public User AddUser(User user)
        {
            var userFromDb = _userBl.AddUser(user);
            return userFromDb;
        }

        public Role AddRole(Role role)
        {
            var roleFromDb = _userBl.AddRole(role);
            return roleFromDb;
        }
        public Organization AddOrganization(Organization organization)
        {
            var organizationFromDb = _userBl.AddOrganization(organization);
            return organizationFromDb;
        }
        public User GetUserByUsername(string loginName, LoginType loginType)
        {
            var user = _userBl.GetUserByUsername(loginName, loginType);
            return user;
        }

        public User CheckEmailExists(string email)
        {
            var user = _userBl.CheckEmailExists(email);
            return user;
        }

        public List<Role> GetRole(long organizationId)
        {
            var roles = _userBl.GetRole(organizationId);
            return roles;
        }

        public bool CheckRoleExists(Role role)
        {
            var roleModel = _userBl.CheckRoleExists(role);
            return roleModel;
        }

        public List<Department> GetDepartment(long organizationId)
        {
            var departmentModel = _userBl.GetDepartment(organizationId);
            return departmentModel;
        }

        public bool CheckOrganizationExist(string orgName)
        {
            var isExist = _userBl.CheckOrganizationExist(orgName);
            return isExist;
        }

        public UserHistory AddUserHistory(UserHistory userHistory)
        {
            var userHstry = _userBl.AddUserHistory(userHistory);
            return userHstry;
        }

        public void UpdateUserLogin(UserHistory userHistory)
        {
            _userBl.UpdateUserLogin(userHistory);
        }
    }
}
