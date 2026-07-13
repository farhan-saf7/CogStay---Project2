using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Models.FrontDesk;
using CogStayMVC.Models.Housekeeping;

namespace CogStayMVC.Models.Room;

public enum RoomStatus
{
    AVAILABLE,
    OCCUPIED,
    UNDER_MAINTENANCE
}

public class Room
{
    [Key]
    public int RoomId { get; set; }

    [Required]
    [MaxLength(10)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RoomType { get; set; } = string.Empty; // Standard, Deluxe, Suite, Presidential

    [Required]
    [Precision(18, 2)]
    public decimal PricePerNight { get; set; }

    [Required]
    public int Capacity { get; set; }

    public bool IsAvailable { get; set; } = true;

    [Required]
    public RoomStatus Status { get; set; } = RoomStatus.AVAILABLE;

    [Required]
    [MaxLength(20)]
    public string Cleanliness { get; set; } = "Clean"; // Clean, Dirty, In Progress

    [MaxLength(255)]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public ICollection<HousekeepingTask> HousekeepingTasks { get; set; } = new List<HousekeepingTask>();

    public ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();

    public ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();
}
