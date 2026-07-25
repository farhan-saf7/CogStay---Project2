using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.DTOs;
using CogStayMVC.Enums;
using TaskStatus = CogStayMVC.Enums.TaskStatus;

namespace CogStayMVC.Services.Interfaces;

/// <summary>
/// Defines business services for managing guest registrations, validations, and lookups.
/// </summary>
public interface IGuestService
{
    /// <summary>
    /// Retrieves all guests.
    /// </summary>
    /// <returns>A collection of guest response DTOs.</returns>
    Task<IEnumerable<GuestResponseDTO>> GetAllGuestsAsync();

    /// <summary>
    /// Retrieves a guest by ID.
    /// </summary>
    /// <param name="id">Guest identifier.</param>
    /// <returns>Guest response, or null if not found.</returns>
    Task<GuestResponseDTO?> GetGuestByIdAsync(int id);

    /// <summary>
    /// Retrieves a guest by email.
    /// </summary>
    /// <param name="email">Unique guest email.</param>
    /// <returns>Guest response, or null if not found.</returns>
    Task<GuestResponseDTO?> GetGuestByEmailAsync(string email);

    /// <summary>
    /// Registers a new guest in the system.
    /// </summary>
    /// <param name="dto">Create guest parameters.</param>
    /// <returns>Newly created guest response.</returns>
    Task<GuestResponseDTO> RegisterGuestAsync(CreateGuestDTO dto);

    /// <summary>
    /// Validates guest login credentials.
    /// </summary>
    /// <param name="dto">Login DTO.</param>
    /// <returns>Guest details if valid, else null.</returns>
    Task<GuestResponseDTO?> ValidateGuestLoginAsync(GuestLoginDTO dto);

    /// <summary>
    /// Updates guest profile information.
    /// </summary>
    /// <param name="dto">Updated profile parameters.</param>
    Task UpdateGuestAsync(UpdateGuestDTO dto);

    /// <summary>
    /// Deletes a guest by their ID.
    /// </summary>
    /// <param name="id">Guest identifier.</param>
    Task DeleteGuestAsync(int id);
}

/// <summary>
/// Defines business services for room inventory configuration and availability lookups.
/// </summary>
public interface IRoomService
{
    /// <summary>
    /// Retrieves all rooms.
    /// </summary>
    /// <returns>Collection of rooms.</returns>
    Task<IEnumerable<RoomResponseDTO>> GetAllRoomsAsync();

    /// <summary>
    /// Retrieves rooms that are currently available.
    /// </summary>
    /// <returns>Collection of available rooms.</returns>
    Task<IEnumerable<RoomResponseDTO>> GetAvailableRoomsAsync();

    /// <summary>
    /// Retrieves a room by its ID.
    /// </summary>
    /// <param name="id">Room identifier.</param>
    /// <returns>Room details, or null.</returns>
    Task<RoomResponseDTO?> GetRoomByIdAsync(int id);

    /// <summary>
    /// Creates a new hotel room.
    /// </summary>
    /// <param name="dto">Room creation parameters.</param>
    /// <returns>Newly created room details.</returns>
    Task<RoomResponseDTO> CreateRoomAsync(CreateRoomDTO dto);

    /// <summary>
    /// Updates room details (type, number, pricing).
    /// </summary>
    /// <param name="dto">Updated room parameters.</param>
    Task UpdateRoomAsync(UpdateRoomDTO dto);

    /// <summary>
    /// Changes the operational status of a room.
    /// </summary>
    /// <param name="roomId">Room identifier.</param>
    /// <param name="status">New RoomStatus value.</param>
    Task UpdateRoomStatusAsync(int roomId, RoomStatus status);

    /// <summary>
    /// Deletes a room from the inventory.
    /// </summary>
    /// <param name="id">Room identifier.</param>
    Task DeleteRoomAsync(int id);
}

/// <summary>
/// Defines business services for managing room bookings.
/// </summary>
public interface IReservationService
{
    /// <summary>
    /// Retrieves all reservations.
    /// </summary>
    /// <returns>Collection of reservations.</returns>
    Task<IEnumerable<ReservationResponseDTO>> GetAllReservationsAsync();

    /// <summary>
    /// Retrieves a specific reservation by ID.
    /// </summary>
    /// <param name="id">Reservation ID.</param>
    /// <returns>Reservation details, or null.</returns>
    Task<ReservationResponseDTO?> GetReservationByIdAsync(int id);

