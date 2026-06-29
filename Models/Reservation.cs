namespace CogStayMVC.Models;

public class Reservation
{
    public int Id { get; set; }
    public string BookingId { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int GuestsCount { get; set; }
    public decimal TotalBilling { get; set; }
    public string PaymentStatus { get; set; } = "Unpaid";
    public string Status { get; set; } = "Pending";
}
