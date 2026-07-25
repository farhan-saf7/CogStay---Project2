using System;
using System.ComponentModel.DataAnnotations;

namespace CogStayMVC.DTOs;

/// <summary>
/// Data Transfer Object representing active stay records of guests in rooms.
/// </summary>
public class StayRecordResponseDTO
{
    /// <summary>
    /// Gets or sets the unique Stay ID.
    /// </summary>
    public int StayId { get; set; }

    /// <summary>
    /// Gets or sets the linked Guest ID.
    /// </summary>
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the guest full name.
    /// </summary>
    public string GuestName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the linked Reservation ID.
    /// </summary>
    public int ReservationId { get; set; }

    /// <summary>
    /// Gets or sets the room number.
    /// </summary>
    public string RoomNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when check-in occurred.
    /// </summary>
    public DateTime? ActualCheckIn { get; set; }

    /// <summary>
    /// Gets or sets the date and time when check-out occurred.
    /// </summary>
    public DateTime? ActualCheckOut { get; set; }

    /// <summary>
    /// Gets a value indicating whether checkout has completed.
    /// </summary>
    public bool IsCheckedOut => ActualCheckOut.HasValue;

    /// <summary>
    /// Gets or sets the invoice detail summary associated with this stay.
    /// </summary>
    public BillingResponseDTO? Billing { get; set; }

    /// <summary>
    /// Gets or sets the booking reservation lookup string reference.
    /// </summary>
    public string? BookingReference { get; set; }

    /// <summary>
    /// Gets or sets the billing invoice lookup string reference.
    /// </summary>
    public string? BillingReference { get; set; }

    /// <summary>
    /// Gets or sets general stay summary descriptions.
    /// </summary>
    public string? StayDetails { get; set; }
}

/// <summary>
/// Parameters needed to check in a guest from an existing reservation.
/// </summary>
public class CreateCheckInDTO
{
    /// <summary>
    /// Gets or sets the reservation ID to be activated.
    /// </summary>
    [Required(ErrorMessage = "Reservation ID is required.")]
    public int ReservationId { get; set; }
}

/// <summary>
/// Parameters needed to request check out of a stay record.
/// </summary>
public class CheckOutDTO
{
    /// <summary>
    /// Gets or sets the Stay Record ID to check out.
    /// </summary>
    [Required(ErrorMessage = "Stay ID is required.")]
    public int StayId { get; set; }
}
