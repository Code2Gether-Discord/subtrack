using subtrack.DAL.Entities;
using subtrack.MAUI.Responses;
using subtrack.MAUI.Services;

namespace subtrack.MAUI.Utilities
{
    public static class SubscriptionMapper
    {
        public static IEnumerable<SubscriptionResponse> GetSubscriptionResponses(IEnumerable<Subscription> subscriptions)
        {
            return subscriptions.Select(sub => new SubscriptionResponse
            {
                Subscription = sub,
                DueDays = SubscriptionsCalculator.GetDueDays(sub) // Replace with your actual logic to calculate due days
            });
        }
    }
}
