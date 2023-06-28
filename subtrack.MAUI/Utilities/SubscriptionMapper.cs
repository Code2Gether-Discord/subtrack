using subtrack.DAL.Entities;
using subtrack.MAUI.Responses;
using subtrack.MAUI.Services;

namespace subtrack.MAUI.Utilities
{
    public static class SubscriptionMapper
    {
        public static IEnumerable<SubscriptionResponse> ToResponses(this IEnumerable<Subscription> subscriptions)
        {
            return subscriptions.Select(sub => new SubscriptionResponse
            {
                Id = sub.Id,
                Name = sub.Name,
                Description = sub?.Description,
                IsAutoPaid = sub.IsAutoPaid,
                Cost = sub.Cost,
                LastPayment = sub.LastPayment,
                Subscription = sub,
                DueDays = SubscriptionsCalculator.GetDueDays(sub)
            });
        }
    }
}
