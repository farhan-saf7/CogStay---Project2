using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models.FrontDesk;

public class StayRecord
{
    [Key]
    public int StayId { get; set; }

    [Required]
    public int GuestId { get; set; }

    [Required]
    public int ReservationId { get; set; }

    [Required]
    public DateTime ActualCheckIn { get; set; }

    public DateTime? ActualCheckOut { get; set; }

    // Navigation properties
    [ForeignKey(nameof(GuestId))]
    public Customer.Customer? Guest { get; set; }

    [ForeignKey(nameof(ReservationId))]
    public Reservation? Reservation { get; set; }

    public Billing.Billing? Billing { get; set; }
}
