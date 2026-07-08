using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class Feedback : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int GuestId { get; set; }

    [ForeignKey(nameof(GuestId))]
    public virtual Guest Guest { get; set; } = null!;

    public int? RoomId { get; set; }

    [ForeignKey(nameof(RoomId))]
    public virtual Room? Room { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = "General"; // Comfort, Service, Food, Facilities, Overall

    [Required]
    [StringLength(1000)]
    public string Comments { get; set; } = string.Empty;

    public bool IsApproved { get; set; } = false;
}
