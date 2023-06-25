using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services;

public static class SubscriptionsCalculator
{
    public static decimal GetMonthlyCost(this IEnumerable<Subscription> subscriptions)
    {
        if (subscriptions is null || !subscriptions.Any()) return 0;
        var price = subscriptions.Sum(x => x.Cost);
        return price;
    }

    public static int GetDueDays(this Subscription subscription)
    {
        if (subscription is null) return 0;
        var duration = DateTime.Now.Subtract(subscription.LastPayment);
        if (duration.Days < 0) return 0;
        return duration.Days;
    }

}
