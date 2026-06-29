namespace CogStayMVC.Models;

public class StaffUser
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = "Housekeeper"; // Admin, Manager, FrontDesk, Housekeeper
    public string ShiftSchedule { get; set; } = "Day Shift"; // Day Shift, Night Shift, On Call
    public string Status { get; set; } = "Active"; // Active, On Leave, Suspended
}
