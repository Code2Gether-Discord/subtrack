using subtrack.DAL.Entities;

namespace subtrack.MAUI.Responses
{
    public class SubscriptionResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public bool IsAutoPaid { get; set; }
        public decimal Cost { get; set; }
        public DateTime LastPayment { get; set; }
        public int DueDays { get; set; }

    }
}
