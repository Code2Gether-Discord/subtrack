using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface ISubscriptionsCalculator
    {
        int GetDueDays(Subscription subscription);

        DateTime GetNextPaymentDate(Subscription subscription);

        decimal GetTotalCost(IEnumerable<Subscription> subscriptions);

        decimal GetYearlyAverageCost(Subscription subscription);

        (bool IsDue, DateTime NextPaymentDate) IsDue(Subscription subscription);

        decimal GetMonthlyAverageCost(IEnumerable<Subscription> subscriptions);
    }
}