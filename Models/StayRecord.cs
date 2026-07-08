using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class StayRecord : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ReservationId { get; set; }

    [ForeignKey(nameof(ReservationId))]
    public virtual Reservation Reservation { get; set; } = null!;

    [Required]
    public int GuestId { get; set; }

    [ForeignKey(nameof(GuestId))]
    public virtual Guest Guest { get; set; } = null!;

    public DateTime? ActualCheckIn { get; set; }
    
    public DateTime? ActualCheckOut { get; set; }

    // Navigation properties
    public virtual Billing? Billing { get; set; }
    public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}
