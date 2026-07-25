using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Repositories.Implementations;

namespace CogStayMVC.Repositories.FrontDesk;

/// <summary>
/// Repository implementation managing Room Reservation database queries.
/// </summary>
public class ReservationRepository : Repository<Reservation>, IReservationRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReservationRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ReservationRepository(HotelDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves all reservations with fully loaded Guest, Room, and StayRecord navigation details.
    /// </summary>
    /// <returns>A collection of detailed reservations.</returns>
    public async Task<IEnumerable<Reservation>> GetReservationsWithDetailsAsync()
    {
        return await _dbSet
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Include(r => r.StayRecord)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a single reservation by ID with fully loaded Guest, Room, and StayRecord navigation details.
    /// </summary>
    /// <param name="id">Reservation primary key ID.</param>
    /// <returns>The detailed reservation record, or null.</returns>
    public async Task<Reservation?> GetReservationWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Include(r => r.StayRecord)
            .FirstOrDefaultAsync(r => r.ReservationId == id);
    }

    /// <summary>
    /// Retrieves all reservations placed by a specific guest.
    /// </summary>
    /// <param name="guestId">Guest identifier.</param>
    /// <returns>A collection of reservations with room and stay record details.</returns>
    public async Task<IEnumerable<Reservation>> GetReservationsByGuestAsync(int guestId)
    {
        return await _dbSet
            .Include(r => r.Room)
            .Include(r => r.StayRecord)
            .Where(r => r.GuestId == guestId)
            .ToListAsync();
    }
}

/// <summary>
/// Repository implementation managing Guest Stay Record database queries.
/// </summary>
public class StayRecordRepository : Repository<StayRecord>, IStayRecordRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StayRecordRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public StayRecordRepository(HotelDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves all guest stays with Guest, Reservation, Room, and Billing details preloaded.
    /// </summary>
    /// <returns>A collection of detailed stay records.</returns>
    public async Task<IEnumerable<StayRecord>> GetStayRecordsWithDetailsAsync()
    {
        return await _dbSet
            .Include(s => s.Guest)
            .Include(s => s.Reservation)
                .ThenInclude(r => r.Room)
            .Include(s => s.Billing)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific guest stay by ID with Guest, Reservation, Room, and Billing details preloaded.
    /// </summary>
    /// <param name="id">StayRecord primary key ID.</param>
    /// <returns>Detailed stay record, or null.</returns>
    public async Task<StayRecord?> GetStayRecordWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Guest)
            .Include(s => s.Reservation)
                .ThenInclude(r => r.Room)
            .Include(s => s.Billing)
            .FirstOrDefaultAsync(s => s.StayId == id);
    }

    /// <summary>
    /// Retrieves a guest stay record linked to a specific reservation ID.
    /// </summary>
    /// <param name="reservationId">Reservation primary key ID.</param>
    /// <returns>StayRecord, or null.</returns>
    public async Task<StayRecord?> GetStayRecordByReservationAsync(int reservationId)
    {
        return await _dbSet
            .Include(s => s.Guest)
            .Include(s => s.Reservation)
                .ThenInclude(r => r.Room)
            .Include(s => s.Billing)
            .FirstOrDefaultAsync(s => s.ReservationId == reservationId);
    }
}

/// <summary>
/// Repository implementation managing Billing and invoice database queries.
/// </summary>
public class BillingRepository : Repository<Billing>, IBillingRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BillingRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public BillingRepository(HotelDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves all invoices with fully loaded Guest, Stay, Reservation, and Room details.
    /// </summary>
    /// <returns>A collection of detailed bills.</returns>
    public async Task<IEnumerable<Billing>> GetBillingsWithDetailsAsync()
    {
        return await _dbSet
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Guest)
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Reservation)
                    .ThenInclude(r => r.Room)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves detailed invoice by its ID.
    /// </summary>
    /// <param name="id">Bill primary key ID.</param>
    /// <returns>Detailed bill record, or null.</returns>
    public async Task<Billing?> GetBillingWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Guest)
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Reservation)
                    .ThenInclude(r => r.Room)
            .FirstOrDefaultAsync(b => b.BillId == id);
    }

    /// <summary>
    /// Retrieves invoice details generated for a specific stay.
    /// </summary>
    /// <param name="stayId">Stay record primary key ID.</param>
    /// <returns>Detailed bill record, or null.</returns>
    public async Task<Billing?> GetBillingByStayIdAsync(int stayId)
    {
        return await _dbSet
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Guest)
            .Include(b => b.StayRecord)
                .ThenInclude(s => s.Reservation)
                    .ThenInclude(r => r.Room)
            .FirstOrDefaultAsync(b => b.StayId == stayId);
    }
}
