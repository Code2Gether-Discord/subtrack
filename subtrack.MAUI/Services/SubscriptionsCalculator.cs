using subtrack.DAL.Entities;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Utilities;

namespace subtrack.MAUI.Services;

public class SubscriptionsCalculator : ISubscriptionsCalculator
{
    private const int _monthsInAYear = 12;

    private readonly IDateProvider _dateProvider;

    public SubscriptionsCalculator(IDateProvider dateProvider)
    {
        _dateProvider = dateProvider;
    }

    public int GetDueDays(Subscription subscription)
    {
        if (subscription is null)
            throw new ArgumentNullException(nameof(subscription));

        var dueDate = GetNextPaymentDate(subscription);
        var duration = dueDate.TimeRemainingFromToday(_dateProvider);
        return duration.Days;
    }

    public decimal GetTotalCost(IEnumerable<Subscription> subscriptions)
    {
        if (subscriptions is null || !subscriptions.Any())
            return 0;

        var price = subscriptions.Sum(x => x.Cost);
        return price;
    }

    public decimal GetYearlyCost(Subscription subscription)
    {
        if (subscription is null)
            throw new ArgumentNullException(nameof(subscription));

        return subscription.Cost * _monthsInAYear;
    }

    public IEnumerable<Subscription> GetSubscriptionListByMonth(IEnumerable<Subscription> subscriptions, DateTime monthDate)
    {
        bool paidThisMonth(Subscription s) => s.LastPayment.Month == monthDate.Month && s.LastPayment.Year == monthDate.Year;
        bool paymentStartsInTheFuture(Subscription s) => s.LastPayment > monthDate;

        var subscriptionsByMonth = subscriptions
                    .Where(s => !paidThisMonth(s) && !paymentStartsInTheFuture(s))
                    .ToList();

        return subscriptionsByMonth;
    }

    public DateTime GetNextPaymentDate(Subscription subscription)
    {
        int startDay = subscription.FirstPaymentDay;
        var lastPayment = subscription.LastPayment;

        var nextMonthDate = lastPayment.AddMonths(1);
        var nextMonthTotalDays = DateTime.DaysInMonth(nextMonthDate.Year, nextMonthDate.Month);

        if (startDay > nextMonthTotalDays)
            return nextMonthDate;

        return new DateTime(nextMonthDate.Year, nextMonthDate.Month, startDay);
    }

    public (bool IsDue, DateTime NextPaymentDate) IsDue(Subscription subscription)
    {
        var nextPaymentDate = GetNextPaymentDate(subscription);
        return (nextPaymentDate <= _dateProvider.Today, nextPaymentDate);
    }
}
