using subtrack.DAL.Entities;
using System.Linq.Expressions;

namespace subtrack.MAUI.Utilities;

public class GetSubscriptionsFilter
{
	public Expression<Func<Subscription, bool>> _autoPaidFilter = sub => true;

    public GetSubscriptionsFilter(bool useAutoPaidFilter)
    {
        if (useAutoPaidFilter) _autoPaidFilter = sub => sub.IsAutoPaid;
    }

    public Expression<Func<Subscription, bool>> GetAutoPaidFilter() { return _autoPaidFilter; }
}
