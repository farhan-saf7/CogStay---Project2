using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class RoomAmenity
{
    public int RoomId { get; set; }

    [ForeignKey(nameof(RoomId))]
    public virtual Room Room { get; set; } = null!;

    public int AmenityId { get; set; }

    [ForeignKey(nameof(AmenityId))]
    public virtual Amenity Amenity { get; set; } = null!;
}
