using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Repositories.Implementations;

namespace CogStayMVC.Repositories.Housekeeping;

/// <summary>
/// Repository implementation managing Housekeeping tasks.
/// Handles querying tasks with room navigation properties preloaded.
/// </summary>
public class HousekeepingTaskRepository : Repository<HousekeepingTask>, IHousekeepingTaskRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HousekeepingTaskRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public HousekeepingTaskRepository(HotelDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves all housekeeping tasks with associated Room details loaded.
    /// </summary>
    /// <returns>A collection of detailed housekeeping tasks.</returns>
    public async Task<IEnumerable<HousekeepingTask>> GetTasksWithDetailsAsync()
    {
        return await _dbSet
            .Include(t => t.Room)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves detailed info for a single housekeeping task by its ID.
    /// </summary>
    /// <param name="id">Task identifier.</param>
    /// <returns>Detailed housekeeping task, or null.</returns>
    public async Task<HousekeepingTask?> GetTaskWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(t => t.Room)
            .FirstOrDefaultAsync(t => t.TaskId == id);
    }

    /// <summary>
    /// Retrieves housekeeping tasks assigned to a specific room.
    /// </summary>
    /// <param name="roomId">Room identifier.</param>
    /// <returns>A collection of detailed housekeeping tasks.</returns>
    public async Task<IEnumerable<HousekeepingTask>> GetTasksByRoomIdAsync(int roomId)
    {
        return await _dbSet
            .Include(t => t.Room)
            .Where(t => t.RoomId == roomId)
            .ToListAsync();
    }
}
