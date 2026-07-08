using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class Reservation : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string ReservationNumber { get; set; } = string.Empty;

    [Required]
    public int GuestId { get; set; }

    [ForeignKey(nameof(GuestId))]
    public virtual Guest Guest { get; set; } = null!;

    [Required]
    public int RoomId { get; set; }

    [ForeignKey(nameof(RoomId))]
    public virtual Room Room { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateTime CheckInDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime CheckOutDate { get; set; }

    [Required]
    [Range(1, 20)]
    public int GuestsCount { get; set; }

    [Required]
    [Range(0, 1000000)]
    public decimal TotalAmount { get; set; }

    [Required]
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    [StringLength(500)]
    public string SpecialRequests { get; set; } = string.Empty;

    // Navigation properties
    public virtual StayRecord? StayRecord { get; set; }
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
