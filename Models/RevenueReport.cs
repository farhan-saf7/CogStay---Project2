namespace CogStayMVC.Models;

public class RevenueReport
{
    public decimal AverageDailyRate { get; set; }
    public decimal OccupancyRate { get; set; }
    public decimal RevPAR { get; set; }
    public decimal RoomRevenue { get; set; }
    public decimal FoodRevenue { get; set; }
    public decimal LaundryRevenue { get; set; }
    public decimal SpaRevenue { get; set; }
    public decimal TotalRevenue => RoomRevenue + FoodRevenue + LaundryRevenue + SpaRevenue;
}
