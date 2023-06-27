using subtrack.DAL.Entities;
using subtrack.MAUI.Response;

namespace subtrack.MAUI.Mappers
{
    public static class SubscriptionMapper
    {
        public static IEnumerable<SubscriptionResponse> ToResponses(this IEnumerable<Subscription> subscriptions)
        {
            var subscriptionResponse = subscriptions.Select(subscription => new SubscriptionResponse
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Description = subscription?.Description,
                IsAutoPaid = subscription.IsAutoPaid,
                Cost = subscription.Cost,
                LastPayment = subscription.LastPayment
            }).ToList();

            return subscriptionResponse;
        }
    }
}
