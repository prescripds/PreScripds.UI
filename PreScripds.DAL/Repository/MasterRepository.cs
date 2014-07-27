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
using PreScripds.Infrastructure.UnitOfWork;

namespace PreScripds.DAL.Repository
{
    public class MasterRepository : IMasterRepository
    {
        public MasterRepository(PreScripdsDb context)
        {

        }
        #region Department Cache
        public ICollection<Department> GetDepartments()
        {
            using (var uow = new UnitOfWork())
            {
                var departments = CacheService.Get<ICollection<Department>>(Constants.CacheKeys.DEPARTMENTS);
                if (departments == null)
                {
                    var newDepartments = uow.GetRepository<Department>().Items.ToList();
                    CacheService.Set(Constants.CacheKeys.DEPARTMENTS, newDepartments);
                    return newDepartments;
                }
                return departments;
            }
        }

        #endregion

        #region Module Cache
        public ICollection<Module> GetModule()
        {
            using (var uow = new UnitOfWork())
            {
                var modules = CacheService.Get<ICollection<Module>>(Constants.CacheKeys.MODULE);
                if (modules == null)
                {
                    var newModules = uow.GetRepository<Module>().Items.ToList();
                    CacheService.Set(Constants.CacheKeys.MODULE, newModules);
                    return newModules;
                }
                return modules;
            }
        }

        #endregion

        #region Permission Cache
        public ICollection<Permission> GetPermission()
        {
            using (var uow = new UnitOfWork())
            {

                var permissions = CacheService.Get<ICollection<Permission>>(Constants.CacheKeys.PERMISSION);
                if (permissions == null)
                {
                    var newPermissions = uow.GetRepository<Permission>().Items.ToList();
                    CacheService.Set(Constants.CacheKeys.PERMISSION, newPermissions);
                    return newPermissions;
                }
                return permissions;
            }

        }

        #endregion

        #region Country Cache
        public ICollection<Country> GetCountry()
        {
            using (var uow = new UnitOfWork())
            {
                var countries = CacheService.Get<ICollection<Country>>(Constants.CacheKeys.COUNTRY);
                if (countries == null)
                {
                    var newCountries = uow.GetRepository<Country>().Items.ToList();
                    CacheService.Set(Constants.CacheKeys.COUNTRY, newCountries);
                    return newCountries;
                }
                return countries;
            }

        }
        #endregion

        #region State Cache
        public ICollection<State> GetState()
        {
            using (var uow = new UnitOfWork())
            {

                var states = CacheService.Get<ICollection<State>>(Constants.CacheKeys.STATE);
                if (states == null)
                {
                    var newStates = uow.GetRepository<State>().Items.ToList();
                    CacheService.Set(Constants.CacheKeys.STATE, newStates);
                    return newStates;
                }
                return states;
            }
        }
        #endregion

        //#region City Cache
        //public ICollection<City> GetCity()
        //{
        //    var cities = CacheService.Get<ICollection<City>>(Constants.CacheKeys.CITY);
        //    if (cities == null)
        //    {
        //        var newCities = ContextRep.Cities.ToList();
        //        CacheService.Set(Constants.CacheKeys.CITY, newCities);
        //        return newCities;
        //    }
        //    return cities;
        //}
        //#endregion

        #region Security Question Cache
        public ICollection<SecurityQuestion> GetSecurityQuestion()
        {
            using (var uow = new UnitOfWork())
            {
                var securityQuestions = CacheService.Get<ICollection<SecurityQuestion>>(Constants.CacheKeys.SECURITY_QUESTION);
                if (securityQuestions == null)
                {
                    var newSecurityQuestions = uow.GetRepository<SecurityQuestion>().Items.ToList();
                    CacheService.Set(Constants.CacheKeys.SECURITY_QUESTION, newSecurityQuestions);
                    return newSecurityQuestions;
                }
                return securityQuestions;
            }

        }
        #endregion
    }
}
