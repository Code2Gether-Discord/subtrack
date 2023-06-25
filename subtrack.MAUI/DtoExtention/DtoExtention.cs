using subtrack.DAL.Entities;
using subtrack.MAUI.Responses;
using subtrack.MAUI.Services;

namespace subtrack.MAUI.DtoExtention
{
    internal static class DtoExtention
    {
        public static IEnumerable<SubscriptionResponse> ToSubscriptionResponses(this IEnumerable<Subscription> subscriptions)
        {
            return subscriptions.Select(sub => new SubscriptionResponse
            {
                Subscription = sub,
                DueDays = sub.GetDueDays() // Replace with your actual logic to calculate due days
            }) ;
        }
    }
}
