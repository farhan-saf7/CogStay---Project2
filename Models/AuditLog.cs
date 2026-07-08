using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class AuditLog
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    [Required]
    [StringLength(100)]
    public string Action { get; set; } = string.Empty; // Create, Update, Delete, Login, Logout

    [Required]
    [StringLength(100)]
    public string EntityName { get; set; } = string.Empty; // Room, Reservation, etc.

    [Required]
    [StringLength(50)]
    public string EntityId { get; set; } = string.Empty; // Primary key of changed entity

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [StringLength(4000)]
    public string OldValues { get; set; } = string.Empty; // JSON snapshot of old state

    [StringLength(4000)]
    public string NewValues { get; set; } = string.Empty; // JSON snapshot of new state
}
