using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Repositories.Interfaces;

public interface IStayRecordRepository
{
    Task<StayRecord?> GetByIdAsync(int id);
    Task<StayRecord?> GetByReservationIdAsync(int reservationId);
    Task<IEnumerable<StayRecord>> GetAllAsync();
    Task<IEnumerable<StayRecord>> GetActiveStaysAsync();
    Task AddAsync(StayRecord stayRecord);
    Task UpdateAsync(StayRecord stayRecord);
    Task DeleteAsync(int id);
}
