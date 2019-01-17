using System;
using System.Linq;
using System.Linq.Expressions;

namespace RequestModel
{
    public static class FilterCollection
    {
        public static IQueryable<T> FilterBy<T>(this IQueryable<T> queryable, string id, Expression<Func<T, bool>> predicate)
        {
            if (!string.IsNullOrWhiteSpace(id) && id != new Guid().ToString())
            {
                queryable = queryable.Where(predicate);
            }
            return queryable;
        }
    }
}