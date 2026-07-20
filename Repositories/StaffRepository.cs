using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;

namespace CogStayMVC.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly HotelDbContext _context;

    public StaffRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Staff?> GetByIdAsync(int id)
    {
        return await _context.Staff.FindAsync(id);
    }

    public async Task<Staff?> GetByEmailAsync(string email)
    {
        return await _context.Staff
            .FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<IEnumerable<Staff>> GetAllAsync()
    {
        return await _context.Staff.ToListAsync();
    }

    public async Task AddAsync(Staff staff)
    {
        await _context.Staff.AddAsync(staff);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Staff staff)
    {
        _context.Staff.Update(staff);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var staff = await _context.Staff.FindAsync(id);
        if (staff != null)
        {
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
        }
    }
}
