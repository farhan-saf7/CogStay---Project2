using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CogStayMVC.Models.Admin;

namespace CogStayMVC.Models.Common;

public class RefreshToken
{
    [Key]
    public int RefreshTokenId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Token { get; set; } = string.Empty;

    [Required]
    public DateTime Expires { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAt { get; set; }

    // Navigation property
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
