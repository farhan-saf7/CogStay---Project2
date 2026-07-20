using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Services.Interfaces;

public interface IGuestService
{
    Task<Guest?> GetByIdAsync(int id);
    Task<Guest?> AuthenticateAsync(string email, string password);
    Task<Guest> RegisterAsync(Guest guest, string password);
    Task UpdateProfileAsync(Guest guest);
    Task<IEnumerable<Guest>> GetAllGuestsAsync();
}
