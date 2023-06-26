using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services;

public static class SubscriptionsCalculator
{
    public static decimal GetMonthlyCost(IEnumerable<Subscription> subscriptions)
    {
        if (subscriptions is null || !subscriptions.Any()) return 0;
        var price = subscriptions.Sum(x => x.Cost);
        return price;
    }
}
