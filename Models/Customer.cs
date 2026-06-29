namespace CogStayMVC.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string LoyaltyTier { get; set; } = "Bronze";
    public string LoyaltyId { get; set; } = string.Empty;
    public DateTime MemberSince { get; set; } = DateTime.UtcNow;
}
