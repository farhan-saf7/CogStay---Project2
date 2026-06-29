namespace CogStayMVC.Models;

public class Feedback
{
    public int Id { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Category { get; set; } = "General"; // Comfort, Service, Food, Facilities, Overall
    public string Comments { get; set; } = string.Empty;
    public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;
    public bool IsApproved { get; set; } = false;
}
