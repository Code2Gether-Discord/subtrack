using Microsoft.EntityFrameworkCore;
using subtrack.DAL;
using subtrack.DAL.Entities;
using subtrack.MAUI.Exceptions;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Utilities;
using System;

namespace subtrack.MAUI.Services
{
    internal class SubscriptionService : ISubscriptionService
    {
        private readonly SubtrackDbContext _context;
        public SubscriptionService(SubtrackDbContext context) => _context = context;

        public async Task<IEnumerable<Subscription>> GetAllAsync(GetSubscriptionsFilter? filter = null)
        {
            if (filter is not null) return await _context.Subscriptions
                    .WhereIf(sub => sub.IsAutoPaid == filter.AutoPaid, filter.AutoPaid is not null)
                    .ToListAsync();

            return await _context.Subscriptions.ToListAsync();
        }

        public async Task Update(Subscription subscriptionToUpdate)
        {
            var sub = await _context.Subscriptions.FindAsync(subscriptionToUpdate.Id);

            if (sub == null) throw new NotFoundException($"Subscription with id: {subscriptionToUpdate.Id} not found.");

            if (sub.LastPayment.Date != subscriptionToUpdate.LastPayment.Date)
            {
                sub.FirstPaymentDay = subscriptionToUpdate.LastPayment.Day;
                sub.LastPayment = subscriptionToUpdate.LastPayment.Date;
            }

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
            subscriptionToCreate.FirstPaymentDay = subscriptionToCreate.LastPayment.Day;
            await _context.Subscriptions.AddAsync(subscriptionToCreate);
            await _context.SaveChangesAsync();
            return subscriptionToCreate;
        }
        public async Task<DateTime> UpdateLastPaymentDateAsync(int subscriptionId, DateTime newLastPaymentDate)
        {
            var sub = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == subscriptionId);

            if (sub is  null) throw new NotFoundException($"Subscription with an id:{subscriptionId} not found.");

            sub.LastPayment = newLastPaymentDate;
            await _context.SaveChangesAsync();

            return newLastPaymentDate;
        }
    }
}
