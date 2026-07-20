using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;

namespace CogStayMVC.Repositories;

public class StayRecordRepository : IStayRecordRepository
{
    private readonly HotelDbContext _context;

    public StayRecordRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<StayRecord?> GetByIdAsync(int id)
    {
        return await _context.StayRecords
            .Include(s => s.Guest)
            .Include(s => s.Reservation)
            .Include(s => s.Billing)
            .FirstOrDefaultAsync(s => s.StayId == id);
    }

    public async Task<StayRecord?> GetByReservationIdAsync(int reservationId)
    {
        return await _context.StayRecords
            .Include(s => s.Guest)
            .Include(s => s.Reservation)
            .Include(s => s.Billing)
            .FirstOrDefaultAsync(s => s.ReservationId == reservationId);
    }

    public async Task<IEnumerable<StayRecord>> GetAllAsync()
    {
        return await _context.StayRecords
            .Include(s => s.Guest)
            .Include(s => s.Reservation)
            .Include(s => s.Billing)
            .ToListAsync();
    }

    public async Task<IEnumerable<StayRecord>> GetActiveStaysAsync()
    {
        return await _context.StayRecords
            .Include(s => s.Guest)
            .Include(s => s.Reservation)
            .Include(s => s.Billing)
            .Where(s => s.ActualCheckIn != null && s.ActualCheckOut == null)
            .ToListAsync();
    }

    public async Task AddAsync(StayRecord stayRecord)
    {
        await _context.StayRecords.AddAsync(stayRecord);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StayRecord stayRecord)
    {
        _context.StayRecords.Update(stayRecord);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var record = await _context.StayRecords.FindAsync(id);
        if (record != null)
        {
            _context.StayRecords.Remove(record);
            await _context.SaveChangesAsync();
        }
    }
}
