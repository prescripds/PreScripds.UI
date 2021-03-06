﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PreScripds.BL;
using PreScripds.BL.Interface;
using PreScripds.DAL;
using PreScripds.Domain;
using PreScripds.Domain.Master;

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
        public List<Permission> GetPermission()
        {
            var permissions = _masterBl.GetPermission().ToList();
            return permissions;
        }

        public List<Country> GetCountry()
        {
            var countries = _masterBl.GetCountry().ToList();
            return countries;
        }

        public List<State> GetState()
        {
            var states = _masterBl.GetState().ToList();
            return states;
        }

        public List<Module> GetModule()
        {
            var modules = _masterBl.GetModule().ToList();
            return modules;
        }

        public List<SecurityQuestion> GetSecurityQuestion()
        {
            var securityQuestions = _masterBl.GetSecurityQuestion().ToList();
            return securityQuestions;
        }

        public List<State> GetStateByCountry(long countryId)
        {
            var states = _masterBl.GetState().ToList();
            states = states.Where(x => x.CountryId == countryId).ToList();
            return states;
        }
        public List<Department> GetDepartment()
        {
            var department = _masterBl.GetDepartments().ToList();
            return department;
        }
    }
}
