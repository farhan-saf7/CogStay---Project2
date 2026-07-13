using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models.Room;

public class RoomAmenity
{
    [Key]
    public int RoomAmenityId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    [MaxLength(50)]
    public string AmenityName { get; set; } = string.Empty;

    // Navigation property
    [ForeignKey(nameof(RoomId))]
    public Room? Room { get; set; }
}
