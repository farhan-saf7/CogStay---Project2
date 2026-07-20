using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Services.Interfaces;

public interface IRoomService
{
    Task<Room?> GetByIdAsync(int id);
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<IEnumerable<Room>> GetAvailableRoomsAsync();
    Task<Room> CreateRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);
    Task<bool> DeleteRoomAsync(int id);
    Task<IEnumerable<Room>> SearchRoomsAsync(string? query);
}
