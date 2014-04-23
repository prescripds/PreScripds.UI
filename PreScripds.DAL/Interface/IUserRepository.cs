using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;
using PreScripds.Infrastructure.Repositories;

namespace PreScripds.DAL.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetUsers();
        User AddUser(User user);
        Role AddRole(Role role);
        Task<User> GetUserByUsername(string loginName);
        Task<User> CheckEmailExists(string email);
        List<Role> GetRole(long organizationId);
        bool CheckRoleExists(Role role);
        List<Department> GetDepartment(long organizationId);
    }
}
