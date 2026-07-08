using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models;

public class WellnessSpaRequest
{
    [Key]
    public int WellnessSpaRequestId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    [MaxLength(100)]
    public string GuestName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ServiceTreatment { get; set; } = string.Empty; // Aromatherapy Massage, Swedish Massage, Facial, etc.

    [Required]
    [MaxLength(50)]
    public string PreferredSlot { get; set; } = string.Empty;

    [MaxLength(100)]
    public string AssignedTherapist { get; set; } = "Pending Assignment";

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Scheduled, In-Progress, Completed

    [MaxLength(255)]
    public string Remarks { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(RoomId))]
    public Room? Room { get; set; }
}
