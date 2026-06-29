namespace CogStayMVC.Models;

public class Payment
{
    public int Id { get; set; }
    public string CardholderName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Paid, Refunded
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}
