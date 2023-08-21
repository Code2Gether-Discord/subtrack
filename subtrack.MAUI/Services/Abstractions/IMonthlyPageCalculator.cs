using subtrack.DAL.Entities;
using subtrack.MAUI.Responses;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface IMonthlyPageCalculator
    {
        Task<IEnumerable<IGrouping<int, SubscriptionsMonthResponse>>> GetMonthlySubscriptionLists(DateTime fromIncludedDate, DateTime toIncludedDate);
    }
}
