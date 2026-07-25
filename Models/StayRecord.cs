using System;

namespace CogStayMVC.Models;

/// <summary>
/// Represents a guest stay record in the hotel. Initiated upon successful check-in
/// and finalized during checkout. Linked with guest, reservation, and billing details.
/// </summary>
public class StayRecord
{
    /// <summary>
    /// Gets or sets the unique identifier for the stay record.
    /// </summary>
    public int StayId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the guest checking in.
    /// </summary>
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the confirmed reservation that initiated this stay.
    /// </summary>
    public int ReservationId { get; set; }

    /// <summary>
    /// Gets or sets the actual timestamp of the guest check-in.
    /// </summary>
    public DateTime? ActualCheckIn { get; set; }

    /// <summary>
    /// Gets or sets the actual timestamp of the guest check-out.
    /// </summary>
    public DateTime? ActualCheckOut { get; set; }

    /// <summary>
    /// Gets or sets the guest's name at check-in time.
    /// </summary>
    public string GuestName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets historical reference codes regarding the original room booking.
    /// </summary>
    public string? BookingReference { get; set; }

    /// <summary>
    /// Gets or sets historical reference codes regarding the associated billing invoice.
    /// </summary>
    public string? BillingReference { get; set; }

    /// <summary>
    /// Gets or sets any optional description, logs, or special guest service notes.
    /// </summary>
    public string? StayDetails { get; set; }

    // Navigation Properties

    /// <summary>
    /// Gets or sets the guest associated with this stay.
    /// </summary>
    public virtual Guest Guest { get; set; } = null!;

    /// <summary>
    /// Gets or sets the reservation that spawned this stay.
    /// </summary>
    public virtual Reservation Reservation { get; set; } = null!;

    /// <summary>
    /// Gets or sets the billing record associated with this stay.
    /// Represents a One-to-One relationship with the Billing model.
    /// </summary>
    public virtual Billing? Billing { get; set; }
}
