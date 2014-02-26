using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreScripds.DAL.Interface;
using PreScripds.DAL.Repository;
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
            var departments = userRepository.GetUsers();
        }
    }
}
