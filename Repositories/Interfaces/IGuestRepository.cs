using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Repositories.Interfaces;

public interface IGuestRepository
{
    Task<Guest?> GetByIdAsync(int id);
    Task<Guest?> GetByEmailAsync(string email);
    Task<IEnumerable<Guest>> GetAllAsync();
    Task AddAsync(Guest guest);
    Task UpdateAsync(Guest guest);
    Task DeleteAsync(int id);
    Task<bool> HasReservationsAsync(int guestId);
}
