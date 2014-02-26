using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Infrastructure.Repositories
{
    public class RepositoryBase<T, TRep> : IRepository<T>
        where T : class
        where TRep : DbContext
    {
        public RepositoryBase(DbContext context)
        {
            Set = context.Set<T>();
            ContextRep = context as TRep;
        }
        protected TRep ContextRep { get; private set; }
        protected DbSet<T> Set { get; set; }

        #region Repository Members

        public virtual void Delete(T entity)
        {
            Set.Remove(entity);
        }

        public virtual void Insert(T entity)
        {
            Set.Add(entity);
        }

        public virtual void Update(T entity)
        {
            Set.Attach(entity);
            ContextRep.Entry(entity).State = EntityState.Modified;
        }

        public virtual IQueryable<T> Include<TProp>(Expression<Func<T, TProp>> propSelector)
        {
            return Set.Include(propSelector);
        }
        public IQueryable<T> Items { get { return Set.AsQueryable(); } }

        public void SaveChanges()
        {
            ContextRep.SaveChanges();
        }


        #endregion
    }
}
