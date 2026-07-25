using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CogStayMVC.DTOs;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Services.GuestModule;

/// <summary>
/// Service implementation handling Guest business operations, registration, profile updates, and secure login verification.
/// </summary>
public class GuestService : IGuestService
{
    private readonly IGuestRepository _guestRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuestService"/> class.
    /// </summary>
    /// <param name="guestRepository">Repository managing guest data access.</param>
    public GuestService(IGuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
    }

    /// <summary>
    /// Retrieves all guests.
    /// </summary>
    /// <returns>Collection of guest response DTOs.</returns>
    public async Task<IEnumerable<GuestResponseDTO>> GetAllGuestsAsync()
    {
        var guests = await _guestRepository.GetAllAsync();
        return guests.Select(MapToDTO);
    }

    /// <summary>
    /// Retrieves a single guest by their ID.
    /// </summary>
    /// <param name="id">Guest ID.</param>
    /// <returns>The guest response DTO, or null if not found.</returns>
    public async Task<GuestResponseDTO?> GetGuestByIdAsync(int id)
    {
        var guest = await _guestRepository.GetByIdAsync(id);
        return guest != null ? MapToDTO(guest) : null;
    }

    /// <summary>
    /// Retrieves a guest by their unique email.
    /// </summary>
    /// <param name="email">Unique guest email.</param>
    /// <returns>The guest response DTO, or null if not found.</returns>
    public async Task<GuestResponseDTO?> GetGuestByEmailAsync(string email)
    {
        var guest = await _guestRepository.GetByEmailAsync(email);
        return guest != null ? MapToDTO(guest) : null;
    }

    /// <summary>
    /// Registers a new guest and hashes their password for database storage.
    /// </summary>
    /// <param name="dto">Create guest DTO containing email, password, address, phone.</param>
    /// <returns>Created guest response details.</returns>
    public async Task<GuestResponseDTO> RegisterGuestAsync(CreateGuestDTO dto)
    {
        var existing = await _guestRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
        {
            throw new InvalidOperationException("A guest with this email already exists.");
        }

        var guest = new Guest
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            PasswordHash = HashPassword(dto.Password),
            CreatedAt = DateTime.Now
        };

        await _guestRepository.AddAsync(guest);
        return MapToDTO(guest);
    }

    /// <summary>
    /// Validates guest login credentials.
    /// </summary>
    /// <param name="dto">Login credentials DTO.</param>
    /// <returns>Guest details response on successful matching, otherwise null.</returns>
    public async Task<GuestResponseDTO?> ValidateGuestLoginAsync(GuestLoginDTO dto)
    {
        var guest = await _guestRepository.GetByEmailAsync(dto.Email);
        if (guest == null || guest.PasswordHash != HashPassword(dto.Password))
        {
            return null;
        }

        return MapToDTO(guest);
    }

    /// <summary>
    /// Updates guest profile information in the database.
    /// </summary>
    /// <param name="dto">Updated profile fields DTO.</param>
    public async Task UpdateGuestAsync(UpdateGuestDTO dto)
    {
        var guest = await _guestRepository.GetByIdAsync(dto.GuestId);
        if (guest == null)
            throw new KeyNotFoundException("Guest not found.");

        guest.FullName = dto.FullName;
        guest.Email = dto.Email;
        guest.PhoneNumber = dto.PhoneNumber;
        guest.Address = dto.Address;

        await _guestRepository.UpdateAsync(guest);
    }

    /// <summary>
    /// Deletes a specific guest by ID.
    /// </summary>
    /// <param name="id">Guest identifier.</param>
    public async Task DeleteGuestAsync(int id)
    {
        await _guestRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Maps a Guest model to a GuestResponseDTO.
    /// </summary>
    private static GuestResponseDTO MapToDTO(Guest guest) => new()
    {
        GuestId = guest.GuestId,
        FullName = guest.FullName,
        Email = guest.Email,
        PhoneNumber = guest.PhoneNumber,
        Address = guest.Address,
        CreatedAt = guest.CreatedAt
    };

    /// <summary>
    /// Utility function that generates a SHA256 base64 hash of a password string.
    /// </summary>
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
