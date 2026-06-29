namespace CogStayMVC.ViewModels;

public class HousekeepingTaskViewModel
{
    public string RoomNumber { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public string TaskCategory { get; set; } = "checkout";
    public string Priority { get; set; } = "medium";
    public string Instructions { get; set; } = string.Empty;
}
