using Microsoft.AspNetCore.Components.Forms;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Utilities;

namespace subtrack.MAUI.Services;

public class SubscriptionsCalculator : ISubscriptionsCalculator
{
    private const int _monthsInAYear = 12;

    private readonly IDateTimeProvider _dateTimeProvider;

    public SubscriptionsCalculator(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public int GetDueDays(Subscription subscription)
    {
        if (subscription is null)
            throw new ArgumentNullException(nameof(subscription));

        var dueDate = GetNextPaymentDate(subscription);
        var duration = dueDate.TimeRemainingFromToday(_dateTimeProvider);
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
        var nextPaymentDate = lastPayment.AddMonths(1).Date;

        int currentTotalDays = DateTime.DaysInMonth(lastPayment.Year, lastPayment.Month);
        int nextMonthTotalDays = DateTime.DaysInMonth(nextPaymentDate.Year, nextPaymentDate.Month);

        bool shouldIncrementDay = startDay == currentTotalDays && startDay < nextMonthTotalDays && nextPaymentDate.Month != 3;

        if (shouldIncrementDay)
            return nextPaymentDate.AddDays(1);


        int selectedDay = startDay <= nextMonthTotalDays ? startDay : nextPaymentDate.Day;
        return new DateTime(nextPaymentDate.Year, nextPaymentDate.Month, selectedDay);

    }
}
