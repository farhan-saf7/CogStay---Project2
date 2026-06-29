namespace CogStayMVC.ViewModels;

public class CheckOutViewModel
{
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public decimal RoomCharges { get; set; }
    public decimal MinibarCharges { get; set; }
    public decimal IncidentalCharges { get; set; }
    public decimal TotalAmount => RoomCharges + MinibarCharges + IncidentalCharges;
}
