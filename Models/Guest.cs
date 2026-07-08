using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class Guest : BaseEntity
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

    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    [StringLength(50)]
    public string IdentificationType { get; set; } = string.Empty; // Passport, Driver License, Aadhaar, SSN, etc.

    [StringLength(50)]
    public string IdentificationNumber { get; set; } = string.Empty;

    [StringLength(20)]
    public string LoyaltyTier { get; set; } = "Bronze"; // Bronze, Silver, Gold, Platinum

    [StringLength(50)]
    public string LoyaltyId { get; set; } = string.Empty;

    public DateTime MemberSince { get; set; } = DateTime.UtcNow;

    public int? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    // Navigation properties
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public virtual ICollection<StayRecord> StayRecords { get; set; } = new List<StayRecord>();
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
