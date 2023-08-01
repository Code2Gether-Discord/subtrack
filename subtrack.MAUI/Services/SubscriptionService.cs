﻿using Microsoft.EntityFrameworkCore;
using subtrack.DAL;
using subtrack.DAL.Entities;
using subtrack.MAUI.Exceptions;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Utilities;

namespace subtrack.MAUI.Services
{
    internal class SubscriptionService : ISubscriptionService
    {
        private readonly SubtrackDbContext _context;
        public SubscriptionService(SubtrackDbContext context) => _context = context;

        public async Task<IEnumerable<Subscription>> GetSubscriptions(GetSubscriptionsFilter? filter = null)
        {
            bool shouldFilter = filter != null;

            return await _context.Subscriptions.WhereIf(filter?.GetAutoPaidFilter(), shouldFilter).ToListAsync();
        }

        public async Task Update(Subscription subscriptionToUpdate)
        {
            var sub = await _context.Subscriptions.FindAsync(subscriptionToUpdate.Id);

            if (sub == null) throw new NotFoundException($"Subscription with id: {subscriptionToUpdate.Id} not found.");

            sub.LastPayment = subscriptionToUpdate.LastPayment.Date;
            sub.Name = subscriptionToUpdate.Name;
            sub.Description = subscriptionToUpdate.Description;
            sub.IsAutoPaid = subscriptionToUpdate.IsAutoPaid;
            sub.Cost = subscriptionToUpdate.Cost;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var sub = await _context.Subscriptions.FindAsync(id);

            if (sub == null) throw new NotFoundException($"Subscription with an id:{id} not found.");

            _context.Subscriptions.Remove(sub);
            await _context.SaveChangesAsync();
        }

        public async Task<Subscription?> GetById(int id)
        {
            var sub = await _context.Subscriptions.AsNoTracking()
                                                  .FirstOrDefaultAsync(s => s.Id == id);

            return sub;
        }

        public async Task<Subscription> CreateSubscriptionAsync(Subscription subscriptionToCreate)
        {
            subscriptionToCreate.LastPayment = subscriptionToCreate.LastPayment.Date;
            await _context.Subscriptions.AddAsync(subscriptionToCreate);
            await _context.SaveChangesAsync();
            return subscriptionToCreate;
        }
    }
}
