using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Services;

public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;

    public StaffService(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public async Task<Staff?> GetByIdAsync(int id)
    {
        return await _staffRepository.GetByIdAsync(id);
    }

    public async Task<Staff?> AuthenticateAsync(string email, string password)
    {
        var staff = await _staffRepository.GetByEmailAsync(email);
        if (staff == null) return null;

        bool isValid = BCrypt.Net.BCrypt.Verify(password, staff.PasswordHash);
        return isValid ? staff : null;
    }

    public async Task<IEnumerable<Staff>> GetAllStaffAsync()
    {
        return await _staffRepository.GetAllAsync();
    }

    public async Task<Staff> CreateStaffAsync(Staff staff, string password)
    {
        var existing = await _staffRepository.GetByEmailAsync(staff.Email);
        if (existing != null)
        {
            throw new InvalidOperationException("Email address is already in use by another staff profile.");
        }

        staff.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        staff.CreatedAt = DateTime.UtcNow;
        staff.IsActive = true;

        await _staffRepository.AddAsync(staff);
        return staff;
    }

    public async Task UpdateStaffAsync(Staff staff)
    {
        var dbStaff = await _staffRepository.GetByIdAsync(staff.StaffId);
        if (dbStaff == null)
        {
            throw new KeyNotFoundException("Staff profile not found.");
        }

        dbStaff.FullName = staff.FullName;
        dbStaff.PhoneNumber = staff.PhoneNumber;
        dbStaff.Role = staff.Role;
        dbStaff.IsActive = staff.IsActive;

        if (dbStaff.Email != staff.Email)
        {
            var other = await _staffRepository.GetByEmailAsync(staff.Email);
            if (other != null)
            {
                throw new InvalidOperationException("Email address is already in use.");
            }
            dbStaff.Email = staff.Email;
        }

        // Hash new password if supplied
        if (!string.IsNullOrEmpty(staff.PasswordHash) && staff.PasswordHash != dbStaff.PasswordHash)
        {
            dbStaff.PasswordHash = BCrypt.Net.BCrypt.HashPassword(staff.PasswordHash);
        }

        await _staffRepository.UpdateAsync(dbStaff);
    }

    public async Task DeleteStaffAsync(int id)
    {
        await _staffRepository.DeleteAsync(id);
    }
}
