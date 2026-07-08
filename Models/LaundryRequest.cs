using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class LaundryRequest
{
    [Key]
    public int LaundryRequestId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    [MaxLength(100)]
    public string GuestName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceClass { get; set; } = string.Empty; // Dry Cleaning, Standard Wash, Express Ironing

    [Required]
    public int LinenCount { get; set; } = 1;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Sorting"; // Sorting, In Wash, Ironing, Delivered

    // Navigation properties
    [ForeignKey(nameof(RoomId))]
    public Room? Room { get; set; }
}
