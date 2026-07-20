using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;

namespace CogStayMVC.Repositories;

public class HousekeepingRepository : IHousekeepingRepository
{
    private readonly HotelDbContext _context;

    public HousekeepingRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<HousekeepingTask?> GetByIdAsync(int id)
    {
        return await _context.HousekeepingTasks
            .Include(t => t.Room)
            .FirstOrDefaultAsync(t => t.TaskId == id);
    }

    public async Task<IEnumerable<HousekeepingTask>> GetAllAsync()
    {
        return await _context.HousekeepingTasks
            .Include(t => t.Room)
            .ToListAsync();
    }

    public async Task<IEnumerable<HousekeepingTask>> GetByRoomIdAsync(int roomId)
    {
        return await _context.HousekeepingTasks
            .Include(t => t.Room)
            .Where(t => t.RoomId == roomId)
            .ToListAsync();
    }

    public async Task AddAsync(HousekeepingTask task)
    {
        await _context.HousekeepingTasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(HousekeepingTask task)
    {
        _context.HousekeepingTasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var task = await _context.HousekeepingTasks.FindAsync(id);
        if (task != null)
        {
            _context.HousekeepingTasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
