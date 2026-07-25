using System.ComponentModel.DataAnnotations;
using CogStayMVC.Enums;

namespace CogStayMVC.DTOs;

/// <summary>
/// Data Transfer Object representing Room details.
/// </summary>
public class RoomResponseDTO
{
    /// <summary>
    /// Gets or sets the unique Room ID.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the unique Room number string.
    /// </summary>
    public string RoomNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the room type classification (e.g. Single, Twin, Suite).
    /// </summary>
    public string RoomType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the nightly rental pricing.
    /// </summary>
    public decimal PricePerNight { get; set; }

    /// <summary>
    /// Gets or sets the operational room status.
    /// </summary>
    public RoomStatus Status { get; set; }
}

/// <summary>
/// Parameters needed to add a new room to inventory.
/// </summary>
public class CreateRoomDTO
{
    /// <summary>
    /// Gets or sets the unique room number.
    /// </summary>
    [Required(ErrorMessage = "Room number is required.")]
    [StringLength(50)]
    public string RoomNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the room type standard.
    /// </summary>
    [Required(ErrorMessage = "Room type is required.")]
    [StringLength(100)]
    public string RoomType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the nightly rental pricing.
    /// </summary>
    [Required(ErrorMessage = "Price per night is required.")]
    [Range(0.01, 100000.00, ErrorMessage = "Price per night must be greater than zero.")]
    public decimal PricePerNight { get; set; }

    /// <summary>
    /// Gets or sets the initial operational status. Defaults to Available.
    /// </summary>
    public RoomStatus Status { get; set; } = RoomStatus.Available;
}

/// <summary>
/// Parameters needed to update room metadata or status.
/// </summary>
public class UpdateRoomDTO
{
    /// <summary>
    /// Gets or sets the Room ID to update.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the updated room number.
    /// </summary>
    [Required(ErrorMessage = "Room number is required.")]
    [StringLength(50)]
    public string RoomNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the updated room type.
    /// </summary>
    [Required(ErrorMessage = "Room type is required.")]
    [StringLength(100)]
    public string RoomType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the updated price per night.
    /// </summary>
    [Required(ErrorMessage = "Price per night is required.")]
    [Range(0.01, 100000.00, ErrorMessage = "Price per night must be greater than zero.")]
    public decimal PricePerNight { get; set; }

    /// <summary>
    /// Gets or sets the updated room status.
    /// </summary>
    public RoomStatus Status { get; set; }
}
