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
using System.ServiceModel.Activation;

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
        private readonly IUserRepository _userRepository;
        public UserService(PreScripdsDb Context)
        {
            _userRepository = new UserRepository(Context);
        }
        //NOTE: Needed for service to run.
        public UserService()
        {

        }
        public List<User> GetUsers()
        {
            var users = _userRepository.GetUsers();
            return users;
        }
    }
}
