using subtrack.DAL.Entities;
using subtrack.MAUI.Responses;
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

    public decimal GetYearlyCost(Subscription subscription)
    {
        if (subscription is null)
            throw new ArgumentNullException(nameof(subscription));
        var weeksInYear = ISOWeek.GetWeeksInYear(_dateProvider.Today.Year);
        var yearlyPaymentsCount = (subscription.BillingOccurrence, subscription.BillingInterval) switch
        {
            (BillingOccurrence.Month, var monthlyInterval) when monthlyInterval <= _monthsInAYear => _monthsInAYear / monthlyInterval,
            (BillingOccurrence.Week, var weeklyInterval) when weeklyInterval <= weeksInYear => weeksInYear / weeklyInterval,
            _ => throw new NotImplementedException("Only billing Cycles within a year are supported")
        };
        return subscription.Cost * yearlyPaymentsCount;
    }

    private IEnumerable<Subscription> GetPaymentsUntilMonth(Subscription subscription, DateTime fromIncludedDate, DateTime toIncludedDate)
    {
        subscription.LastPayment = GetNextPaymentDate(subscription);

        while (subscription.LastPayment.Date >= fromIncludedDate.Date
                && subscription.LastPayment.Date <= toIncludedDate.Date)
        {
            yield return (Subscription)subscription.Clone();
            subscription.LastPayment = GetNextPaymentDate(subscription);
        }
    }

    public IEnumerable<SubscriptionsMonthResponse> GetMonthlySubscriptionLists(IEnumerable<Subscription> subscriptions, DateTime fromIncludedMonthDate, DateTime finalIncludedMonthDate)
    {
        return subscriptions
             .SelectMany(s => GetPaymentsUntilMonth(s, fromIncludedMonthDate, finalIncludedMonthDate))
             .GroupBy(s => (s.LastPayment.Year, s.LastPayment.Month))
             .Select(g =>
                  new SubscriptionsMonthResponse
                  {
                      MonthDate = new DateTime(g.Key.Year, g.Key.Month, 1),
                      Subscriptions = g.OrderBy(s => s.LastPayment).ToList(),
                      Cost = GetTotalCost(g)
                  }).ToList();
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
        var yearlyPaymentsCount = totalDuration / subscription.BillingInterval;
        return yearlyPaymentsCount * subscription.Cost / _monthsInAYear;
    }

    public decimal GetAverageMonthlyCost(IEnumerable<Subscription> subscriptions)
    {
        return subscriptions.Aggregate(0.0M, (acc, curr) => acc + GetAverageMonthlyCost(curr));
    }
}
