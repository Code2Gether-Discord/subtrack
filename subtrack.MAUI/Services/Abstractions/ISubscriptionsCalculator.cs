using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface ISubscriptionsCalculator
    {
        int GetDueDays(Subscription subscription);
        decimal GetTotalCost(IEnumerable<Subscription> subscriptions);
        decimal GetYearlyCost(Subscription subscription);
        IEnumerable<Subscription> GetSubscriptionListByMonth(IEnumerable<Subscription> subscriptions, int month);
    }
}
