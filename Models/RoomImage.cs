using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class RoomImage
{
    [Key]
    public int RoomImageId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    [MaxLength(255)]
    public string ImageUrl { get; set; } = string.Empty;

    // Navigation property
    [ForeignKey(nameof(RoomId))]
    public Room? Room { get; set; }
}
