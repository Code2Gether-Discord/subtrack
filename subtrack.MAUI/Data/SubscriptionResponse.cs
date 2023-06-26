using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtrack.MAUI.Data
{
    public class SubscriptionResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsAutoPaid { get; set; }
        public decimal Cost { get; set; }
        public DateTime LastPayment { get; set; }
    }
}