    /// <summary>
    /// Retrieves all reservations placed by a guest.
    /// </summary>
    /// <param name="guestId">Guest identifier.</param>
    /// <returns>Collection of reservations.</returns>
    Task<IEnumerable<ReservationResponseDTO>> GetReservationsByGuestAsync(int guestId);

    /// <summary>
    /// Books a room, performing collision checks on dates.
    /// </summary>
    /// <param name="dto">Reservation parameters.</param>
    /// <returns>Completed reservation details.</returns>
    Task<ReservationResponseDTO> BookRoomAsync(CreateReservationDTO dto);

    /// <summary>
    /// Updates an existing reservation.
    /// </summary>
    /// <param name="dto">Update parameters.</param>
    Task UpdateReservationAsync(UpdateReservationDTO dto);

    /// <summary>
    /// Cancels a reservation and frees the associated room.
    /// </summary>
    /// <param name="reservationId">Reservation ID to cancel.</param>
    Task CancelReservationAsync(int reservationId);

    /// <summary>
    /// Deletes a reservation record from the database.
    /// </summary>
    /// <param name="id">Reservation ID.</param>
    Task DeleteReservationAsync(int id);
}

/// <summary>
/// Defines business services for guest check-in stays and checkout triggers.
/// </summary>
public interface ICheckInService
{
    /// <summary>
    /// Retrieves all stay records.
    /// </summary>
    /// <returns>Collection of stays.</returns>
    Task<IEnumerable<StayRecordResponseDTO>> GetAllStaysAsync();

    /// <summary>
    /// Retrieves a stay record by its ID.
    /// </summary>
    /// <param name="id">Stay record identifier.</param>
    /// <returns>Stay record details, or null.</returns>
    Task<StayRecordResponseDTO?> GetStayByIdAsync(int id);

    /// <summary>
    /// Retrieves stay record associated with a reservation ID.
    /// </summary>
    /// <param name="reservationId">Reservation ID.</param>
    /// <returns>Stay record, or null.</returns>
    Task<StayRecordResponseDTO?> GetStayByReservationIdAsync(int reservationId);

    /// <summary>
    /// Processes guest check-in, setting room status to Occupied.
    /// </summary>
    /// <param name="dto">Check-in parameters.</param>
    /// <returns>Activated stay details.</returns>
    Task<StayRecordResponseDTO> CheckInGuestAsync(CreateCheckInDTO dto);

    /// <summary>
    /// Requests a checkout for a stay record.
    /// </summary>
    /// <param name="stayId">Stay record identifier.</param>
    Task RequestCheckOutAsync(int stayId);

    /// <summary>
    /// Processes final checkout, releasing rooms.
    /// </summary>
    /// <param name="stayId">Stay record identifier.</param>
    Task CompleteCheckOutAsync(int stayId);

    /// <summary>
    /// Deletes a stay record.
    /// </summary>
    /// <param name="id">Stay record identifier.</param>
    Task DeleteStayAsync(int id);
}

/// <summary>
/// Defines business services for invoices and payment processing.
/// </summary>
public interface IBillingService
{
    /// <summary>
    /// Retrieves all bills.
    /// </summary>
    /// <returns>Collection of bills.</returns>
    Task<IEnumerable<BillingResponseDTO>> GetAllBillsAsync();

    /// <summary>
    /// Retrieves a bill by its ID.
    /// </summary>
    /// <param name="id">Bill identifier.</param>
    /// <returns>Bill details, or null.</returns>
    Task<BillingResponseDTO?> GetBillByIdAsync(int id);

    /// <summary>
    /// Retrieves a bill by the associated stay ID.
    /// </summary>
    /// <param name="stayId">Stay record identifier.</param>
    /// <returns>Bill details, or null.</returns>
    Task<BillingResponseDTO?> GetBillByStayIdAsync(int stayId);

    /// <summary>
    /// Generates a bill invoice automatically based on stay dates and room rate.
    /// </summary>
    /// <param name="stayId">Stay record identifier.</param>
    /// <param name="remarks">Optional invoice remarks.</param>
    /// <returns>Generated invoice details.</returns>
    Task<BillingResponseDTO> GenerateBillForStayAsync(int stayId, string? remarks = null);

    /// <summary>
    /// Creates a manual custom bill record.
    /// </summary>
    /// <param name="dto">Bill parameters.</param>
    /// <returns>Created bill details.</returns>
    Task<BillingResponseDTO> CreateBillAsync(CreateBillDTO dto);

    /// <summary>
    /// Processes a payment and triggers checkout release and cleaning cycles.
    /// </summary>
    /// <param name="dto">Payment details DTO.</param>
    Task ProcessPaymentAsync(ProcessPaymentDTO dto);

