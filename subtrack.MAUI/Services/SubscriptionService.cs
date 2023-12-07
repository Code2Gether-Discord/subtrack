using Microsoft.EntityFrameworkCore;
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
        private readonly ISubscriptionsCalculator _subscriptionsCalculator;

        public SubscriptionService(SubtrackDbContext context, ISubscriptionsCalculator subscriptionsCalculator)
        {
            _context = context;
            _subscriptionsCalculator = subscriptionsCalculator;
        }

        public async Task<IEnumerable<Subscription>> GetAllAsync(GetSubscriptionsFilter? filter = null)
        {
            var query = _context.Subscriptions.AsNoTracking();
            if (filter is not null)
                query = query.WhereIf(sub => sub.IsAutoPaid == filter.AutoPaid, filter.AutoPaid is not null);

            return await query.ToListAsync();
        }

        public async Task Update(Subscription subscriptionToUpdate)
        {
            var sub = await GetByIdAsync(subscriptionToUpdate.Id);

            if (sub.LastPayment.Date != subscriptionToUpdate.LastPayment.Date)
            {
                sub.FirstPaymentDay = subscriptionToUpdate.LastPayment.Day;
                sub.LastPayment = subscriptionToUpdate.LastPayment.Date;
            }

            sub.Name = subscriptionToUpdate.Name;
            sub.Description = subscriptionToUpdate.Description;
            sub.IsAutoPaid = subscriptionToUpdate.IsAutoPaid;
            sub.BillingOccurrence = subscriptionToUpdate.BillingOccurrence;
            sub.BillingInterval = subscriptionToUpdate.BillingInterval;
            sub.PrimaryColor = subscriptionToUpdate.PrimaryColor;
            sub.SecondaryColor = subscriptionToUpdate.SecondaryColor;
            sub.Icon = subscriptionToUpdate.Icon;
            AutoPay(sub);
            sub.Cost = subscriptionToUpdate.Cost;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var sub = await GetByIdAsync(id);
            _context.Subscriptions.Remove(sub);
            await _context.SaveChangesAsync();
        }

        public async Task<Subscription?> GetByIdIfExists(int id)
        {
            return await _context.Subscriptions.AsNoTracking()
                                                  .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Subscription> CreateSubscriptionAsync(Subscription subscriptionToCreate)
        {
            SetupNewSubscription(subscriptionToCreate);
            await _context.Subscriptions.AddAsync(subscriptionToCreate);
            await _context.SaveChangesAsync();
            return subscriptionToCreate;
        }

        public async Task<IReadOnlyCollection<Subscription>> CreateSubscriptionsAsync(IEnumerable<Subscription> subscriptionsToCreate)
        {
            var newSubscriptions = subscriptionsToCreate.Select(s =>
            {
                SetupNewSubscription(s);
                return s;
            }).ToArray();

            await _context.Subscriptions.AddRangeAsync(newSubscriptions);
            await _context.SaveChangesAsync();

            return newSubscriptions;
        }

        public async Task<Subscription> MarkNextPaymentAsPaidAsync(int subscriptionId)
        {
            var sub = await GetByIdAsync(subscriptionId);
            sub.LastPayment = _subscriptionsCalculator.GetNextPaymentDate(sub);
            await _context.SaveChangesAsync();

            return sub;
        }

        public async Task<Subscription> AutoPayAsync(int subscriptionId)
        {
            var subscription = await GetByIdAsync(subscriptionId);
            AutoPay(subscription);
            await _context.SaveChangesAsync();

            return subscription;
        }

        private void SetupNewSubscription(Subscription subscriptionToCreate)
        {
            subscriptionToCreate.LastPayment = subscriptionToCreate.LastPayment.Date;
            subscriptionToCreate.FirstPaymentDay = subscriptionToCreate.LastPayment.Day;
            AutoPay(subscriptionToCreate);
        }
        private void AutoPay(Subscription subscription)
        {
            if (!subscription.IsAutoPaid)
                return;

            var (isDue, dueDate) = _subscriptionsCalculator.IsDue(subscription);
            if (!isDue)
                return;

            subscription.LastPayment = dueDate;
            AutoPay(subscription);
        }

        private async Task<Subscription> GetByIdAsync(int subscriptionId)
        {
            return await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == subscriptionId)
               ?? throw new NotFoundException($"Subscription with an id:{subscriptionId} not found.");
        }
    }
}