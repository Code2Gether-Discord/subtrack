using subtrack.DAL.Entities;
using subtrack.MAUI.Data;

namespace subtrack.MAUI.Response
{
    public class SubscriptionResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public bool IsAutoPaid { get; set; }
        public decimal Cost { get; set; }
        public DateTime LastPayment { get; set; }
    }
}
