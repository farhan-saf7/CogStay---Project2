using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class Staff : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(100)]
    public string ShiftSchedule { get; set; } = string.Empty; // Day Shift, Night Shift, On Call, etc.

    [StringLength(50)]
    public string Status { get; set; } = "Active"; // Active, On Leave, Terminated

    public int? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    // Navigation properties
    public virtual ICollection<HousekeepingTask> AssignedTasks { get; set; } = new List<HousekeepingTask>();
    public virtual ICollection<ServiceRequest> AssignedServiceRequests { get; set; } = new List<ServiceRequest>();
    public virtual ICollection<MaintenanceRequest> ReportedMaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}
