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
        #region Role Cache
        //public ICollection<Role> GetRoles()
        //{
        //    var roles = CacheService.Get<ICollection<Role>>(Constants.CacheKeys.ROLE);
        //    if (roles == null)
        //    {
        //        var newRoles = ContextRep.roles.ToList();
        //        CacheService.Set(Constants.CacheKeys.ROLE, newRoles);
        //        return newRoles;
        //    }
        //    return roles;
        //}

        #endregion

        #region Country Cache
        public ICollection<Country> GetCountry()
        {
            var countries = CacheService.Get<ICollection<Country>>(Constants.CacheKeys.COUNTRY);
            if (countries == null)
            {
                var newCountries = ContextRep.countries.ToList();
                CacheService.Set(Constants.CacheKeys.COUNTRY, newCountries);
                return newCountries;
            }
            return countries;
        }
        #endregion
    }
}
