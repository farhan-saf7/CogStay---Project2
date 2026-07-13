using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CogStayMVC.Models.Admin;

namespace CogStayMVC.Models.Common;

public class Notification
{
    [Key]
    public int NotificationId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
