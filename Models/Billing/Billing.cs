using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Models.FrontDesk;

namespace CogStayMVC.Models.Billing;

public enum PaymentStatus
{
    PENDING,
    PAID
}

public class Billing
{
    [Key]
    public int BillId { get; set; }

    [Required]
    public int StayId { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal TotalAmount { get; set; }

    [Required]
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.PENDING;

    [MaxLength(500)]
    public string Remarks { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(StayId))]
    public StayRecord? StayRecord { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
