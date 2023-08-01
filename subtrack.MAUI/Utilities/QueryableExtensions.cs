using System.Linq.Expressions;

namespace subtrack.MAUI.Utilities;

public static class QueryableExtensions
{
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, Expression<Func<TSource,bool>>? filter, bool shouldFilter)
    {
        if(filter is not null && shouldFilter) source.Where(filter);
        return source;
    }
}
