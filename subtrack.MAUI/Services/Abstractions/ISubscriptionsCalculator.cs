using subtrack.DAL.Entities;
using subtrack.MAUI.Responses;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface ISubscriptionsCalculator
    {
        int GetDueDays(Subscription subscription);
        decimal GetTotalCost(IEnumerable<Subscription> subscriptions);
        decimal GetYearlyCost(Subscription subscription);

        /// <summary>
        /// Gets lists of due subs for each month up until-including a specific month date
        /// </summary>
        /// <param name="subscriptions"></param>
        /// <param name="finalIncludedMonthDate"></param>
        /// <returns>Subscriptions due for a specific Month Date</returns>
        IEnumerable<SubscriptionsMonthResponse> GetMonthlySubscriptionLists(IEnumerable<Subscription> subscriptions, DateTime fromIncludedMonthDate, DateTime finalIncludedMonthDate);
        DateTime GetNextPaymentDate(Subscription subscription);
        (bool IsDue, DateTime NextPaymentDate) IsDue(Subscription subscription);
    }
}
