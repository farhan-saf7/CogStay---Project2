using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CogStayMVC.Models.Admin;

namespace CogStayMVC.Models.Common;

public class LoginHistory
{
    [Key]
    public int LoginHistoryId { get; set; }

    [Required]
    public int UserId { get; set; }

    public DateTime LoginTime { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string IpAddress { get; set; } = string.Empty;

    // Navigation property
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
