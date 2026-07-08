using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public enum HousekeepingTaskStatus
{
    PENDING,
    IN_PROGRESS,
    COMPLETED
}

public class HousekeepingTask
{
    [Key]
    public int TaskId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [MaxLength(500)]
    public string TaskDescription { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TaskType { get; set; } = "Checkout Clean"; // Checkout Clean, Stay-Over Clean, Deep Clean

    public int? AssignedToStaffId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Priority { get; set; } = "Medium"; // Low, Medium, Urgent

    [Required]
    public HousekeepingTaskStatus TaskStatus { get; set; } = HousekeepingTaskStatus.PENDING;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<string> ChecklistItems { get; set; } = new();

    // Navigation properties
    [ForeignKey(nameof(RoomId))]
    public Room? Room { get; set; }

    [ForeignKey(nameof(AssignedToStaffId))]
    public StaffUser? AssignedStaff { get; set; }
}
