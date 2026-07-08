using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CogStayMVC.Models;

public enum ReservationStatus
{
    BOOKED,
    CANCELLED
}

public class Reservation
{
    [Key]
    public int ReservationId { get; set; }

    [Required]
    public int GuestId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Required]
    public int GuestsCount { get; set; } = 1;

    [Required]
    [Precision(18, 2)]
    public decimal TotalBilling { get; set; }

    [Required]
    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "Unpaid"; // Unpaid, Partial, Paid, Refunded

    [Required]
    public ReservationStatus ReservationStatus { get; set; } = ReservationStatus.BOOKED;

    [MaxLength(1000)]
    public string SpecialRequests { get; set; } = string.Empty;

    public DateTime? CancelledDate { get; set; }

    [MaxLength(500)]
    public string CancellationReason { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(GuestId))]
    public Customer? Guest { get; set; }

    [ForeignKey(nameof(RoomId))]
    public Room? Room { get; set; }

    public StayRecord? StayRecord { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
