using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Services.Interfaces;

public interface IReservationService
{
    Task<Reservation?> GetByIdAsync(int id);
    Task<IEnumerable<Reservation>> GetAllReservationsAsync();
    Task<IEnumerable<Reservation>> GetByGuestIdAsync(int guestId);
    Task<Reservation> CreateReservationAsync(Reservation reservation);
    Task<bool> CancelReservationAsync(int id);
    Task<IEnumerable<Reservation>> SearchReservationsAsync(string? query);
}
