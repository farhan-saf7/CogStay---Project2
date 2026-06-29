namespace CogStayMVC.ViewModels;

public class CheckInViewModel
{
    public string BookingId { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string IdVerification { get; set; } = "verified";
    public string CreditCardNumber { get; set; } = string.Empty;
}
