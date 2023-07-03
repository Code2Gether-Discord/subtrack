using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services.Abstractions
{
    internal interface ISubscriptionService
    {
        Task<IEnumerable<Subscription>> GetSubscriptions();
        Task<Subscription> CreateSubscriptionAsync(Subscription subscriptionToCreate);
    }
}
