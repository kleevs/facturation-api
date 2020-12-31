using System;
using System.Collections.Generic;

namespace FacturationApi.Linq2
{
    public static class IQueryableExtension 
    {
        public static IEnumerable<TResult> Select<TSource, TResult>(this System.Linq.IQueryable<TSource> source, Func<TSource, TResult> selector)
        {
            return System.Linq.Enumerable.Select(source, selector);
        }

        public static System.Linq.IQueryable<TSource> Where<TSource>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<Func<TSource, bool>> predicate) 
        {
            return System.Linq.Queryable.Where(source, predicate);
        }

        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.ToList(source);
        }
    }
}
