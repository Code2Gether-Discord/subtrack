﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace subtrack.DAL.Entities;

[DebuggerDisplay("Name = {Name} LastPayment = {LastPayment} BillingOccurrence = {BillingOccurrence}")]
public class Subscription : ICloneable
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Service name should be less than 50 characters.")]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsAutoPaid { get; set; }

    public decimal Cost { get; set; }

    [Required]
    public int FirstPaymentDay { get; set; }
    [Required]
    public DateTime LastPayment { get; set; }

    public BillingOccurrence BillingOccurrence { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Interval has to be greater than 0")]
    public int BillingInterval { get; set; }

    public string PrimaryColor { get; set; }

    public string? Icon { get; set; }

    public string SecondaryColor { get; set; }

    [Range(0, 31, ErrorMessage = "Notification can be set up to 31 days before due date")]
    public int? NotificationDays { get; set; }

    public object Clone()
    {
        return (Subscription)MemberwiseClone();
    }
}
