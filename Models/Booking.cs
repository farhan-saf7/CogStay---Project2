namespace CogStayMVC.Models;

public class Booking
{
    public int Id { get; set; }
    public string BookingNumber => $"CS-{1000 + Id}";
    public string CustomerName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int GuestsCount { get; set; } = 1;
    public decimal TotalAmount { get; set; }
    public string BookingStatus { get; set; } = "Pending"; // Pending, Confirmed, Completed, Cancelled
    public string PaymentStatus { get; set; } = "Unpaid"; // Unpaid, Partial, Paid, Refunded
    public string SpecialRequests { get; set; } = string.Empty;
}
