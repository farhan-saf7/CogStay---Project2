using System;
using System.ComponentModel.DataAnnotations;
using CogStayMVC.Enums;

namespace CogStayMVC.DTOs;

/// <summary>
/// Data Transfer Object representing a room booking reservation response.
/// </summary>
public class ReservationResponseDTO
{
    /// <summary>
    /// Gets or sets the unique Reservation ID.
    /// </summary>
    public int ReservationId { get; set; }

    /// <summary>
    /// Gets or sets the linked Guest ID.
    /// </summary>
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the guest full name.
    /// </summary>
    public string GuestName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the linked Room ID.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the room number.
    /// </summary>
    public string RoomNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the room type classification (e.g. Deluxe, Suite).
    /// </summary>
    public string RoomType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the night pricing rate of the room.
    /// </summary>
    public decimal PricePerNight { get; set; }

    /// <summary>
    /// Gets or sets the booking check-in date.
    /// </summary>
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Gets or sets the booking check-out date.
    /// </summary>
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// Gets or sets the booking status (Booked/Cancelled).
    /// </summary>
    public ReservationStatus ReservationStatus { get; set; }

    /// <summary>
    /// Gets the total nights duration. Defaults to minimum 1 night.
    /// </summary>
    public int TotalNights => (CheckOutDate - CheckInDate).Days > 0 ? (CheckOutDate - CheckInDate).Days : 1;

    /// <summary>
    /// Gets the estimated total charge for the reservation stay.
    /// </summary>
    public decimal EstimatedTotalCost => TotalNights * PricePerNight;
}

/// <summary>
/// Parameters needed to create a new reservation.
/// </summary>
public class CreateReservationDTO
{
    /// <summary>
    /// Gets or sets the guest account ID placing the booking.
    /// </summary>
    [Required(ErrorMessage = "Guest selection is required.")]
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the target Room ID.
    /// </summary>
    [Required(ErrorMessage = "Room selection is required.")]
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the desired check-in date.
    /// </summary>
    [Required(ErrorMessage = "Check-in date is required.")]
    [DataType(DataType.Date)]
    public DateTime CheckInDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Gets or sets the desired check-out date.
    /// </summary>
    [Required(ErrorMessage = "Check-out date is required.")]
    [DataType(DataType.Date)]
    public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);
}

/// <summary>
/// Parameters needed to update dates or state of a reservation.
/// </summary>
public class UpdateReservationDTO
{
    /// <summary>
    /// Gets or sets the Reservation ID to modify.
    /// </summary>
    public int ReservationId { get; set; }

    /// <summary>
    /// Gets or sets the guest ID.
    /// </summary>
    [Required]
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the Room ID.
    /// </summary>
    [Required]
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the check-in date.
    /// </summary>
    [Required]
    [DataType(DataType.Date)]
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Gets or sets the check-out date.
    /// </summary>
    [Required]
    [DataType(DataType.Date)]
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// Gets or sets the current booking reservation status.
    /// </summary>
    public ReservationStatus ReservationStatus { get; set; }
}
