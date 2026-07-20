using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Repositories.Interfaces;

public interface IBillingRepository
{
    Task<Billing?> GetByIdAsync(int id);
    Task<Billing?> GetByStayIdAsync(int stayId);
    Task<IEnumerable<Billing>> GetAllAsync();
    Task AddAsync(Billing billing);
    Task UpdateAsync(Billing billing);
    Task DeleteAsync(int id);
}
