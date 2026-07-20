using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Repositories.Interfaces;

public interface IStaffRepository
{
    Task<Staff?> GetByIdAsync(int id);
    Task<Staff?> GetByEmailAsync(string email);
    Task<IEnumerable<Staff>> GetAllAsync();
    Task AddAsync(Staff staff);
    Task UpdateAsync(Staff staff);
    Task DeleteAsync(int id);
}
