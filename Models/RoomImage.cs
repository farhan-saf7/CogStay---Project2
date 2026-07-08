using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class RoomImage : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RoomId { get; set; }

    [ForeignKey(nameof(RoomId))]
    public virtual Room Room { get; set; } = null!;

    [Required]
    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    public bool IsPrimary { get; set; } = false;
}
