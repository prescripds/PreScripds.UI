using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreScripds.Infrastructure.Services;

namespace PreScripds.Test
{
    [TestClass]
    public class UnitTest1
    {
        private WcfServiceInvoker _wcfService;
        private PreScripds.DAL.PreScripdsDb _dbContext;
        [TestInitialize]
        public void TestSetup()
        {
            _dbContext = new PreScripds.DAL.PreScripdsDb();
            _wcfService = new WcfServiceInvoker();
        }

        [TestMethod]
        public void TestMethod1()
        {
            var users = _dbContext.users;
        }
    }
}
