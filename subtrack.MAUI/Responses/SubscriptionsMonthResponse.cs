using subtrack.DAL.Entities;

namespace subtrack.MAUI.Responses
{
    public class SubscriptionsMonthResponse
    {
        public DateTime MonthDate { get; set; }
        public decimal Cost { get; set; }
        public IEnumerable<Subscription> Subscriptions { get; set; } = null!;
    }
}
