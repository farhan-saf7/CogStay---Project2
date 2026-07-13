using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models.Housekeeping;

public class ServiceRequest
{
    [Key]
    public int ServiceRequestId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    [MaxLength(50)]
    public string RequestType { get; set; } = string.Empty; // e.g. Cleaning, Room Service Food, Laundry Service, Maintenance, Amenities

    [Required]
    [MaxLength(500)]
    public string Details { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Priority { get; set; } = "Medium"; // Low, Medium, Urgent

    public DateTime RosterTime { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Completed, Dispatched, Resolved

    // Navigation properties
    [ForeignKey(nameof(RoomId))]
    public Room.Room? Room { get; set; }
}
