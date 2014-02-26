using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Delete(T entity);
        void Insert(T entity);
        void Update(T entity);
        IQueryable<T> Include<TProp>(Expression<Func<T, TProp>> propSelector);
        void SaveChanges();

    }
}
