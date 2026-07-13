using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models.Housekeeping;

public class MaintenanceRequest
{
    [Key]
    public int MaintenanceRequestId { get; set; }

    public int? RoomId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Location { get; set; } = string.Empty; // e.g. Room 201 or Lobby

    [Required]
    [MaxLength(1000)]
    public string IssueDescription { get; set; } = string.Empty;

    public DateTime LoggedTimestamp { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(20)]
    public string Severity { get; set; } = "Medium"; // Low, Medium, Urgent

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Assigned, Resolved

    // Navigation properties
    [ForeignKey(nameof(RoomId))]
    public Room.Room? Room { get; set; }
}
