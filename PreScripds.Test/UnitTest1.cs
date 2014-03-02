using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreScripds.DAL.Interface;
using PreScripds.DAL.Repository;
using PreScripds.Domain;
using PreScripds.Infrastructure.Services;

namespace PreScripds.Test
{
    [TestClass]
    public class UnitTest1
    {
        private WcfServiceInvoker _wcfService;
        private IUserRepository userRepository;
        [TestInitialize]
        public void TestSetup()
        {
            userRepository = new UserRepository();
            _wcfService = new WcfServiceInvoker();
        }

        [TestMethod]
        public void TestMethod1()
        {
            var users = userRepository.GetUsers();
        }

        [TestMethod]
        public void AddUser()
        {
            var user = new User()
            {
                active = 1,
                address = "Bangalore",
                approved_by = 1,
                created_date = DateTime.Now,
                department_id = 1,
                firstname = "shreyas",
                gender = 1,
                isapproved = 1,
                lastname = "shrey",
                mobile = 1234567890,
                password = "abcd",
                recovery_email = "shreyasbs86@gmail.com",
                roleid = 1,
                salt_key = "Pass",
                updated_date = DateTime.Now,
                username = "shreyasbs86",email="shreyasbs86@gmail.com"
            };
            var userFromDb = userRepository.AddUser(user);
        }
    }
}
