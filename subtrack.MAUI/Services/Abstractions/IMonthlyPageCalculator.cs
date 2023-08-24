using subtrack.DAL.Entities;
using subtrack.MAUI.Responses;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface IMonthlyPageCalculator
    {
        /// <summary>
        /// Fetches all subscriptions that are active between the given dates grouped by month and year
        /// </summary>
        /// <param name="subscriptions"></param>
        /// <param name="fromIncludedDate">Starting date for fetching the subscriptions</param>
        /// <param name="toIncludedDate">Ending date for fetching the subscriptions</param>
        /// <returns>A dictionary where key is the year and value is the list of subscriptions grouped by month for that year</returns>
        IDictionary<int, List<SubscriptionsMonthResponse>> GetMonthlySubscriptionLists(IEnumerable<Subscription> subscriptions, DateTime fromIncludedDate, DateTime toIncludedDate);
    }
}
