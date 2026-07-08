using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class Feedback
{
    [Key]
    public int FeedbackId { get; set; }

    public int? CustomerId { get; set; }

    [Required]
    [MaxLength(100)]
    public string GuestName { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = "General"; // Comfort, Service, Food, Facilities, Overall

    [Required]
    [MaxLength(2000)]
    public string Comments { get; set; } = string.Empty;

    public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;

    public bool IsApproved { get; set; } = false;

    // Navigation property
    [ForeignKey(nameof(CustomerId))]
    public Customer? Guest { get; set; }
}
