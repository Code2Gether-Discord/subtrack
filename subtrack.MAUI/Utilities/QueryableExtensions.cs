using System.Linq.Expressions;

namespace subtrack.MAUI.Utilities;

public static class QueryableExtensions
{
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>>? filter, bool shouldFilter)
    {
        return filter is not null && shouldFilter ? source.Where(filter) : source;
    }
}
