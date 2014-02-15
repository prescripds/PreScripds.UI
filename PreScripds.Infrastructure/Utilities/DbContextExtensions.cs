using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Infrastructure
{
    public static class DbContextExtensions
    {
        public static void Update<T>(this DbContext context, T entity) where T : class
        {
            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
