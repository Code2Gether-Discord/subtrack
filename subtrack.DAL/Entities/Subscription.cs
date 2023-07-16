using System.ComponentModel.DataAnnotations;

namespace subtrack.DAL.Entities;

public class Subscription
{
    public int Id { get; set; }

    [Required]
    [StringLength(20,ErrorMessage = "Service name should be less than 20 characters.")]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsAutoPaid { get; set; }

    public decimal Cost { get; set; }

    [Required]
    public DateTime LastPayment { get; set; }
}
