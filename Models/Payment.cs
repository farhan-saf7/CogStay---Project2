using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CogStayMVC.Models;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int? BillingId { get; set; }

    public int? ReservationId { get; set; }

    [Required]
    [MaxLength(100)]
    public string CardholderName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string CardNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(5)]
    public string ExpiryDate { get; set; } = string.Empty; // MM/YY

    [Required]
    [MaxLength(4)]
    public string CVV { get; set; } = string.Empty;

    [Required]
    [Precision(18, 2)]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Paid, Refunded

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(BillingId))]
    public Billing? Billing { get; set; }

    [ForeignKey(nameof(ReservationId))]
    public Reservation? Reservation { get; set; }
}
