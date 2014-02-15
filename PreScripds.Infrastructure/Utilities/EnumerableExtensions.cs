using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;

namespace PreScripds.Infrastructure
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
            return source;
        }

        public static bool IsCollectionValid<T>(this ICollection<T> source)
        {
            return source != null && source.Count > 0;
        }

        public static bool IsCollectionValid<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        public static IEnumerable<T> Distinct<T, TK>(this IEnumerable<T> source, Func<T, TK> keySelector)
        {
            var dict = new HashSet<TK>();

            foreach (var item in source)
            {
                var key = keySelector(item);

                if (!dict.Contains(key))
                {
                    dict.Add(key);
                    yield return item;
                }
            }
        }

        public static List<T> ToListOrDefault<T>(this IEnumerable<T> source)
        {
            return (source != null && source.Any()) ? source.ToList() : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Current input</param>
        /// <param name="dest">Records in database</param>
        /// <param name="keySelector"></param>
        /// <param name="onInsert"></param>
        /// <param name="onUpdate"></param>
        /// <param name="onDelete"></param>
        public static void SyncLists<T>(IEnumerable<T> source, IEnumerable<T> dest, Func<T, long> keySelector, Action<T> onInsert, Action<T,T> onUpdate, Action<T> onDelete)
        {
            if ((source == null || !source.Any()) && (dest != null && dest.Any()))
                dest.Each(onDelete);
            else if (source != null && source.Any())
            {
                //source.Where(x => keySelector(x) == 0).Each(onInsert);
                if (dest != null && source != null)
                {
                    var lookupFordest = dest.Where(x => keySelector(x) > 0).ToDictionary(keySelector);
                    var itemsToInsert = source.Where(srcItem => !lookupFordest.ContainsKey(keySelector(srcItem))).ToArray();
                    itemsToInsert.Each(onInsert);
                }
                else if (source != null)
                    source.Each(onInsert);


                var lookupForSource = source.Where(x => keySelector(x) > 0)
                    .ToDictionary(keySelector);

                if (dest.IsCollectionValid())
                {
                    foreach (var destItem in dest)
                    {
                        if (lookupForSource.ContainsKey(keySelector(destItem)) && onUpdate != null)
                            onUpdate(lookupForSource[keySelector(destItem)], destItem);
                    }

                    if(onDelete != null)
                        dest.Where(destItem => !lookupForSource.ContainsKey(keySelector(destItem))).Each(onDelete);
                }
            }
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source, string[] colHeadersExclude = null)
        {
            if (source != null && source.Any())
            {
                var entityType = typeof(T);

                var propertyHeaderCollection = TypeDescriptor.GetProperties(entityType);

                var dataTable = CreateDataTable(propertyHeaderCollection, entityType.Name, colHeadersExclude);

                FillDataTable(dataTable, propertyHeaderCollection, source, colHeadersExclude);

                return dataTable;
            }
            return null;
        }

        private static void FillDataTable<T>(DataTable dataTable, IEnumerable propertyHeaderCollection, IEnumerable<T> stagingDataColl, string[] colHeadersExclude = null)
        {
            var isExcludeHeadersPrsesent = colHeadersExclude.IsCollectionValid();
            foreach (T dataItem in stagingDataColl)
            {
                var dataRow = dataTable.NewRow();
                foreach (PropertyDescriptor propertyInfo in propertyHeaderCollection)
                {
                    var colHeaderName = propertyInfo.Name;
                    if (isExcludeHeadersPrsesent)
                    {
                        if (!colHeadersExclude.Contains(colHeaderName))
                        {
                            dataRow[colHeaderName] = propertyInfo.GetValue(dataItem) ?? DBNull.Value;
                        }
                    }
                    else
                    {
                        dataRow[colHeaderName] = propertyInfo.GetValue(dataItem) ?? DBNull.Value;
                    }

                }
                dataTable.Rows.Add(dataRow);
            }

        }

        private static DataTable CreateDataTable(PropertyDescriptorCollection propertyHeaderCollection, string dataTableName, string[] colHeadersExclude = null)
        {
            var isExcludeHeadersPrsesent = colHeadersExclude.IsCollectionValid();
            var dataTable = new DataTable(dataTableName);
            foreach (PropertyDescriptor propertyInfo in propertyHeaderCollection)
            {
                var colHeaderName = propertyInfo.Name;
                if (!isExcludeHeadersPrsesent || !colHeadersExclude.Contains(colHeaderName))
                {
                    var propertyType = propertyInfo.PropertyType;

                    if (propertyType.IsNullable())
                        propertyType = Nullable.GetUnderlyingType(propertyType);

                    dataTable.Columns.Add(colHeaderName, propertyType);
                }
            }
            return dataTable;
        }

    }
}
