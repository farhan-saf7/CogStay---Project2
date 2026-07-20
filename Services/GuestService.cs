using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Services;

public class GuestService : IGuestService
{
    private readonly IGuestRepository _guestRepository;

    public GuestService(IGuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
    }

    public async Task<Guest?> GetByIdAsync(int id)
    {
        return await _guestRepository.GetByIdAsync(id);
    }

    public async Task<Guest?> AuthenticateAsync(string email, string password)
    {
        var guest = await _guestRepository.GetByEmailAsync(email);
        if (guest == null) return null;

        // Verify password using BCrypt
        bool isValid = BCrypt.Net.BCrypt.Verify(password, guest.PasswordHash);
        return isValid ? guest : null;
    }

    public async Task<Guest> RegisterAsync(Guest guest, string password)
    {
        var existing = await _guestRepository.GetByEmailAsync(guest.Email);
        if (existing != null)
        {
            throw new InvalidOperationException("Email address is already registered.");
        }

        // Hash password securely with BCrypt
        guest.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        guest.CreatedAt = DateTime.UtcNow;

        await _guestRepository.AddAsync(guest);
        return guest;
    }

    public async Task UpdateProfileAsync(Guest guest)
    {
        var dbGuest = await _guestRepository.GetByIdAsync(guest.GuestId);
        if (dbGuest == null)
        {
            throw new KeyNotFoundException("Guest profile not found.");
        }

        dbGuest.FullName = guest.FullName;
        dbGuest.PhoneNumber = guest.PhoneNumber;
        dbGuest.Address = guest.Address;

        // If email has changed, verify uniqueness
        if (dbGuest.Email != guest.Email)
        {
            var other = await _guestRepository.GetByEmailAsync(guest.Email);
            if (other != null)
            {
                throw new InvalidOperationException("Email address is already in use.");
            }
            dbGuest.Email = guest.Email;
        }

        // Save password if updated (if a new one is set, it will be hashed by controllers before calling this, 
        // or we can handle it directly if we copy it)
        if (!string.IsNullOrEmpty(guest.PasswordHash) && guest.PasswordHash != dbGuest.PasswordHash)
        {
            dbGuest.PasswordHash = BCrypt.Net.BCrypt.HashPassword(guest.PasswordHash);
        }

        await _guestRepository.UpdateAsync(dbGuest);
    }

    public async Task<IEnumerable<Guest>> GetAllGuestsAsync()
    {
        return await _guestRepository.GetAllAsync();
    }
}
