using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CogStayMVC.Models;

public class Amenity : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [StringLength(100)]
    public string IconClass { get; set; } = string.Empty; // FontAwesome icon class

    // Navigation properties
    public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
}
