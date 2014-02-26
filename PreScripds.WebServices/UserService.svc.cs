using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PreScripds.Domain;
using PreScripds.DAL;
using PreScripds.DAL.Interface;
using PreScripds.DAL.Repository;

namespace PreScripds.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService()
            : base(new PreScripdsDb())
        {
            _userRepository = new UserRepository((PreScripdsDb)Context);
        }
        public List<User> GetUsers()
        {
            var users = _userRepository.GetUsers();
            return users;
        }
    }
}
