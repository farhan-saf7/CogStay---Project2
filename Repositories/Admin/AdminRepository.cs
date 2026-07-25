using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Repositories.Implementations;

namespace CogStayMVC.Repositories.Admin;

/// <summary>
/// Repository implementation managing Room entity queries.
/// </summary>
public class RoomRepository : Repository<Room>, IRoomRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoomRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public RoomRepository(HotelDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves a room record by its unique room number.
    /// </summary>
    /// <param name="roomNumber">Unique identifier (e.g., "101").</param>
    /// <returns>Matching Room, or null.</returns>
    public async Task<Room?> GetByRoomNumberAsync(string roomNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
    }

    /// <summary>
    /// Retrieves rooms matching the specified operational status.
    /// </summary>
    /// <param name="status">RoomStatus enum value.</param>
    /// <returns>A collection of matching rooms.</returns>
    public async Task<IEnumerable<Room>> GetRoomsByStatusAsync(RoomStatus status)
    {
        return await _dbSet.Where(r => r.Status == status).ToListAsync();
    }
}

/// <summary>
/// Repository implementation managing Staff entity queries.
/// </summary>
public class StaffRepository : Repository<Staff>, IStaffRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StaffRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public StaffRepository(HotelDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves a staff record by their unique login email.
    /// </summary>
    /// <param name="email">Staff login email.</param>
    /// <returns>Matching Staff record, or null.</returns>
    public async Task<Staff?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.Email == email);
    }
}
