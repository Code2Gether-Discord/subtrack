using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface ISubscriptionsCalculator
    {
        int GetDueDays(Subscription subscription);

        decimal GetTotalCost(IEnumerable<Subscription> subscriptions);

        decimal GetYearlyCost(Subscription subscription);

        DateTime GetNextPaymentDate(Subscription subscription);

        (bool IsDue, DateTime NextPaymentDate) IsDue(Subscription subscription);

        decimal GetAverageMonthlyCost(IEnumerable<Subscription> subscriptions);
    }
}
