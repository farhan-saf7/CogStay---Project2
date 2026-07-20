using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;

namespace CogStayMVC.Repositories;

public class GuestRepository : IGuestRepository
{
    private readonly HotelDbContext _context;

    public GuestRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Guest?> GetByIdAsync(int id)
    {
        return await _context.Guests
            .Include(g => g.Reservations)
            .Include(g => g.StayRecords)
            .FirstOrDefaultAsync(g => g.GuestId == id);
    }

    public async Task<Guest?> GetByEmailAsync(string email)
    {
        return await _context.Guests
            .FirstOrDefaultAsync(g => g.Email == email);
    }

    public async Task<IEnumerable<Guest>> GetAllAsync()
    {
        return await _context.Guests.ToListAsync();
    }

    public async Task AddAsync(Guest guest)
    {
        await _context.Guests.AddAsync(guest);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guest guest)
    {
        _context.Guests.Update(guest);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var guest = await _context.Guests.FindAsync(id);
        if (guest != null)
        {
            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasReservationsAsync(int guestId)
    {
        return await _context.Reservations.AnyAsync(r => r.GuestId == guestId);
    }
}
