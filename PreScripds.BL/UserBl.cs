using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.BL.Interface;
using PreScripds.DAL;
using PreScripds.DAL.Interface;
using PreScripds.DAL.Repository;
using PreScripds.Domain;

namespace PreScripds.BL
{
    public class UserBl : IUserBl
    {
        private IUserRepository _userRepository;
        //private PreScripdsDb context;
        public UserBl(PreScripdsDb context)
        {
            _userRepository = new UserRepository(context);
        }
        
        public List<User> GetUsers()
        {
            var users = _userRepository.GetUsers();
            return users;
        }
    }
}
