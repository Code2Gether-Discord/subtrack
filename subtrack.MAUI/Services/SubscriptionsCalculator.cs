using subtrack.DAL.Entities;
using subtrack.MAUI.DateAndTimeProvider;

namespace subtrack.MAUI.Services;

public class SubscriptionsCalculator
{
    private const int _monthsInAYear = 12;

    private readonly IDateTimeProvider _dateTimeProvider;

    public SubscriptionsCalculator(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public static decimal GetTotalCost(IEnumerable<Subscription> subscriptions)
    {
        if (subscriptions is null || !subscriptions.Any())
            return 0;

        var price = subscriptions.Sum(x => x.Cost);
        return price;
    }

    public static IEnumerable<Subscription> GetSubscriptionListByMonth(IEnumerable<Subscription> subscriptions, int month)
    {
        var subscriptionsByMonth = subscriptions
                    .Where(s => s.LastPayment.Month != month)
                    .ToList();

        return subscriptionsByMonth;
    }

    public int GetDueDays(Subscription subscription)
    {
        if (subscription is null)
            throw new ArgumentNullException(nameof(subscription));

        var dueDate = subscription.LastPayment.AddMonths(1).Date;
        var duration = dueDate.Subtract(_dateTimeProvider.Now.Date);
        return duration.Days;
    }

    public static decimal GetYearlyCost(Subscription subscription)
    {
        if (subscription is null)
            throw new ArgumentNullException(nameof(subscription));

        return subscription.Cost * _monthsInAYear;
    }
}
