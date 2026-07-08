using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class ServiceRequest : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int StayRecordId { get; set; }

    [ForeignKey(nameof(StayRecordId))]
    public virtual StayRecord StayRecord { get; set; } = null!;

    [Required]
    public ServiceType RequestType { get; set; }

    [Required]
    [StringLength(255)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, 100000)]
    public decimal Amount { get; set; }

    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public int? AssignedToStaffId { get; set; }

    [ForeignKey(nameof(AssignedToStaffId))]
    public virtual Staff? AssignedToStaff { get; set; }
}
