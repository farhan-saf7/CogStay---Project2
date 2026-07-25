using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Enums;
using CogStayMVC.Models;

namespace CogStayMVC.Repositories.Interfaces;

/// <summary>
/// Defines data-access operations specific to Guest entities.
/// </summary>
public interface IGuestRepository : IRepository<Guest>
{
    /// <summary>
    /// Gets a guest record by email address.
    /// </summary>
    /// <param name="email">Unique email identifier.</param>
    /// <returns>Matching Guest record, or null if not found.</returns>
    Task<Guest?> GetByEmailAsync(string email);
}

/// <summary>
/// Defines data-access operations specific to Room entities.
/// </summary>
public interface IRoomRepository : IRepository<Room>
{
    /// <summary>
    /// Gets a room record by room number.
    /// </summary>
    /// <param name="roomNumber">Unique room number string.</param>
    /// <returns>Matching Room record, or null if not found.</returns>
    Task<Room?> GetByRoomNumberAsync(string roomNumber);

    /// <summary>
    /// Retrieves rooms matching the specified operational status.
    /// </summary>
    /// <param name="status">RoomStatus enum value.</param>
    /// <returns>Collection of rooms.</returns>
    Task<IEnumerable<Room>> GetRoomsByStatusAsync(RoomStatus status);
}

/// <summary>
/// Defines data-access operations specific to Reservation entities.
/// </summary>
public interface IReservationRepository : IRepository<Reservation>
{
    /// <summary>
    /// Retrieves all reservations including guest and room navigation entities.
    /// </summary>
    /// <returns>Collection of detailed reservations.</returns>
    Task<IEnumerable<Reservation>> GetReservationsWithDetailsAsync();

    /// <summary>
    /// Retrieves a single reservation details by its ID, including related entities.
    /// </summary>
    /// <param name="id">Reservation identifier.</param>
    /// <returns>Detailed reservation record, or null.</returns>
    Task<Reservation?> GetReservationWithDetailsAsync(int id);

    /// <summary>
    /// Retrieves all reservations made by a specific guest.
    /// </summary>
    /// <param name="guestId">Guest identifier.</param>
    /// <returns>Collection of detailed reservations.</returns>
    Task<IEnumerable<Reservation>> GetReservationsByGuestAsync(int guestId);
}

/// <summary>
/// Defines data-access operations specific to StayRecord entities.
/// </summary>
public interface IStayRecordRepository : IRepository<StayRecord>
{
    /// <summary>
    /// Retrieves all stay records including related Guest, Reservation, and Billing objects.
    /// </summary>
    /// <returns>Collection of detailed stay records.</returns>
    Task<IEnumerable<StayRecord>> GetStayRecordsWithDetailsAsync();

    /// <summary>
    /// Retrieves detailed stay record details by its primary ID.
    /// </summary>
    /// <param name="id">StayRecord identifier.</param>
    /// <returns>Detailed stay record, or null.</returns>
    Task<StayRecord?> GetStayRecordWithDetailsAsync(int id);

    /// <summary>
    /// Retrieves stay record linked to a specific reservation ID.
    /// </summary>
    /// <param name="reservationId">Reservation identifier.</param>
    /// <returns>StayRecord, or null.</returns>
    Task<StayRecord?> GetStayRecordByReservationAsync(int reservationId);
}

/// <summary>
/// Defines data-access operations specific to Billing entities.
/// </summary>
public interface IBillingRepository : IRepository<Billing>
{
    /// <summary>
    /// Retrieves all bills with guest and room details.
    /// </summary>
    /// <returns>Collection of bills.</returns>
    Task<IEnumerable<Billing>> GetBillingsWithDetailsAsync();

    /// <summary>
    /// Retrieves detailed invoice details by bill ID.
    /// </summary>
    /// <param name="id">Bill identifier.</param>
    /// <returns>Detailed bill, or null.</returns>
    Task<Billing?> GetBillingWithDetailsAsync(int id);

    /// <summary>
    /// Retrieves the billing record generated for a stay record ID.
    /// </summary>
    /// <param name="stayId">Stay record identifier.</param>
    /// <returns>Billing record, or null.</returns>
    Task<Billing?> GetBillingByStayIdAsync(int stayId);
}

/// <summary>
/// Defines data-access operations specific to HousekeepingTask entities.
/// </summary>
public interface IHousekeepingTaskRepository : IRepository<HousekeepingTask>
{
    /// <summary>
    /// Retrieves all housekeeping tasks along with room details.
    /// </summary>
    /// <returns>Collection of tasks.</returns>
    Task<IEnumerable<HousekeepingTask>> GetTasksWithDetailsAsync();

    /// <summary>
    /// Retrieves housekeeping task details by ID.
    /// </summary>
    /// <param name="id">Task identifier.</param>
    /// <returns>Housekeeping task, or null.</returns>
    Task<HousekeepingTask?> GetTaskWithDetailsAsync(int id);

    /// <summary>
    /// Retrieves all tasks assigned to a specific room.
    /// </summary>
    /// <param name="roomId">Room identifier.</param>
    /// <returns>Collection of housekeeping tasks.</returns>
    Task<IEnumerable<HousekeepingTask>> GetTasksByRoomIdAsync(int roomId);
}

/// <summary>
/// Defines data-access operations specific to Staff entities.
/// </summary>
public interface IStaffRepository : IRepository<Staff>
{
    /// <summary>
    /// Retrieves a staff member by email address.
    /// </summary>
    /// <param name="email">Staff login email.</param>
    /// <returns>Staff member record, or null.</returns>
    Task<Staff?> GetByEmailAsync(string email);
}

/// <summary>
/// Defines data-access operations specific to Feedback entities.
/// </summary>
public interface IFeedbackRepository : IRepository<Feedback>
{
    /// <summary>
    /// Retrieves feedbacks along with guest and reservation descriptions.
    /// </summary>
    /// <returns>Collection of feedback comments.</returns>
    Task<IEnumerable<Feedback>> GetFeedbacksWithDetailsAsync();
}
