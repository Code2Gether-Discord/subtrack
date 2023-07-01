namespace subtrack.MAUI.Responses
{
    public class SubscriptionsMonthResponse
    {
        public DateTime CurrentDate { get; set; }
        public decimal Cost { get; set; }
        public IEnumerable<SubscriptionResponse> SubscriptionResponses { get; set; }
    }
}
