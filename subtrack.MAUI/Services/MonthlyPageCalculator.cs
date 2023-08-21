using subtrack.DAL.Entities;
using subtrack.MAUI.Responses;
using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Services
{
    internal class MonthlyPageCalculator : IMonthlyPageCalculator
    {
        private readonly ISubscriptionsCalculator _subscriptionsCalculator;
        private readonly ISubscriptionService _subscriptionService;

        public MonthlyPageCalculator(ISubscriptionsCalculator subscriptionsCalculator, ISubscriptionService subscriptionService)
        {
            _subscriptionsCalculator = subscriptionsCalculator;
            _subscriptionService = subscriptionService;
        }

        private IEnumerable<Subscription> GetPaymentsUntilMonth(Subscription subscription, DateTime fromIncludedDate, DateTime toIncludedDate)
        {
            subscription.LastPayment = _subscriptionsCalculator.GetNextPaymentDate(subscription);

            while (subscription.LastPayment.Date >= fromIncludedDate.Date
                    && subscription.LastPayment.Date <= toIncludedDate.Date)
            {
                yield return (Subscription)subscription.Clone();
                subscription.LastPayment = _subscriptionsCalculator.GetNextPaymentDate(subscription);
            }
        }

        public async Task<IDictionary<int, List<SubscriptionsMonthResponse>>> GetMonthlySubscriptionLists(DateTime fromIncludedMonthDate, DateTime finalIncludedMonthDate)
        {
            var subscriptions = await _subscriptionService.GetAllAsync();
            return subscriptions
                 .SelectMany(s => GetPaymentsUntilMonth(s, fromIncludedMonthDate, finalIncludedMonthDate))
                 .GroupBy(s => (s.LastPayment.Year, s.LastPayment.Month))
                 .Select(g =>
                      new SubscriptionsMonthResponse
                      {
                          MonthDate = new DateTime(g.Key.Year, g.Key.Month, 1),
                          Subscriptions = g.OrderBy(s => s.LastPayment).ToList(),
                          Cost = _subscriptionsCalculator.GetTotalCost(g)
                      }).GroupBy(r => r.MonthDate.Year).ToDictionary(k => k.Key, v => v.ToList());
        }
    }
}
