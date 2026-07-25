using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Repositories.Interfaces;

namespace CogStayMVC.Repositories.Implementations;

/// <summary>
/// Implements generic CRUD and query database operations for entity T.
/// </summary>
/// <typeparam name="T">A class representing a database model entity.</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    /// <summary>
    /// The hotel database context instance.
    /// </summary>
    protected readonly HotelDbContext _context;

    /// <summary>
    /// The entity-specific DbSet for operations.
    /// </summary>
    protected readonly DbSet<T> _dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{T}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public Repository(HotelDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    /// <summary>
    /// Gets all records of entity type T.
    /// </summary>
    /// <returns>Collection of entities.</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Searches for matching records using a predicate filter expression.
    /// </summary>
    /// <param name="predicate">Evaluation expression.</param>
    /// <returns>Collection of matching entities.</returns>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Finds a specific record by its primary key ID.
    /// </summary>
    /// <param name="id">Primary key ID.</param>
    /// <returns>Entity record T, or null.</returns>
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Adds a record of type T and saves changes to the database.
    /// </summary>
    /// <param name="entity">New entity instance.</param>
    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates a record of type T and saves changes to the database.
    /// </summary>
    /// <param name="entity">Existing entity instance with modified fields.</param>
    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a record by ID and saves changes to the database.
    /// </summary>
    /// <param name="id">Primary key ID of the record to delete.</param>
    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
