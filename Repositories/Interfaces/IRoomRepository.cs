using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Repositories.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(int id);
    Task<Room?> GetByRoomNumberAsync(string roomNumber);
    Task<IEnumerable<Room>> GetAllAsync();
    Task<IEnumerable<Room>> GetAvailableRoomsAsync();
    Task AddAsync(Room room);
    Task UpdateAsync(Room room);
    Task DeleteAsync(int id);
    Task<bool> HasReservationsAsync(int roomId);
    Task<bool> HasHousekeepingTasksAsync(int roomId);
}
