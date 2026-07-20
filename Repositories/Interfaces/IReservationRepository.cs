using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Repositories.Interfaces;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(int id);
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<IEnumerable<Reservation>> GetByGuestIdAsync(int guestId);
    Task AddAsync(Reservation reservation);
    Task UpdateAsync(Reservation reservation);
    Task DeleteAsync(int id);
}
