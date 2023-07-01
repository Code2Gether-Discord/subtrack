using Microsoft.EntityFrameworkCore;
using subtrack.DAL;
using subtrack.DAL.Entities;
using subtrack.MAUI.Exceptions;
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

        public async Task Delete(int id)
        {
            var sub = await _context.Subscriptions.FindAsync(id);

            if (sub == null) throw new NotFoundException($"Subscription with an id:{id} not found.");

            _context.Subscriptions.Remove(sub);
            await _context.SaveChangesAsync();
        }
    }
}
