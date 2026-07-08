using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class FoodOrder
{
    [Key]
    public int FoodOrderId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    [MaxLength(100)]
    public string GuestName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string DishOrderDetails { get; set; } = string.Empty;

    [MaxLength(255)]
    public string DeliveryNote { get; set; } = string.Empty;

    public DateTime OrderedTime { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Preparing"; // Preparing, Cooking, Delivered

    // Navigation properties
    [ForeignKey(nameof(RoomId))]
    public Room? Room { get; set; }
}
