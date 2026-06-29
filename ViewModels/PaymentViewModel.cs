namespace CogStayMVC.ViewModels;

public class PaymentViewModel
{
    public string CardholderName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;
    public decimal AmountDue { get; set; }
}
