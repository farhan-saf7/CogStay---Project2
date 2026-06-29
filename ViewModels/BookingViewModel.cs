namespace CogStayMVC.ViewModels;

public class BookingViewModel
{
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string RoomCategory { get; set; } = "standard";
    public int NumberOfGuests { get; set; } = 1;
    public string SpecialRequests { get; set; } = string.Empty;
}
