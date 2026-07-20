using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public ReservationService(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _reservationRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
    {
        return await _reservationRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByGuestIdAsync(int guestId)
    {
        return await _reservationRepository.GetByGuestIdAsync(guestId);
    }

    public async Task<Reservation> CreateReservationAsync(Reservation reservation)
    {
        var room = await _roomRepository.GetByIdAsync(reservation.RoomId);
        if (room == null)
        {
            throw new KeyNotFoundException("The selected room does not exist.");
        }

        // Business rule: Guest cannot reserve unavailable or occupied/under maintenance room
        if (room.Status != RoomStatus.Available)
        {
            throw new InvalidOperationException("The selected room is not available for booking.");
        }

        if (reservation.CheckInDate >= reservation.CheckOutDate)
        {
            throw new InvalidOperationException("Check-in date must be prior to check-out date.");
        }

        reservation.ReservationStatus = ReservationStatus.Booked;
        await _reservationRepository.AddAsync(reservation);
        return reservation;
    }

    public async Task<bool> CancelReservationAsync(int id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
        {
            throw new KeyNotFoundException("Reservation not found.");
        }

        if (reservation.StayRecord != null && reservation.StayRecord.ActualCheckIn != null)
        {
            throw new InvalidOperationException("Cannot cancel a reservation that has already checked in.");
        }

        reservation.ReservationStatus = ReservationStatus.Cancelled;
        await _reservationRepository.UpdateAsync(reservation);
        return true;
    }

    public async Task<IEnumerable<Reservation>> SearchReservationsAsync(string? query)
    {
        var list = await _reservationRepository.GetAllAsync();
        if (string.IsNullOrWhiteSpace(query))
        {
            return list;
        }

        query = query.Trim().ToLower();
        return list.Where(r => 
            r.ReservationId.ToString().Contains(query) || 
            r.Guest.FullName.ToLower().Contains(query) || 
            r.Room.RoomNumber.ToLower().Contains(query) ||
            r.ReservationStatus.ToString().ToLower().Contains(query)
        ).ToList();
    }
}
