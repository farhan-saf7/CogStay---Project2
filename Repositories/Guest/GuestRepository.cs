using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Repositories.Implementations;

namespace CogStayMVC.Repositories.GuestModule;

/// <summary>
/// Repository implementation managing Guest entities.
/// Handles guest queries, registration verification, and email lookups.
/// </summary>
public class GuestRepository : Repository<Guest>, IGuestRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GuestRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public GuestRepository(HotelDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves a guest record by their login email address.
    /// </summary>
    /// <param name="email">Unique email identifier.</param>
    /// <returns>Matching Guest record, or null.</returns>
    public async Task<Guest?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(g => g.Email == email);
    }
}
