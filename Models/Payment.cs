using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class Payment : BaseEntity
{
    [Key]
    public int Id { get; set; }

    public int? BillingId { get; set; }

    [ForeignKey(nameof(BillingId))]
    public virtual Billing? Billing { get; set; }

    public int? ReservationId { get; set; }

    [ForeignKey(nameof(ReservationId))]
    public virtual Reservation? Reservation { get; set; }

    [Required]
    [Range(0.01, 10000000)]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = "Credit Card"; // Credit Card, Cash, Debit Card, UPI, NetBanking

    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    [Required]
    [StringLength(100)]
    public string TransactionId { get; set; } = string.Empty;

    [Required]
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    [StringLength(100)]
    public string CardholderName { get; set; } = string.Empty;

    [StringLength(20)]
    public string CardNumber { get; set; } = string.Empty; // Masked card number (e.g. **** **** **** 1234)
}
