using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Infrastructure
{
    public static class IQueryableExtensions
    {
        static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> PropertyInfoCache
            = new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

        public static IOrderedQueryable<TModel> OrderByDescending<TModel>(this IQueryable<TModel> source, string propertyName)
        {
            return source.OrderBy(propertyName, true);
        }

        public static IOrderedQueryable<TModel> OrderBy<TModel>(this IQueryable<TModel> source, string propertyName, bool desc = false)
        {
            var command = desc ? "OrderByDescending" : "OrderBy";
            Dictionary<string, PropertyInfo> memberLookup;
            if (!PropertyInfoCache.TryGetValue(typeof(TModel), out memberLookup))
            {
                var type = typeof(TModel);
                //Implemented two levels
                memberLookup = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList()
                    .SelectMany(p =>
                    {
                        if (!p.PropertyType.IsPrimitive && p.PropertyType != typeof(string) && p.PropertyType != typeof(DateTime))
                        {
                            return
                                p.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .ToDictionary(k => p.Name + "." + k.Name, v => v);

                        }

                        return new Dictionary<string, PropertyInfo> { { p.Name, p } };

                    }).ToDictionary(k => k.Key, v => v.Value);

                PropertyInfoCache.TryAdd(type, memberLookup);
            }

            PropertyInfo prop;

            if (!memberLookup.TryGetValue(propertyName, out prop))
            {
                throw new Exception("Unable to find property with name {0} in type {1}");
            }

            var sortExpr = BuildMemberAccessExpr<TModel>(propertyName);
            var resultExpression = Expression.Call(typeof(Queryable), command, new[] { typeof(TModel), prop.PropertyType }, source.Expression, sortExpr);
            return (IOrderedQueryable<TModel>)source.Provider.CreateQuery<TModel>(resultExpression);
        }

        private static Expression BuildMemberAccessExpr<TModel>(string propName)
        {
            var parameter = Expression.Parameter(typeof(TModel), "p");

            Expression body = propName.Split('.').Aggregate<string, Expression>(parameter, (current, member) => Expression.PropertyOrField(current, member));

            return Expression.Lambda(body, parameter);
        }

    }

}
