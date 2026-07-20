using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Services.Interfaces;

public interface IStaffService
{
    Task<Staff?> GetByIdAsync(int id);
    Task<Staff?> AuthenticateAsync(string email, string password);
    Task<IEnumerable<Staff>> GetAllStaffAsync();
    Task<Staff> CreateStaffAsync(Staff staff, string password);
    Task UpdateStaffAsync(Staff staff);
    Task DeleteStaffAsync(int id);
}
