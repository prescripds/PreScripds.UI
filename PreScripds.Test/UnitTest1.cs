using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreScripds.Domain;
using PreScripds.Infrastructure.Services;
using System.Runtime.Serialization;
using PreScripds.WebServices;
using System.Collections.Generic;
using PreScripds.DAL;
using PreScripds.Domain.Master;
using System.Threading.Tasks;

namespace PreScripds.Test
{
    [TestClass]
    public class UnitTest1
    {
        private WcfServiceInvoker _wcfService;
        private PreScripdsDb _context;

        [TestInitialize]
        public void TestSetup()
        {
            _wcfService = new WcfServiceInvoker();
            _context = new PreScripdsDb();
        }

        [TestMethod]
        public void TestMethod1()
        {
            //var countries = _wcfService.InvokeService<IUserService, User>(svc => svc.GetUserByUsername("shreyas"));
            //var users = userRepository.GetUsers();

            //var userLst = _wcfService.InvokeService<IMasterService, List<Department>>((svc => svc.GetDepartments()));
        }

        [TestMethod]
        public void AddUser()
        {
            //var user = new User()
            //{
            //    active = 1,
            //    address = "Bangalore",
            //    approved_by = 1,
            //    created_date = DateTime.Now,
            //    department_id = 1,
            //    firstname = "shreyas",
            //    gender = 1,
            //    isapproved = 1,
            //    lastname = "shrey",
            //    mobile = 1234567890,
            //    password = "abcd",
            //    recovery_email = "shreyasbs86@gmail.com",
            //    roleid = 1,
            //    salt_key = "Pass",
            //    updated_date = DateTime.Now,
            //    username = "shreyasbs86",
            //    email = "shreyasbs86@gmail.com"
            //};
            // var userFromDb = userRepository.AddUser(user);
        }
    }
}
