using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;

namespace CogStayMVC.Repositories;

public class BillingRepository : IBillingRepository
{
    private readonly HotelDbContext _context;

    public BillingRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Billing?> GetByIdAsync(int id)
    {
        return await _context.Billings
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Guest)
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Reservation)
                    .ThenInclude(r => r.Room)
            .FirstOrDefaultAsync(b => b.BillId == id);
    }

    public async Task<Billing?> GetByStayIdAsync(int stayId)
    {
        return await _context.Billings
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Guest)
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Reservation)
                    .ThenInclude(r => r.Room)
            .FirstOrDefaultAsync(b => b.StayId == stayId);
    }

    public async Task<IEnumerable<Billing>> GetAllAsync()
    {
        return await _context.Billings
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Guest)
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Reservation)
                    .ThenInclude(r => r.Room)
            .ToListAsync();
    }

    public async Task AddAsync(Billing billing)
    {
        await _context.Billings.AddAsync(billing);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Billing billing)
    {
        _context.Billings.Update(billing);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var bill = await _context.Billings.FindAsync(id);
        if (bill != null)
        {
            _context.Billings.Remove(bill);
            await _context.SaveChangesAsync();
        }
    }
}
