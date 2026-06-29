namespace CogStayMVC.Models;

public class HousekeepingTask
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string TaskType { get; set; } = "Checkout Clean"; // Checkout Clean, Stay-Over Clean, Deep Clean
    public string AssignedTo { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium"; // Low, Medium, Urgent
    public string Status { get; set; } = "Pending"; // Pending, In Progress, Completed
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<string> ChecklistItems { get; set; } = new();
}
