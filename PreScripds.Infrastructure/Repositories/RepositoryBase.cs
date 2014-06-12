using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity> where TEntity : class
    {
        public DbContext Context;
        internal DbSet<TEntity> _items;
        public RepositoryBase(DbContext context)
        {
            this.Context = context;
            this._items = context.Set<TEntity>();
        }
        //protected TRep ContextRep { get; private set; }
        //protected DbSet<T> Set { get; set; }

        #region Repository Members

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _items.Attach(entityToDelete);
            }
            _items.Remove(entityToDelete);
        }

        public virtual void Insert(TEntity entity)
        {
            _items.Add(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _items.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        //public virtual IQueryable<T> Include<TProp>(Expression<Func<T, TProp>> propSelector)
        //{
        //    return Set.Include(propSelector);
        //}
        public IQueryable<TEntity> Items { get { return _items; } }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }


        #endregion
    }
}
