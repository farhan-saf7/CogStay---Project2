using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogStayMVC.DTOs;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;
using TaskStatus = CogStayMVC.Enums.TaskStatus;

namespace CogStayMVC.Services.FrontDesk;

/// <summary>
/// Service implementation managing customer room bookings and reservation validation.
/// Checks date ranges, overlapping bookings, guest existence, and room availability before saving reservations.
/// </summary>
public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IGuestRepository _guestRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReservationService"/> class.
    /// </summary>
    /// <param name="reservationRepository">Repository handling Reservation models.</param>
    /// <param name="roomRepository">Repository handling Room database models.</param>
    /// <param name="guestRepository">Repository managing Guest records.</param>
    public ReservationService(
        IReservationRepository reservationRepository,
        IRoomRepository roomRepository,
        IGuestRepository guestRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
        _guestRepository = guestRepository;
    }

    /// <summary>
    /// Retrieves all reservations in the system.
    /// </summary>
    /// <returns>Collection of reservation response DTOs.</returns>
    public async Task<IEnumerable<ReservationResponseDTO>> GetAllReservationsAsync()
    {
        var resList = await _reservationRepository.GetReservationsWithDetailsAsync();
        return resList.Select(MapToDTO);
    }

    /// <summary>
    /// Retrieves a single reservation details by ID.
    /// </summary>
    /// <param name="id">Reservation primary key ID.</param>
    /// <returns>The reservation details response DTO, or null.</returns>
    public async Task<ReservationResponseDTO?> GetReservationByIdAsync(int id)
    {
        var res = await _reservationRepository.GetReservationWithDetailsAsync(id);
        return res != null ? MapToDTO(res) : null;
    }

    /// <summary>
    /// Retrieves all bookings placed by a guest.
    /// </summary>
    /// <param name="guestId">Guest identifier.</param>
    /// <returns>Collection of reservation response DTOs.</returns>
    public async Task<IEnumerable<ReservationResponseDTO>> GetReservationsByGuestAsync(int guestId)
    {
        var resList = await _reservationRepository.GetReservationsByGuestAsync(guestId);
        return resList.Select(MapToDTO);
    }

    /// <summary>
    /// Registers a room booking. Performs check-in date rules and room status checks.
    /// </summary>
    /// <param name="dto">Create reservation parameters DTO.</param>
    /// <returns>Completed reservation details response.</returns>
    public async Task<ReservationResponseDTO> BookRoomAsync(CreateReservationDTO dto)
    {
        if (dto.CheckInDate >= dto.CheckOutDate)
        {
            throw new InvalidOperationException("Check-Out date must be after Check-In date.");
        }

        if (dto.CheckInDate.Date < DateTime.Today)
        {
            throw new InvalidOperationException("Check-In date cannot be in the past.");
        }

        var guest = await _guestRepository.GetByIdAsync(dto.GuestId);
        if (guest == null)
            throw new InvalidOperationException("Guest account not found.");

        var room = await _roomRepository.GetByIdAsync(dto.RoomId);
        if (room == null)
            throw new InvalidOperationException("Selected room not found.");

        if (room.Status != RoomStatus.Available)
        {
            throw new InvalidOperationException($"Room {room.RoomNumber} is currently not available for booking (Status: {room.Status}).");
        }

        // Check overlapping active reservations
        var existingReservations = await _reservationRepository.FindAsync(r =>
            r.RoomId == dto.RoomId &&
            r.ReservationStatus == ReservationStatus.Booked &&
            !(dto.CheckOutDate <= r.CheckInDate || dto.CheckInDate >= r.CheckOutDate));

        if (existingReservations.Any())
        {
            throw new InvalidOperationException($"Room {room.RoomNumber} is already booked for the selected dates.");
        }

        var reservation = new Reservation
        {
            GuestId = dto.GuestId,
            GuestName = guest.FullName,
            RoomId = dto.RoomId,
            CheckInDate = dto.CheckInDate,
            CheckOutDate = dto.CheckOutDate,
            ReservationStatus = ReservationStatus.Booked
        };

        await _reservationRepository.AddAsync(reservation);

        // Transition room status to Booked
        room.Status = RoomStatus.Booked;
        await _roomRepository.UpdateAsync(room);

        var saved = await _reservationRepository.GetReservationWithDetailsAsync(reservation.ReservationId);
        return MapToDTO(saved ?? reservation);
    }

    /// <summary>
    /// Updates check-in/out dates or status on an existing booking.
    /// </summary>
    /// <param name="dto">Update parameters DTO.</param>
    public async Task UpdateReservationAsync(UpdateReservationDTO dto)
    {
        var res = await _reservationRepository.GetByIdAsync(dto.ReservationId);
        if (res == null)
            throw new KeyNotFoundException("Reservation not found.");

        res.CheckInDate = dto.CheckInDate;
        res.CheckOutDate = dto.CheckOutDate;
        res.ReservationStatus = dto.ReservationStatus;

        await _reservationRepository.UpdateAsync(res);
    }

    /// <summary>
    /// Cancels a booked reservation and releases the associated room.
    /// </summary>
    /// <param name="reservationId">Reservation ID to cancel.</param>
    public async Task CancelReservationAsync(int reservationId)
    {
        var res = await _reservationRepository.GetReservationWithDetailsAsync(reservationId);
        if (res == null)
            throw new KeyNotFoundException("Reservation not found.");

        res.ReservationStatus = ReservationStatus.Cancelled;
        await _reservationRepository.UpdateAsync(res);

        if (res.Room != null && res.Room.Status == RoomStatus.Booked)
        {
            res.Room.Status = RoomStatus.Available;
            await _roomRepository.UpdateAsync(res.Room);
        }
    }

    /// <summary>
    /// Deletes a reservation record from the database.
    /// </summary>
    /// <param name="id">Reservation ID.</param>
    public async Task DeleteReservationAsync(int id)
    {
        await _reservationRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Maps a Reservation model to a ReservationResponseDTO.
    /// </summary>
    private static ReservationResponseDTO MapToDTO(Reservation res) => new()
    {
        ReservationId = res.ReservationId,
        GuestId = res.GuestId,
        GuestName = string.IsNullOrEmpty(res.GuestName) ? (res.Guest?.FullName ?? "Unknown") : res.GuestName,
        RoomId = res.RoomId,
        RoomNumber = res.Room?.RoomNumber ?? "N/A",
        RoomType = res.Room?.RoomType ?? "N/A",
        PricePerNight = res.Room?.PricePerNight ?? 0,
        CheckInDate = res.CheckInDate,
        CheckOutDate = res.CheckOutDate,
        ReservationStatus = res.ReservationStatus
    };
}

