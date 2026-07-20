using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Services;

public class StayRecordService : IStayRecordService
{
    private readonly IStayRecordRepository _stayRecordRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IBillingRepository _billingRepository;

    public StayRecordService(
        IStayRecordRepository stayRecordRepository,
        IReservationRepository reservationRepository,
        IRoomRepository roomRepository,
        IBillingRepository billingRepository)
    {
        _stayRecordRepository = stayRecordRepository;
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
        _billingRepository = billingRepository;
    }

    public async Task<StayRecord?> GetByIdAsync(int id)
    {
        return await _stayRecordRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<StayRecord>> GetAllStaysAsync()
    {
        return await _stayRecordRepository.GetAllAsync();
    }

    public async Task<IEnumerable<StayRecord>> GetActiveStaysAsync()
    {
        return await _stayRecordRepository.GetActiveStaysAsync();
    }

    public async Task<StayRecord> CheckInAsync(int reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
        {
            throw new KeyNotFoundException("Reservation record not found.");
        }

        if (reservation.ReservationStatus == ReservationStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot check in to a cancelled reservation.");
        }

        var existingStay = await _stayRecordRepository.GetByReservationIdAsync(reservationId);
        if (existingStay != null)
        {
            throw new InvalidOperationException("Guest has already checked in for this reservation.");
        }

        var room = await _roomRepository.GetByIdAsync(reservation.RoomId);
        if (room == null)
        {
            throw new KeyNotFoundException("Room not found.");
        }

        // Check in logic
        var stay = new StayRecord
        {
            ReservationId = reservationId,
            GuestId = reservation.GuestId,
            ActualCheckIn = DateTime.Now,
            ActualCheckOut = null
        };

        await _stayRecordRepository.AddAsync(stay);

        // Update room status to Occupied
        room.Status = RoomStatus.Occupied;
        await _roomRepository.UpdateAsync(room);

        return stay;
    }

    public async Task<StayRecord> CheckOutAsync(int stayId)
    {
        var stay = await _stayRecordRepository.GetByIdAsync(stayId);
        if (stay == null)
        {
            throw new KeyNotFoundException("Stay record not found.");
        }

        if (stay.ActualCheckOut != null)
        {
            throw new InvalidOperationException("Guest has already checked out.");
        }

        stay.ActualCheckOut = DateTime.Now;
        await _stayRecordRepository.UpdateAsync(stay);

        // Update room status back to Available
        var room = await _roomRepository.GetByIdAsync(stay.Reservation.RoomId);
        if (room != null)
        {
            room.Status = RoomStatus.Available;
            await _roomRepository.UpdateAsync(room);
        }

        // Business rule: Generate invoice automatically after checkout
        // Calculate: Number of Days * Room Price
        var checkInDate = stay.ActualCheckIn ?? stay.Reservation.CheckInDate;
        var checkOutDate = stay.ActualCheckOut.Value;
        
        var totalDays = (checkOutDate.Date - checkInDate.Date).Days;
        if (totalDays <= 0) totalDays = 1; // Minimum 1-day charge

        decimal roomPrice = room?.PricePerNight ?? 0;
        decimal totalAmount = totalDays * roomPrice;

        var existingBill = await _billingRepository.GetByStayIdAsync(stayId);
        if (existingBill == null)
        {
            var billing = new Billing
            {
                StayId = stayId,
                TotalAmount = totalAmount,
                PaymentStatus = PaymentStatus.Pending,
                Remarks = $"Auto-generated invoice for {totalDays} night stay."
            };
            await _billingRepository.AddAsync(billing);
        }

        return stay;
    }
}
