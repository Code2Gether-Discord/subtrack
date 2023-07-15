using subtrack.DAL.Entities;
using subtrack.MAUI.Responses;
using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Utilities
{
    public static class SubscriptionMapper
    {
        public static IEnumerable<SubscriptionResponse> ToResponses(this IEnumerable<Subscription> subscriptions, ISubscriptionsCalculator subscriptionsCalculator)
        {
            return subscriptions.Select(sub => new SubscriptionResponse
            {
                Id = sub.Id,
                Name = sub.Name,
                Description = sub.Description ?? string.Empty,
                IsAutoPaid = sub.IsAutoPaid,
                Cost = sub.Cost,
                LastPayment = sub.LastPayment,
                DueDays = subscriptionsCalculator.GetDueDays(sub)
            });
        }
    }
}
