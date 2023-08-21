using subtrack.MAUI.Responses;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface IMonthlyPageCalculator
    {
        /// <summary>
        /// Fetches all subscriptions that are active between the given dates grouped by month and year
        /// </summary>
        /// <param name="fromIncludedDate">Starting date for fetching the subscriptions</param>
        /// <param name="toIncludedDate">Ending date for fetching the subscriptions</param>
        /// <returns>A <see cref="IDictionary{int, List<SubscriptionsMonthResponse>}"/> object where key is the year and value is the list of subscriptions grouped by month for that year</returns>
        Task<IDictionary<int, List<SubscriptionsMonthResponse>>> GetMonthlySubscriptionLists(DateTime fromIncludedDate, DateTime toIncludedDate);
    }
}
