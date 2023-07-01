using Microsoft.EntityFrameworkCore;
using subtrack.DAL;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services.Abstractions;

namespace subtrack.MAUI.Services
{
    internal class SubscriptionService : ISubscriptionService
    {
        private readonly SubtrackDbContext _context;
        public SubscriptionService(SubtrackDbContext context) => _context = context;

        public async Task<IEnumerable<Subscription>> GetSubscriptions()
        {
            return await _context.Subscriptions.ToListAsync();
        }

        public async Task Update(Subscription subscriptionToUpdate)
        {
            var sub = await _context.Subscriptions.FindAsync(subscriptionToUpdate.Id);

            if (sub != null)
            {
                sub.LastPayment = subscriptionToUpdate.LastPayment.Date;
                sub.Name = subscriptionToUpdate.Name;
                sub.Description = subscriptionToUpdate.Description;
                sub.IsAutoPaid = subscriptionToUpdate.IsAutoPaid;
                sub.Cost = subscriptionToUpdate.Cost;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }
    }
}
