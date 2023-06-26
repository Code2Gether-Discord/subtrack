using subtrack.MAUI.Services;
using subtrack.DAL.Entities;
using subtrack.MAUI.Data;

namespace subtrack.MAUI.Response
{
    public class SubscriptionsMonthResponse
    {
        public DateTime CurrentDate { get; set; }
        public IEnumerable<Subscription> Subscriptions { get; }
        public IEnumerable<SubscriptionResponse> SubscriptionResponse { get; }

        public SubscriptionsMonthResponse(DateTime dateTime)
        {
            CurrentDate = dateTime;
        }

        public IEnumerable<SubscriptionResponse> GetSubscriptionCostByMonth()
        {
            var subscriptionByMonth = SubscriptionsCalculator.GetSubscriptionListByMonth(Subscriptions, CurrentDate);
            var subscriptionViewModel = Map(subscriptionByMonth);

            return subscriptionViewModel;
        }

        public IEnumerable<SubscriptionResponse> Map(IEnumerable<Subscription> Subscriptions)
        {
            var subscriptionResponse = new List<SubscriptionResponse>();

            foreach (var subscription in Subscriptions)
            {
                var SubscriptionResponse = new SubscriptionResponse 
                {
                    Id = subscription.Id,
                    Name = subscription.Name,
                    Description = subscription?.Description,
                    IsAutoPaid = subscription.IsAutoPaid,
                    Cost = subscription.Cost,
                    LastPayment = subscription.LastPayment
                };

                subscriptionResponse.Add(SubscriptionResponse);
            }

            return subscriptionResponse;
        }
    }
}
