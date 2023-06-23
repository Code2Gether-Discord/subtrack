using System.ComponentModel.DataAnnotations;

namespace subtrack.DAL.Entities;

public class Subscription
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsAutoPaid { get; set; }

    public decimal Cost { get; set; }

    [Required]
    public DateTime LastPayment { get; set; }
}
