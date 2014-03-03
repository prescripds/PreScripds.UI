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
        private readonly PreScripdsDb _dbContext;
        public UserRepository(PreScripdsDb context)
            : base(context)
        {
            _dbContext = context;
        }
        public List<User> GetUsers()
        {
            var userLst = _dbContext.users.ToList();
            return userLst;
        }

        public User AddUser(User user)
        {
            _dbContext.users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }
    }
}
