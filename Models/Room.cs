namespace CogStayMVC.Models;

public class Room
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty; // Standard, Deluxe, Suite, Presidential
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string Status { get; set; } = "Available"; // Available, Occupied, Maintenance
    public string Cleanliness { get; set; } = "Clean"; // Clean, Dirty, In Progress
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Amenities { get; set; } = new();
}
