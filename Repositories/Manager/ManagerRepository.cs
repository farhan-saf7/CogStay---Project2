using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Repositories.Implementations;

namespace CogStayMVC.Repositories.Manager;

/// <summary>
/// Repository implementation managing Guest Feedback queries.
/// </summary>
public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FeedbackRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public FeedbackRepository(HotelDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves guest feedback records with preloaded Guest, Reservation, and Room details, ordered descending by creation date.
    /// </summary>
    /// <returns>A collection of detailed feedback records.</returns>
    public async Task<IEnumerable<Feedback>> GetFeedbacksWithDetailsAsync()
    {
        return await _dbSet
            .Include(f => f.Guest)
            .Include(f => f.Reservation)
                .ThenInclude(r => r!.Room)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }
}
