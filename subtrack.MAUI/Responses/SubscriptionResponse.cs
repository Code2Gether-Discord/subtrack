using subtrack.DAL.Entities;

namespace subtrack.MAUI.Responses
{
    public class SubscriptionResponse
    {
        public Subscription Subscription { get; set; }
        public int DueDays { get; set; }

    }
}
