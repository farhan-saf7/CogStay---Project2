using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Services.Interfaces;

public interface IBillingService
{
    Task<Billing?> GetByIdAsync(int id);
    Task<Billing?> GetByStayIdAsync(int stayId);
    Task<IEnumerable<Billing>> GetAllBillingsAsync();
    Task GenerateInvoiceAsync(int stayId);
    Task ProcessPaymentAsync(int billId, Enums.PaymentStatus status, string? remarks);
}
