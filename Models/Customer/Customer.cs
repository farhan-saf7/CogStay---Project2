using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CogStayMVC.Models.Admin;
using CogStayMVC.Models.FrontDesk;

namespace CogStayMVC.Models.Customer;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    public int? UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";


    [Required]
    [EmailAddress]
    [MaxLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public string Email { get; set; } = string.Empty;


    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string LoyaltyTier { get; set; } = "Bronze";

    [MaxLength(50)]
    public string LoyaltyId { get; set; } = string.Empty;

    public DateTime MemberSince { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string StreetAddress { get; set; } = string.Empty;

    [MaxLength(50)]
    public string City { get; set; } = string.Empty;

    [MaxLength(50)]
    public string State { get; set; } = string.Empty;

    [MaxLength(20)]
    public string ZipCode { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public ICollection<StayRecord> StayRecords { get; set; } = new List<StayRecord>();

    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
