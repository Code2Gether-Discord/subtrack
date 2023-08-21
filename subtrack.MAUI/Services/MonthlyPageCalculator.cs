using subtrack.DAL.Entities;
using subtrack.MAUI.Responses;
using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Services
{
    internal class MonthlyPageCalculator : IMonthlyPageCalculator
    {
        private readonly ISubscriptionsCalculator subscriptionsCalculator;
        private readonly ISubscriptionService subscriptionService;

        public MonthlyPageCalculator(ISubscriptionsCalculator subscriptionsCalculator, ISubscriptionService subscriptionService)
        {
            this.subscriptionsCalculator = subscriptionsCalculator;
            this.subscriptionService = subscriptionService;
        }

        private IEnumerable<Subscription> GetPaymentsUntilMonth(Subscription subscription, DateTime fromIncludedDate, DateTime toIncludedDate)
        {
            subscription.LastPayment = subscriptionsCalculator.GetNextPaymentDate(subscription);

            while (subscription.LastPayment.Date >= fromIncludedDate.Date
                    && subscription.LastPayment.Date <= toIncludedDate.Date)
            {
                yield return (Subscription)subscription.Clone();
                subscription.LastPayment = subscriptionsCalculator.GetNextPaymentDate(subscription);
            }
        }

        public async Task<IEnumerable<IGrouping<int, SubscriptionsMonthResponse>>> GetMonthlySubscriptionLists(DateTime fromIncludedMonthDate, DateTime finalIncludedMonthDate)
        {
            var subscriptions = await subscriptionService.GetAllAsync();
            return subscriptions
                 .SelectMany(s => GetPaymentsUntilMonth(s, fromIncludedMonthDate, finalIncludedMonthDate))
                 .GroupBy(s => (s.LastPayment.Year, s.LastPayment.Month))
                 .Select(g =>
                      new SubscriptionsMonthResponse
                      {
                          MonthDate = new DateTime(g.Key.Year, g.Key.Month, 1),
                          Subscriptions = g.OrderBy(s => s.LastPayment).ToList(),
                          Cost = subscriptionsCalculator.GetTotalCost(g)
                      }).GroupBy(r => r.MonthDate.Year);
        }
    }
}
