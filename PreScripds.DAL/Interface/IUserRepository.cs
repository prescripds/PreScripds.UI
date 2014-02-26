using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;
using PreScripds.Infrastructure.Repositories;

namespace PreScripds.DAL.Interface
{
    public interface IUserRepository : IRepository<Department>
    {
        List<Department> GetUsers();
    }
}
