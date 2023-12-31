﻿using subtrack.DAL.Entities;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Utilities;
using System.Globalization;

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

    public decimal GetYearlyAverageCost(Subscription subscription)
    {
        return GetAverageMonthlyCost(subscription) * 12;
    }

    public DateTime GetNextPaymentDate(Subscription subscription)
    {
        int startDay = subscription.FirstPaymentDay;
        var lastPayment = subscription.LastPayment;
        switch (subscription.BillingOccurrence)
        {
            case BillingOccurrence.Week:
                var nextWeekDate = lastPayment.AddWeeks(subscription.BillingInterval);
                return new DateTime(nextWeekDate.Year, nextWeekDate.Month, nextWeekDate.Day);
            case BillingOccurrence.Month:
                var nextMonthDate = lastPayment.AddMonths(subscription.BillingInterval);
                var nextMonthTotalDays = DateTime.DaysInMonth(nextMonthDate.Year, nextMonthDate.Month);

                if (startDay > nextMonthTotalDays)
                    return nextMonthDate;

                return new DateTime(nextMonthDate.Year, nextMonthDate.Month, startDay);
            case BillingOccurrence.Year:
                var nextYearDate = lastPayment.AddYears(subscription.BillingInterval);

                if (!DateTime.IsLeapYear(nextYearDate.Year))
                    return nextYearDate;
                return new DateTime(nextYearDate.Year, nextYearDate.Month, startDay);
            default:
                throw new ArgumentOutOfRangeException(subscription.BillingOccurrence.ToString());
        }
    }

    public (bool IsDue, DateTime NextPaymentDate) IsDue(Subscription subscription)
    {
        var nextPaymentDate = GetNextPaymentDate(subscription);
        return (nextPaymentDate <= _dateProvider.Today, nextPaymentDate);
    }

    private decimal GetAverageMonthlyCost(Subscription subscription)
    {
        if (subscription is null)
            throw new ArgumentNullException(nameof(subscription));
        var weeksInYear = ISOWeek.GetWeeksInYear(_dateProvider.Today.Year);
        var totalDuration = subscription.BillingOccurrence switch
        {
            BillingOccurrence.Month => _monthsInAYear,
            BillingOccurrence.Week => weeksInYear,
            BillingOccurrence.Year => 1,
            _ => throw new ArgumentOutOfRangeException($"Billing occurrence ${subscription.BillingOccurrence} is not supported in GetAverageMonthlyCost")
        };

        var yearlyPaymentsCount = (decimal)totalDuration / subscription.BillingInterval;
        return yearlyPaymentsCount * subscription.Cost / _monthsInAYear;
    }

    public decimal GetMonthlyAverageCost(IEnumerable<Subscription> subscriptions)
    {
        return subscriptions.Select(GetAverageMonthlyCost).Sum();
    }
}
