using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.DAL.Interface;
using PreScripds.Domain;
using PreScripds.Infrastructure.Repositories;

namespace PreScripds.DAL.Repository
{
    public class UserRepository : RepositoryBase<User, PreScripdsDb>, IUserRepository
    {
        public UserRepository(PreScripdsDb context)
            : base(context)
        {

        }
        public List<User> GetUsers()
        {
            var userLst = ContextRep.users.ToList();
            return userLst;
        }
    }
}
