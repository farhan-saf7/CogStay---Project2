using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CogStayMVC.Models.FrontDesk;

public class Booking
{
    [Key]
    public int Id { get; set; }

    [NotMapped]
    public string BookingNumber => $"CS-{1000 + Id}";

    [Required]
    [MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Required]
    public int GuestsCount { get; set; } = 1;

    [Required]
    [Precision(18, 2)]
    public decimal TotalAmount { get; set; }

    [Required]
    [MaxLength(20)]
    public string BookingStatus { get; set; } = "Pending"; // Pending, Confirmed, Completed, Cancelled

    [Required]
    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "Unpaid"; // Unpaid, Partial, Paid, Refunded

    [MaxLength(1000)]
    public string SpecialRequests { get; set; } = string.Empty;
}
