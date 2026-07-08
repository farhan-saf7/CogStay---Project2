using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class Room : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    public int RoomTypeId { get; set; }

    [ForeignKey(nameof(RoomTypeId))]
    public virtual RoomType RoomType { get; set; } = null!;

    [Required]
    [Range(0, 100000)]
    public decimal PricePerNight { get; set; }

    [Required]
    [Range(1, 20)]
    public int Capacity { get; set; }

    public bool IsAvailable { get; set; } = true;

    [Required]
    public RoomStatus Status { get; set; } = RoomStatus.Available;

    [StringLength(50)]
    public string Cleanliness { get; set; } = "Clean"; // Clean, Dirty, In Progress

    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();
    public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public virtual ICollection<HousekeepingTask> HousekeepingTasks { get; set; } = new List<HousekeepingTask>();
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}