    /// <summary>
    /// Deletes a billing record.
    /// </summary>
    /// <param name="id">Bill identifier.</param>
    Task DeleteBillAsync(int id);
}

/// <summary>
/// Defines business services for managing room cleanliness tasks.
/// </summary>
public interface IHousekeepingService
{
    /// <summary>
    /// Retrieves all housekeeping tasks.
    /// </summary>
    /// <returns>Collection of tasks.</returns>
    Task<IEnumerable<HousekeepingTaskResponseDTO>> GetAllTasksAsync();

    /// <summary>
    /// Retrieves a housekeeping task by ID.
    /// </summary>
    /// <param name="id">Task identifier.</param>
    /// <returns>Task details, or null.</returns>
    Task<HousekeepingTaskResponseDTO?> GetTaskByIdAsync(int id);

    /// <summary>
    /// Retrieves housekeeping tasks assigned to a specific room.
    /// </summary>
    /// <param name="roomId">Room identifier.</param>
    /// <returns>Collection of tasks.</returns>
    Task<IEnumerable<HousekeepingTaskResponseDTO>> GetTasksByRoomIdAsync(int roomId);

    /// <summary>
    /// Creates a new housekeeping task assignment.
    /// </summary>
    /// <param name="dto">Task creation parameters.</param>
    /// <returns>Created task details.</returns>
    Task<HousekeepingTaskResponseDTO> CreateTaskAsync(CreateHousekeepingTaskDTO dto);

    /// <summary>
    /// Updates task progress and synchronizes room cleanliness flags.
    /// </summary>
    /// <param name="dto">Update status DTO containing task ID and status.</param>
    Task UpdateTaskStatusAsync(UpdateTaskStatusDTO dto);

    /// <summary>
    /// Deletes a housekeeping task.
    /// </summary>
    /// <param name="id">Task identifier.</param>
    Task DeleteTaskAsync(int id);
}

/// <summary>
/// Defines business services for managing staff credentials and logins.
/// </summary>
public interface IStaffService
{
    /// <summary>
    /// Retrieves all staff accounts.
    /// </summary>
    /// <returns>Collection of staff details.</returns>
    Task<IEnumerable<StaffResponseDTO>> GetAllStaffAsync();

    /// <summary>
    /// Retrieves a staff account by ID.
    /// </summary>
    /// <param name="id">Staff identifier.</param>
    /// <returns>Staff details, or null.</returns>
    Task<StaffResponseDTO?> GetStaffByIdAsync(int id);

    /// <summary>
    /// Creates a new staff member account.
    /// </summary>
    /// <param name="dto">Creation parameters.</param>
    /// <returns>Created staff details.</returns>
    Task<StaffResponseDTO> CreateStaffAsync(CreateStaffDTO dto);

    /// <summary>
    /// Updates staff account settings and roles.
    /// </summary>
    /// <param name="dto">Update parameters.</param>
    Task UpdateStaffAsync(UpdateStaffDTO dto);

    /// <summary>
    /// Deletes a staff account.
    /// </summary>
    /// <param name="id">Staff identifier.</param>
    Task DeleteStaffAsync(int id);

    /// <summary>
    /// Validates staff credentials for portal logins.
    /// </summary>
    /// <param name="dto">Login DTO details.</param>
    /// <returns>Staff response if credentials match, else null.</returns>
    Task<StaffResponseDTO?> ValidateStaffLoginAsync(StaffLoginDTO dto);
}

/// <summary>
/// Defines business services for submitting and reading guest feedback reviews.
/// </summary>
public interface IFeedbackService
{
    /// <summary>
    /// Retrieves all guest feedback comments.
    /// </summary>
    /// <returns>Collection of feedbacks.</returns>
    Task<IEnumerable<FeedbackResponseDTO>> GetAllFeedbacksAsync();

    /// <summary>
    /// Retrieves feedback by its ID.
    /// </summary>
    /// <param name="id">Feedback identifier.</param>
    /// <returns>Feedback details, or null.</returns>
    Task<FeedbackResponseDTO?> GetFeedbackByIdAsync(int id);

    /// <summary>
    /// Submits a guest feedback review.
    /// </summary>
    /// <param name="dto">Submission parameters.</param>
    /// <returns>Completed feedback details.</returns>
    Task<FeedbackResponseDTO> SubmitFeedbackAsync(CreateFeedbackDTO dto);

    /// <summary>
    /// Deletes a feedback entry.
    /// </summary>
    /// <param name="id">Feedback identifier.</param>
    Task DeleteFeedbackAsync(int id);
}
