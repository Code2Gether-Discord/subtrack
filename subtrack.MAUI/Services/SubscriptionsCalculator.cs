using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services;

public static class SubscriptionsCalculator
{
    public static decimal GetMonthlyCost(IEnumerable<Subscription> subscriptions)
    {
        if (subscriptions is null || !subscriptions.Any()) return 0;
        var subscriptionsByMonth = GetSubscriptionListByMonth(subscriptions, DateTime.Now);
        var monthlyPrice = subscriptionsByMonth.Sum(x => x.Cost);
        return monthlyPrice;
    }

    public static List<Subscription> GetSubscriptionListByMonth(IEnumerable<Subscription> subscriptions, DateTime localDate)
    {
        var subscriptionsByMonth = subscriptions
                    .Where(s => s.LastPayment.Month != localDate.Month)
                    .ToList();
        return subscriptionsByMonth;
    }
}
