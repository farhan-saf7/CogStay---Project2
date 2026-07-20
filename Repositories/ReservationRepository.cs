using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;

namespace CogStayMVC.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly HotelDbContext _context;

    public ReservationRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Include(r => r.StayRecord)
            .FirstOrDefaultAsync(r => r.ReservationId == id);
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Include(r => r.StayRecord)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByGuestIdAsync(int guestId)
    {
        return await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Include(r => r.StayRecord)
            .Where(r => r.GuestId == guestId)
            .ToListAsync();
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
    }
}
