using subtrack.DAL.Entities;
using subtrack.MAUI.DateAndTimeProvider;
using subtrack.MAUI.Responses;
using subtrack.MAUI.Services;

namespace subtrack.MAUI.Utilities
{
    public static class SubscriptionMapper
    {
        public static IEnumerable<SubscriptionResponse> ToResponses(this IEnumerable<Subscription> subscriptions, IDateTimeProvider date)
        {
            var calculator = new SubscriptionsCalculator(date);

            return subscriptions.Select(sub => new SubscriptionResponse
            {
                Id = sub.Id,
                Name = sub.Name,
                Description = sub?.Description,
                IsAutoPaid = sub.IsAutoPaid,
                Cost = sub.Cost,
                LastPayment = sub.LastPayment,
                DueDays = calculator.GetDueDays(sub)
            });
        }
    }
}
