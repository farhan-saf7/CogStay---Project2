using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class HousekeepingTask : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RoomId { get; set; }

    [ForeignKey(nameof(RoomId))]
    public virtual Room Room { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string TaskType { get; set; } = "Checkout Clean"; // Checkout Clean, Stay-Over Clean, Deep Clean

    [Required]
    [StringLength(255)]
    public string TaskDescription { get; set; } = string.Empty;

    public int? AssignedToStaffId { get; set; }

    [ForeignKey(nameof(AssignedToStaffId))]
    public virtual Staff? AssignedToStaff { get; set; }

    [Required]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    // EF Core 8+ supports primitive collections directly
    public List<string> ChecklistItems { get; set; } = new();
}
