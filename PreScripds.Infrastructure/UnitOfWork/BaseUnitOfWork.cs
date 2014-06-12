using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Infrastructure.Repositories;
using PreScripds.Infrastructure.Utilities;

namespace PreScripds.Infrastructure.UnitOfWork
{

    public class BaseUnitOfWork : IUnitOfWork
    {
        protected DbContext Context = null;

        private IDictionary<string, dynamic> _repositoryCache;

        public BaseUnitOfWork(DbContext context)
        {
            if (Context.IsNull())
                Context = context;
        }

        public RepositoryBase<T> GetRepository<T>() where T : class
        {
            var typeFullName = typeof(T).FullName;

            if (_repositoryCache == null)
            {
                _repositoryCache = new Dictionary<string, dynamic>();
            }
            if (!_repositoryCache.ContainsKey(typeFullName))
            {
                _repositoryCache.Add(typeFullName, new RepositoryBase<T>(Context));
            }
            return _repositoryCache[typeFullName] as RepositoryBase<T>;
        }

        public void SaveChanges()
        {
            if (Context.IsNotNull())
                Context.SaveChanges();
        }

        public void Dispose()
        {
            if (Context.IsNotNull())
                Context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
