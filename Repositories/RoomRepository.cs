using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;

namespace CogStayMVC.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly HotelDbContext _context;

    public RoomRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        return await _context.Rooms
            .Include(r => r.HousekeepingTasks)
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.RoomId == id);
    }

    public async Task<Room?> GetByRoomNumberAsync(string roomNumber)
    {
        return await _context.Rooms
            .FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _context.Rooms.ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
    {
        return await _context.Rooms
            .Where(r => r.Status == RoomStatus.Available)
            .ToListAsync();
    }

    public async Task AddAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room != null)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasReservationsAsync(int roomId)
    {
        return await _context.Reservations.AnyAsync(r => r.RoomId == roomId);
    }

    public async Task<bool> HasHousekeepingTasksAsync(int roomId)
    {
        return await _context.HousekeepingTasks.AnyAsync(t => t.RoomId == roomId);
    }
}
