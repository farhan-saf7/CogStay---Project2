using System.Collections.Generic;
using CogStayMVC.Enums;

namespace CogStayMVC.Models;

/// <summary>
/// Represents a hotel room in the system. Holds details about the room number,
/// type (e.g. Deluxe, Suite), price per night, and current operational/cleanliness status.
/// </summary>
public class Room
{
    /// <summary>
    /// Gets or sets the unique identifier for the room.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the room number identifier (e.g., "101", "Penthouse A"). Must be unique.
    /// </summary>
    public string RoomNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the room category/type (e.g., Single, Double, Deluxe, Suite).
    /// </summary>
    public string RoomType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the nightly cost of booking this room.
    /// </summary>
    public decimal PricePerNight { get; set; }

    /// <summary>
    /// Gets or sets the current operational status of the room (e.g., Available, Booked, Occupied, Maintenance).
    /// </summary>
    public RoomStatus Status { get; set; }

    // Navigation Properties

    /// <summary>
    /// Gets or sets the list of reservations associated with this room.
    /// Represents a One-to-Many relationship (one room can have many reservations over time).
    /// </summary>
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    /// <summary>
    /// Gets or sets the list of housekeeping tasks scheduled or completed for this room.
    /// Represents a One-to-Many relationship (one room can have many housekeeping tasks).
    /// </summary>
    public virtual ICollection<HousekeepingTask> HousekeepingTasks { get; set; } = new List<HousekeepingTask>();
}
