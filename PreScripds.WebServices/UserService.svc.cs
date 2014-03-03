﻿using System;
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
        private readonly IMasterBl _userBl;
        private PreScripdsDb _context;
        public UserService(PreScripdsDb context)
        {
            _userBl = new MasterBl(context);
        }
        public UserService()
        {
            _context = new PreScripdsDb();
            _userBl = new MasterBl(_context);
        }
        public List<User> GetUsers()
        {
            //var users = _userBl.GetDepartments();
            return new List<User>();
        }

        public List<Department> GetDepartments()
        {
            var departments = _userBl.GetDepartments().ToList();
            return departments;
        }
    }
}