using subtrack.DAL.Entities;
using subtrack.MAUI.Data;

namespace subtrack.MAUI.Mappers
{
    public class SubscriptionMapper
    {
        public IEnumerable<SubscriptionResponse> Map(IEnumerable<Subscription> subscriptions)
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
