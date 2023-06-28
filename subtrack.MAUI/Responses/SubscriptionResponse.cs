using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using System.Runtime.CompilerServices;

namespace subtrack.MAUI.Responses
{
    public class SubscriptionResponse
    {
        public Subscription Subscription { get; set; }
        public int DueDays { get; set; }

    }
}
