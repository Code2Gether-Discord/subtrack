﻿using subtrack.DAL.Entities;
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

        var nextPaymentDate = subscription.LastPayment.AddMonths(1).Date;
        var lastDayOfMonth = DateTime.DaysInMonth(subscription.LastPayment.Year, subscription.LastPayment.Month);

        if (subscription.LastPayment.Day != lastDayOfMonth)
            return nextPaymentDate;

        if (subscription.LastPayment.Month == 1)
        {
            var isLeapYear = DateTime.IsLeapYear(subscription.LastPayment.Year);
            return new DateTime(subscription.LastPayment.Year,  2 , isLeapYear ? 29 : 28);
        }

        if (subscription.LastPayment.Month == 2)
            return new DateTime(subscription.LastPayment.Year, 3, 31);

        if (lastDayOfMonth % 2 == 0)
            return nextPaymentDate.AddDays(1);

        return nextPaymentDate;

    }
}
