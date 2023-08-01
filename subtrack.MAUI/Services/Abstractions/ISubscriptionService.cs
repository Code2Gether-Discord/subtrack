using subtrack.DAL.Entities;
using subtrack.MAUI.Utilities;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<Subscription>> GetSubscriptions(GetSubscriptionsFilter? filter = null);
        Task<Subscription> CreateSubscriptionAsync(Subscription subscriptionToCreate);
        Task<Subscription?> GetById(int id);
        Task Delete(int id);
        Task Update(Subscription subscriptionToUpdate);
    }
}
