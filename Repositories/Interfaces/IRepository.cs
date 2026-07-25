using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CogStayMVC.Repositories.Interfaces;

/// <summary>
/// Defines generic data-access operations for any database entity type T.
/// </summary>
/// <typeparam name="T">The type of the domain model entity class.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all records of entity T from the database.
    /// </summary>
    /// <returns>A collection of entity records.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Searches for records of entity T that match the specified predicate expression.
    /// </summary>
    /// <param name="predicate">A filter expression to evaluate against database entities.</param>
    /// <returns>A collection of matching entity records.</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Retrieves a single record of entity T by its unique integer identifier.
    /// </summary>
    /// <param name="id">The primary key ID of the entity record.</param>
    /// <returns>The matching entity record, or null if not found.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Inserts a new entity record into the database table and saves changes.
    /// </summary>
    /// <param name="entity">The entity instance to add.</param>
    Task AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity record in the database table and saves changes.
    /// </summary>
    /// <param name="entity">The entity instance containing updated values.</param>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity record by its unique identifier and saves changes.
    /// </summary>
    /// <param name="id">The primary key ID of the entity record to delete.</param>
    Task DeleteAsync(int id);
}
