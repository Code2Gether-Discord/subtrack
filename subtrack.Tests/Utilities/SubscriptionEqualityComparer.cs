using subtrack.DAL.Entities;
using System.Diagnostics.CodeAnalysis;

namespace subtrack.Tests.Utilities
{
    internal class SubscriptionEqualityComparer : IEqualityComparer<Subscription>
    {
        public bool Equals(Subscription? x, Subscription? y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            return x.Id == y.Id && 
                   x.Name == y.Name && 
                   x.Description == y.Description && 
                   x.IsAutoPaid == y.IsAutoPaid && 
                   x.Cost == y.Cost &&
                   x.FirstPaymentDay == y.FirstPaymentDay &&
                   x.LastPayment == y.LastPayment &&
                   x.BillingOccurrence == y.BillingOccurrence &&
                   x.BillingInterval == y.BillingInterval;
        }

        public int GetHashCode([DisallowNull] Subscription obj)
        {
            return obj.Id.GetHashCode() + 
                   obj.Name.GetHashCode() + 
                   (obj.Description?.GetHashCode() ?? 0) + obj.IsAutoPaid.GetHashCode() + 
                   obj.Cost.GetHashCode() +
                   obj.FirstPaymentDay.GetHashCode() + 
                   obj.LastPayment.GetHashCode() + 
                   obj.BillingOccurrence.GetHashCode() +
                   obj.BillingInterval.GetHashCode();
        }
    }
}
