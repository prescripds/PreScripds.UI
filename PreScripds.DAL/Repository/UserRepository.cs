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
    public class UserRepository : RepositoryBase<Department, PreScripdsDb>, IUserRepository
    {
        private readonly PreScripdsDb _dbContext;
        public UserRepository(PreScripdsDb context)
            : base(context)
        {
            _dbContext = context;
        }
        public List<Department> GetUsers()
        {
            var userLst = _dbContext.departments.ToList();
            return userLst;
        }
    }
}
