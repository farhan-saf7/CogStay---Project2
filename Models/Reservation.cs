using System;
using CogStayMVC.Enums;

namespace CogStayMVC.Models;

/// <summary>
/// Represents a room reservation made by a guest. Holds check-in and check-out dates,
/// reservation status (Booked, Cancelled), and maps to guest and room tables.
/// </summary>
public class Reservation
{
    /// <summary>
    /// Gets or sets the unique identifier for the reservation.
    /// </summary>
    public int ReservationId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the guest who placed this reservation.
    /// </summary>
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the guest's name at the time of booking.
    /// </summary>
    public string GuestName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the room booked in this reservation.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the scheduled check-in date.
    /// </summary>
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Gets or sets the scheduled check-out date.
    /// </summary>
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// Gets or sets the current status of this reservation (e.g., Booked, Cancelled).
    /// </summary>
    public ReservationStatus ReservationStatus { get; set; }

    // Navigation Properties

    /// <summary>
    /// Gets or sets the guest who placed the reservation.
    /// </summary>
    public virtual Guest Guest { get; set; } = null!;

    /// <summary>
    /// Gets or sets the room allocated to this reservation.
    /// </summary>
    public virtual Room Room { get; set; } = null!;

    /// <summary>
    /// Gets or sets the stay record generated once the guest checks in for this reservation.
    /// Establishes a One-to-One relationship with the StayRecord.
    /// </summary>
    public virtual StayRecord? StayRecord { get; set; }
}
