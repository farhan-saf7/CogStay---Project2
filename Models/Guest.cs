using System;
using System.Collections.Generic;

namespace CogStayMVC.Models;

/// <summary>
/// Represents a guest (customer) registered in the CogStay system.
/// Guests can authenticate, book rooms, track their stays, and submit feedback.
/// </summary>
public class Guest
{
    /// <summary>
    /// Gets or sets the unique identifier for the guest.
    /// </summary>
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the guest.
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique email address of the guest used for login and notifications.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the contact phone number of the guest.
    /// </summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the residential address of the guest.
    /// </summary>
    public string Address { get; set; } = null!;

    /// <summary>
    /// Gets or sets the hashed password for account security during authentication.
    /// </summary>
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// Gets or sets the timestamp when the guest account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    // Navigation Properties

    /// <summary>
    /// Gets or sets the collection of reservations associated with this guest.
    /// Represents a One-to-Many relationship (one guest can have many reservations).
    /// </summary>
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    /// <summary>
    /// Gets or sets the history of stay records for this guest.
    /// Represents a One-to-Many relationship (one guest can have many stay records over time).
    /// </summary>
    public virtual ICollection<StayRecord> StayRecords { get; set; } = new List<StayRecord>();
}
