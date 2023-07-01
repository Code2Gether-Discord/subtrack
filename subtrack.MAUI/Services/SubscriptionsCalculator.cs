using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services;

public static class SubscriptionsCalculator
{
    public static decimal GetTotalPrice(IEnumerable<Subscription> subscriptions)
    {
        if (subscriptions is null || !subscriptions.Any()) return 0;
        var price = subscriptions.Sum(x => x.Cost);
        return price;
    }

    public static IEnumerable<Subscription> GetSubscriptionListByMonth(IEnumerable<Subscription> subscriptions, DateTime localDate)
    {
        var subscriptionsByMonth = subscriptions
                    .Where(s => s.LastPayment.Month != localDate.Month)
                    .ToList();
        return subscriptionsByMonth;
    }
  
    public static int GetDueDays(Subscription subscription)
    {
        if (subscription is null)
        {
            throw new ArgumentNullException(nameof(subscription));
        }
        var dueDate = subscription.LastPayment.AddMonths(1).Date;
        var duration = dueDate.Subtract(DateTime.Now.Date);
        return duration.Days;
    }
}
