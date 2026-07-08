using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class MaintenanceRequest : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RoomId { get; set; }

    [ForeignKey(nameof(RoomId))]
    public virtual Room Room { get; set; } = null!;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public int? ReportedByStaffId { get; set; }

    [ForeignKey(nameof(ReportedByStaffId))]
    public virtual Staff? ReportedByStaff { get; set; }
}
