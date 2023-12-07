using subtrack.DAL.Entities;
using subtrack.MAUI.Utilities;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<Subscription>> GetAllAsync(GetSubscriptionsFilter? filter = null);
        Task<Subscription> CreateSubscriptionAsync(Subscription subscriptionToCreate);
        Task<IReadOnlyCollection<Subscription>> CreateSubscriptionsAsync(IEnumerable<Subscription> subscriptionsToCreate);
        Task<Subscription?> GetByIdIfExists(int id);
        Task Delete(int id);
        Task Update(Subscription subscriptionToUpdate);
        Task<Subscription> AutoPayAsync(int subscriptionId);
        Task<Subscription> MarkNextPaymentAsPaidAsync(int subscriptionId);
    }
}
