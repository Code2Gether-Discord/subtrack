using subtrack.DAL.Entities;

namespace subtrack.MAUI.Services.Abstractions
{
    internal interface ISubscriptionsService
    {
        Task<IEnumerable<Subscription>> GetSubscriptions();
    }
}
