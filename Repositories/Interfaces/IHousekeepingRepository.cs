using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Repositories.Interfaces;

public interface IHousekeepingRepository
{
    Task<HousekeepingTask?> GetByIdAsync(int id);
    Task<IEnumerable<HousekeepingTask>> GetAllAsync();
    Task<IEnumerable<HousekeepingTask>> GetByRoomIdAsync(int roomId);
    Task AddAsync(HousekeepingTask task);
    Task UpdateAsync(HousekeepingTask task);
    Task DeleteAsync(int id);
}
