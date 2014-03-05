using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PreScripds.BL;
using PreScripds.BL.Interface;
using PreScripds.DAL;
using PreScripds.Domain;

namespace PreScripds.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MasterService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MasterService.svc or MasterService.svc.cs at the Solution Explorer and start debugging.
    public class MasterService : IMasterService
    {
        private readonly IMasterBl _masterBl;
        private PreScripdsDb _context;
        public MasterService(PreScripdsDb context)
        {
            _masterBl = new MasterBl(context);
        }
        public MasterService()
        {
            _context = new PreScripdsDb();
            _masterBl = new MasterBl(_context);
        }
        public List<Department> GetDepartments()
        {
            var departments = _masterBl.GetDepartments().ToList();
            return departments;
        }
    }
}
