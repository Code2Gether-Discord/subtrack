using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services.Abstractions
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<Subscription>> GetSubscriptions();
        Task<Subscription> CreateSubscriptionAsync(Subscription subscriptionToCreate);
        Task<Subscription?> GetById(int id);
        Task Delete(int id);
        Task Update(Subscription subscriptionToUpdate);
    }
}
