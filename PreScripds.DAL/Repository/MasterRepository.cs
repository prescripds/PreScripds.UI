using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.DAL.Interface;
using PreScripds.Domain;
using PreScripds.Domain.Master;
using PreScripds.Infrastructure;
using PreScripds.Infrastructure.Repositories;

namespace PreScripds.DAL.Repository
{
    public class MasterRepository : RepositoryBase<Department, PreScripdsDb>, IMasterRepository
    {
        public MasterRepository(PreScripdsDb context)
            : base(context)
        {

        }
        #region Department Cache
        //public ICollection<Department> GetDepartments()
        //{
        //    var departments = CacheService.Get<ICollection<Department>>(Constants.CacheKeys.DEPARTMENTS);
        //    if (departments == null)
        //    {
        //        var newDepartments = ContextRep.departments.ToList();
        //        CacheService.Set(Constants.CacheKeys.DEPARTMENTS, newDepartments);
        //        return newDepartments;
        //    }
        //    return departments;
        //}

        #endregion
        #region Permission Cache
        public ICollection<Permission> GetPermission()
        {
            var permissions = CacheService.Get<ICollection<Permission>>(Constants.CacheKeys.PERMISSION);
            if (permissions == null)
            {
                var newPermissions = ContextRep.Permissions.ToList();
                CacheService.Set(Constants.CacheKeys.PERMISSION, newPermissions);
                return newPermissions;
            }
            return permissions;
        }

        #endregion

        #region Country Cache
        public ICollection<Country> GetCountry()
        {
            var countries = CacheService.Get<ICollection<Country>>(Constants.CacheKeys.COUNTRY);
            if (countries == null)
            {
                var newCountries = ContextRep.Countries.ToList();
                CacheService.Set(Constants.CacheKeys.COUNTRY, newCountries);
                return newCountries;
            }
            return countries;
        }
        #endregion

        #region State Cache
        public ICollection<State> GetState()
        {
            var states = CacheService.Get<ICollection<State>>(Constants.CacheKeys.STATE);
            if (states == null)
            {
                var newStates = ContextRep.States.ToList();
                CacheService.Set(Constants.CacheKeys.STATE, newStates);
                return newStates;
            }
            return states;
        }
        #endregion

        #region City Cache
        public ICollection<City> GetCity()
        {
            var cities = CacheService.Get<ICollection<City>>(Constants.CacheKeys.CITY);
            if (cities == null)
            {
                var newCities = ContextRep.Cities.ToList();
                CacheService.Set(Constants.CacheKeys.CITY, newCities);
                return newCities;
            }
            return cities;
        }
        #endregion

        #region Security Question Cache
        public ICollection<SecurityQuestion> GetSecurityQuestion()
        {
            var securityQuestions = CacheService.Get<ICollection<SecurityQuestion>>(Constants.CacheKeys.SECURITY_QUESTION);
            if (securityQuestions == null)
            {
                var newSecurityQuestions = ContextRep.SecurityQuestions.ToList();
                CacheService.Set(Constants.CacheKeys.SECURITY_QUESTION, newSecurityQuestions);
                return newSecurityQuestions;
            }
            return securityQuestions;
        }
        #endregion
    }
}