/// <summary>
/// Service implementation managing guest arrivals (check-in stays) and departures (check-out triggers).
/// </summary>
public class CheckInService : ICheckInService
{
    private readonly IStayRecordRepository _stayRecordRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckInService"/> class.
    /// </summary>
    /// <param name="stayRecordRepository">Repository handling StayRecord models.</param>
    /// <param name="reservationRepository">Repository managing Reservation data.</param>
    /// <param name="roomRepository">Repository managing Room database models.</param>
    public CheckInService(
        IStayRecordRepository stayRecordRepository,
        IReservationRepository reservationRepository,
        IRoomRepository roomRepository)
    {
        _stayRecordRepository = stayRecordRepository;
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Retrieves all stays.
    /// </summary>
    /// <returns>Collection of stay record response DTOs.</returns>
    public async Task<IEnumerable<StayRecordResponseDTO>> GetAllStaysAsync()
    {
        var stays = await _stayRecordRepository.GetStayRecordsWithDetailsAsync();
        return stays.Select(MapToDTO);
    }

    /// <summary>
    /// Retrieves detailed stay info by stay ID.
    /// </summary>
    /// <param name="id">StayRecord ID.</param>
    /// <returns>The stay record details, or null.</returns>
    public async Task<StayRecordResponseDTO?> GetStayByIdAsync(int id)
    {
        var stay = await _stayRecordRepository.GetStayRecordWithDetailsAsync(id);
        return stay != null ? MapToDTO(stay) : null;
    }

    /// <summary>
    /// Retrieves stay record linked to a booking reservation.
    /// </summary>
    /// <param name="reservationId">Reservation ID.</param>
    /// <returns>Stay record details response, or null.</returns>
    public async Task<StayRecordResponseDTO?> GetStayByReservationIdAsync(int reservationId)
    {
        var stay = await _stayRecordRepository.GetStayRecordByReservationAsync(reservationId);
        return stay != null ? MapToDTO(stay) : null;
    }

    /// <summary>
    /// Processes guest check-in, setting room status to Occupied.
    /// </summary>
    /// <param name="dto">The check-in registration details.</param>
    /// <returns>Activated stay record details response.</returns>
    public async Task<StayRecordResponseDTO> CheckInGuestAsync(CreateCheckInDTO dto)
    {
        var reservation = await _reservationRepository.GetReservationWithDetailsAsync(dto.ReservationId);
        if (reservation == null)
            throw new InvalidOperationException("Reservation not found.");

        if (reservation.ReservationStatus != ReservationStatus.Booked)
        {
            throw new InvalidOperationException("Only confirmed reservations can be checked in.");
        }

        var existingStay = await _stayRecordRepository.GetStayRecordByReservationAsync(dto.ReservationId);
        if (existingStay != null)
        {
            throw new InvalidOperationException("Guest is already checked in for this reservation.");
        }

        var stay = new StayRecord
        {
            GuestId = reservation.GuestId,
            ReservationId = reservation.ReservationId,
            ActualCheckIn = DateTime.Now,
            GuestName = string.IsNullOrEmpty(reservation.GuestName) ? (reservation.Guest?.FullName ?? "Unknown") : reservation.GuestName,
            BookingReference = $"BK-{reservation.ReservationId}",
            BillingReference = "Pending",
            StayDetails = $"Room {reservation.Room?.RoomNumber ?? "N/A"} - Stay from {reservation.CheckInDate:yyyy-MM-dd} to {reservation.CheckOutDate:yyyy-MM-dd}"
        };

        await _stayRecordRepository.AddAsync(stay);

        // Update Room status to Occupied
        if (reservation.Room != null)
        {
            reservation.Room.Status = RoomStatus.Occupied;
            await _roomRepository.UpdateAsync(reservation.Room);
        }

        var result = await _stayRecordRepository.GetStayRecordWithDetailsAsync(stay.StayId);
        return MapToDTO(result ?? stay);
    }

    /// <summary>
    /// Flags room status to CheckoutPending, prompting front desk to check departures.
    /// </summary>
    /// <param name="stayId">Stay Record ID.</param>
    public async Task RequestCheckOutAsync(int stayId)
    {
        var stay = await _stayRecordRepository.GetStayRecordWithDetailsAsync(stayId);
        if (stay == null)
            throw new KeyNotFoundException("Stay record not found.");

        if (stay.Reservation?.Room != null)
        {
            stay.Reservation.Room.Status = RoomStatus.CheckoutPending;
            await _roomRepository.UpdateAsync(stay.Reservation.Room);
        }
    }

    /// <summary>
    /// Processes final checkout updates, releases room to CleaningRequired.
    /// </summary>
    /// <param name="stayId">Stay Record ID.</param>
    public async Task CompleteCheckOutAsync(int stayId)
    {
        var stay = await _stayRecordRepository.GetStayRecordWithDetailsAsync(stayId);
        if (stay == null)
            throw new KeyNotFoundException("Stay record not found.");

        stay.ActualCheckOut = DateTime.Now;
        await _stayRecordRepository.UpdateAsync(stay);

        if (stay.Reservation?.Room != null)
        {
            stay.Reservation.Room.Status = RoomStatus.CleaningRequired;
            await _roomRepository.UpdateAsync(stay.Reservation.Room);
        }
    }

    /// <summary>
    /// Deletes a specific stay record.
    /// </summary>
    /// <param name="id">StayRecord ID.</param>
    public async Task DeleteStayAsync(int id)
    {
        await _stayRecordRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Maps a StayRecord model to a StayRecordResponseDTO.
    /// </summary>
    private static StayRecordResponseDTO MapToDTO(StayRecord stay) => new()
    {
        StayId = stay.StayId,
        GuestId = stay.GuestId,
        GuestName = string.IsNullOrEmpty(stay.GuestName) ? (stay.Guest?.FullName ?? "Unknown") : stay.GuestName,
        ReservationId = stay.ReservationId,
        RoomNumber = stay.Reservation?.Room?.RoomNumber ?? "N/A",
        ActualCheckIn = stay.ActualCheckIn,
        ActualCheckOut = stay.ActualCheckOut,
        BookingReference = stay.BookingReference,
        BillingReference = stay.BillingReference,
        StayDetails = stay.StayDetails,
        Billing = stay.Billing != null ? new BillingResponseDTO
        {
            BillId = stay.Billing.BillId,
            StayId = stay.Billing.StayId,
            TotalAmount = stay.Billing.TotalAmount,
            PaymentStatus = stay.Billing.PaymentStatus,
            Remarks = stay.Billing.Remarks
        } : null
    };
}

/// <summary>
/// Service implementation managing guest bill invoicing and payment completions.
/// Triggers automatic housekeeping cleaning tasks upon payment completion.
/// </summary>
public class BillingService : IBillingService
{
    private readonly IBillingRepository _billingRepository;
    private readonly IStayRecordRepository _stayRecordRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IHousekeepingTaskRepository _housekeepingTaskRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="BillingService"/> class.
    /// </summary>
    /// <param name="billingRepository">Repository managing Billing invoice database queries.</param>
    /// <param name="stayRecordRepository">Repository managing StayRecord database queries.</param>
    /// <param name="roomRepository">Repository managing Room database queries.</param>
    /// <param name="housekeepingTaskRepository">Repository managing HousekeepingTask database queries.</param>
    public BillingService(
        IBillingRepository billingRepository,
        IStayRecordRepository stayRecordRepository,
        IRoomRepository roomRepository,
        IHousekeepingTaskRepository housekeepingTaskRepository)
    {
        _billingRepository = billingRepository;
        _stayRecordRepository = stayRecordRepository;
        _roomRepository = roomRepository;
        _housekeepingTaskRepository = housekeepingTaskRepository;
    }

    /// <summary>
    /// Retrieves all bills.
    /// </summary>
    /// <returns>Collection of bills.</returns>
    public async Task<IEnumerable<BillingResponseDTO>> GetAllBillsAsync()
    {
        var bills = await _billingRepository.GetBillingsWithDetailsAsync();
        return bills.Select(MapToDTO);
    }

    /// <summary>
    /// Retrieves a specific bill by ID.
    /// </summary>
    /// <param name="id">Bill identifier.</param>
    /// <returns>Detailed bill response, or null.</returns>
    public async Task<BillingResponseDTO?> GetBillByIdAsync(int id)
    {
        var bill = await _billingRepository.GetBillingWithDetailsAsync(id);
        return bill != null ? MapToDTO(bill) : null;
    }

    /// <summary>
    /// Retrieves a bill record associated with a specific stay record.
    /// </summary>
    /// <param name="stayId">Stay record ID.</param>
    /// <returns>Detailed bill response, or null.</returns>
    public async Task<BillingResponseDTO?> GetBillByStayIdAsync(int stayId)
    {
        var bill = await _billingRepository.GetBillingByStayIdAsync(stayId);
        return bill != null ? MapToDTO(bill) : null;
    }

    /// <summary>
    /// Automatically calculates total fees based on room pricing and nights stayed.
    /// Saves a pending billing invoice.
    /// </summary>
    /// <param name="stayId">Stay Record ID.</param>
    /// <param name="remarks">Optional invoice remarks.</param>
    /// <returns>Generated bill invoice details.</returns>
    public async Task<BillingResponseDTO> GenerateBillForStayAsync(int stayId, string? remarks = null)
    {
        var stay = await _stayRecordRepository.GetStayRecordWithDetailsAsync(stayId);
        if (stay == null)
            throw new InvalidOperationException("Stay record not found.");

        var existingBill = await _billingRepository.GetBillingByStayIdAsync(stayId);
        if (existingBill != null)
        {
            return MapToDTO(existingBill);
        }

        var res = stay.Reservation;
        int nights = (res.CheckOutDate - res.CheckInDate).Days;
        if (nights <= 0) nights = 1;

        decimal price = res.Room?.PricePerNight ?? 100;
        decimal totalAmount = price * nights;

        var bill = new Billing
        {
            StayId = stayId,
            GuestId = stay.GuestId,
            GuestName = string.IsNullOrEmpty(stay.GuestName) ? (stay.Guest?.FullName ?? "Unknown") : stay.GuestName,
            TotalAmount = totalAmount,
            PaymentStatus = PaymentStatus.Pending,
            Remarks = remarks ?? $"Room charge for {nights} night(s) @ {price:C}/night"
        };

        await _billingRepository.AddAsync(bill);

        if (stay != null)
        {
            stay.BillingReference = $"BILL-{bill.BillId}";
            await _stayRecordRepository.UpdateAsync(stay);
        }

        var created = await _billingRepository.GetBillingWithDetailsAsync(bill.BillId);
        return MapToDTO(created ?? bill);
    }

    /// <summary>
    /// Creates a manual customized bill invoice.
    /// </summary>
    /// <param name="dto">The bill parameters DTO.</param>
    /// <returns>Created bill details.</returns>
    public async Task<BillingResponseDTO> CreateBillAsync(CreateBillDTO dto)
    {
        var stay = await _stayRecordRepository.GetStayRecordWithDetailsAsync(dto.StayId);
        if (stay == null)
            throw new InvalidOperationException("Stay record not found.");

        var bill = new Billing
        {
            StayId = dto.StayId,
            GuestId = stay.GuestId,
            GuestName = string.IsNullOrEmpty(stay.GuestName) ? (stay.Guest?.FullName ?? "Unknown") : stay.GuestName,
            TotalAmount = dto.TotalAmount,
            PaymentStatus = PaymentStatus.Pending,
            Remarks = dto.Remarks
        };

        await _billingRepository.AddAsync(bill);

        stay.BillingReference = $"BILL-{bill.BillId}";
        await _stayRecordRepository.UpdateAsync(stay);

        var created = await _billingRepository.GetBillingWithDetailsAsync(bill.BillId);
        return MapToDTO(created ?? bill);
    }

    /// <summary>
    /// Settle guest payment, closes stay records, updates room status to Dirty/CleaningRequired,
    /// and AUTOMATICALLY registers a cleaning housekeeping task.
    /// </summary>
    /// <param name="dto">Payment processing parameters DTO.</param>
    public async Task ProcessPaymentAsync(ProcessPaymentDTO dto)
    {
        var bill = await _billingRepository.GetBillingWithDetailsAsync(dto.BillId);
        if (bill == null)
            throw new KeyNotFoundException("Bill record not found.");

        bill.PaymentStatus = PaymentStatus.Paid;
        bill.Remarks = $"{bill.Remarks} | Paid: {dto.Remarks}";
        await _billingRepository.UpdateAsync(bill);

        // Update stay check-out time & room workflow status
        var stay = bill.StayRecord;
        if (stay != null)
        {
            stay.ActualCheckOut = DateTime.Now;
            await _stayRecordRepository.UpdateAsync(stay);

            if (stay.Reservation?.Room != null)
            {
                var room = stay.Reservation.Room;
                // Transition room status to CleaningRequired
                room.Status = RoomStatus.CleaningRequired;
                await _roomRepository.UpdateAsync(room);

                // AUTOMATICALLY CREATE HOUSEKEEPING CLEANING TASK FOR HOUSEKEEPING MODULE
                var cleaningTask = new HousekeepingTask
                {
                    RoomId = room.RoomId,
                    TaskDescription = $"Room Cleaning Request following Guest Check-Out (Bill #{bill.BillId})",
                    TaskStatus = TaskStatus.Pending
                };
                await _housekeepingTaskRepository.AddAsync(cleaningTask);
            }
        }
    }

    /// <summary>
    /// Deletes a specific billing invoice.
    /// </summary>
    /// <param name="id">Bill identifier.</param>
    public async Task DeleteBillAsync(int id)
    {
        await _billingRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Maps a Billing model to a BillingResponseDTO.
    /// </summary>
    private static BillingResponseDTO MapToDTO(Billing bill) => new()
    {
        BillId = bill.BillId,
        StayId = bill.StayId,
        GuestName = string.IsNullOrEmpty(bill.GuestName) ? (bill.StayRecord?.Guest?.FullName ?? "Unknown") : bill.GuestName,
        RoomNumber = bill.StayRecord?.Reservation?.Room?.RoomNumber ?? "N/A",
        TotalAmount = bill.TotalAmount,
        PaymentStatus = bill.PaymentStatus,
        Remarks = bill.Remarks
    };
}
