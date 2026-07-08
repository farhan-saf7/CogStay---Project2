using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CogStayMVC.Models;

public class RoomType : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal BasePrice { get; set; }

    [Range(1, 20)]
    public int MaxCapacity { get; set; }

    // Navigation properties
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
