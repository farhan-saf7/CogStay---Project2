using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class Billing : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int StayRecordId { get; set; }

    [ForeignKey(nameof(StayRecordId))]
    public virtual StayRecord StayRecord { get; set; } = null!;

    [Required]
    [Range(0, 10000000)]
    public decimal RoomCharges { get; set; }

    [Required]
    [Range(0, 10000000)]
    public decimal ServiceCharges { get; set; }

    [Required]
    [Range(0, 1000000)]
    public decimal TaxAmount { get; set; }

    [Required]
    [Range(0, 1000000)]
    public decimal DiscountAmount { get; set; }

    [Required]
    [Range(0, 10000000)]
    public decimal TotalAmount { get; set; }

    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    [StringLength(255)]
    public string Remarks { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
