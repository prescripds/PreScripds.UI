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
        List<User> GetUsers();
        User AddUser(User user);
    }
}
