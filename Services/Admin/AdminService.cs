using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CogStayMVC.DTOs;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Services.Admin;

/// <summary>
/// Service implementation managing room inventory configurations and queries.
/// </summary>
public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomService"/> class.
    /// </summary>
    /// <param name="roomRepository">Repository handling Room model operations.</param>
    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Retrieves all hotel rooms.
    /// </summary>
    /// <returns>Collection of room response DTOs.</returns>
    public async Task<IEnumerable<RoomResponseDTO>> GetAllRoomsAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();
        return rooms.Select(MapToDTO);
    }

    /// <summary>
    /// Retrieves all available hotel rooms.
    /// </summary>
    /// <returns>Collection of room response DTOs with Available status.</returns>
    public async Task<IEnumerable<RoomResponseDTO>> GetAvailableRoomsAsync()
    {
        var rooms = await _roomRepository.GetRoomsByStatusAsync(RoomStatus.Available);
        return rooms.Select(MapToDTO);
    }

    /// <summary>
    /// Retrieves room details by ID.
    /// </summary>
    /// <param name="id">Room identifier.</param>
    /// <returns>The room response DTO, or null.</returns>
    public async Task<RoomResponseDTO?> GetRoomByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        return room != null ? MapToDTO(room) : null;
    }

    /// <summary>
    /// Creates a new room in the system, validating for room number uniqueness.
    /// </summary>
    /// <param name="dto">Create room parameters DTO.</param>
    /// <returns>Newly created room response details.</returns>
    public async Task<RoomResponseDTO> CreateRoomAsync(CreateRoomDTO dto)
    {
        var existing = await _roomRepository.GetByRoomNumberAsync(dto.RoomNumber);
        if (existing != null)
        {
            throw new InvalidOperationException($"Room number '{dto.RoomNumber}' already exists.");
        }

        var room = new Room
        {
            RoomNumber = dto.RoomNumber,
            RoomType = dto.RoomType,
            PricePerNight = dto.PricePerNight,
            Status = dto.Status
        };

        await _roomRepository.AddAsync(room);
        return MapToDTO(room);
    }

    /// <summary>
    /// Updates room details (pricing, number, status) in the database.
    /// </summary>
    /// <param name="dto">Updated room parameters DTO.</param>
    public async Task UpdateRoomAsync(UpdateRoomDTO dto)
    {
        var room = await _roomRepository.GetByIdAsync(dto.RoomId);
        if (room == null)
            throw new KeyNotFoundException("Room not found.");

        room.RoomNumber = dto.RoomNumber;
        room.RoomType = dto.RoomType;
        room.PricePerNight = dto.PricePerNight;
        room.Status = dto.Status;

        await _roomRepository.UpdateAsync(room);
    }

    /// <summary>
    /// Changes the operational status of a room.
    /// </summary>
    /// <param name="roomId">Room identifier.</param>
    /// <param name="status">New RoomStatus value.</param>
    public async Task UpdateRoomStatusAsync(int roomId, RoomStatus status)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null)
            throw new KeyNotFoundException("Room not found.");

        room.Status = status;
        await _roomRepository.UpdateAsync(room);
    }

    /// <summary>
    /// Deletes a specific room from the database.
    /// </summary>
    /// <param name="id">Room identifier.</param>
    public async Task DeleteRoomAsync(int id)
    {
        await _roomRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Maps a Room model to RoomResponseDTO.
    /// </summary>
    private static RoomResponseDTO MapToDTO(Room room) => new()
    {
        RoomId = room.RoomId,
        RoomNumber = room.RoomNumber,
        RoomType = room.RoomType,
        PricePerNight = room.PricePerNight,
        Status = room.Status
    };
}

/// <summary>
/// Service implementation managing internal employee registrations, accounts, and auth lookups.
/// </summary>
public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffService"/> class.
    /// </summary>
    /// <param name="staffRepository">Repository handling Staff database models.</param>
    public StaffService(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    /// <summary>
    /// Retrieves all staff accounts.
    /// </summary>
    /// <returns>Collection of staff response DTOs.</returns>
    public async Task<IEnumerable<StaffResponseDTO>> GetAllStaffAsync()
    {
        var staffList = await _staffRepository.GetAllAsync();
        return staffList.Select(MapToDTO);
    }

    /// <summary>
    /// Retrieves a specific staff member by their ID.
    /// </summary>
    /// <param name="id">Staff identifier.</param>
    /// <returns>Staff response details, or null if not found.</returns>
    public async Task<StaffResponseDTO?> GetStaffByIdAsync(int id)
    {
        var staff = await _staffRepository.GetByIdAsync(id);
        return staff != null ? MapToDTO(staff) : null;
    }

    /// <summary>
    /// Creates a new employee staff account. Hashes credentials before db write.
    /// </summary>
    /// <param name="dto">Create staff DTO.</param>
    /// <returns>Created staff details response.</returns>
    public async Task<StaffResponseDTO> CreateStaffAsync(CreateStaffDTO dto)
    {
        var existing = await _staffRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new InvalidOperationException("Staff member with this email already exists.");

        var staff = new Staff
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = HashPassword(dto.Password),
            Role = dto.Role,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        await _staffRepository.AddAsync(staff);
        return MapToDTO(staff);
    }

    /// <summary>
    /// Updates employee account information.
    /// </summary>
    /// <param name="dto">Updated staff parameters DTO.</param>
    public async Task UpdateStaffAsync(UpdateStaffDTO dto)
    {
        var staff = await _staffRepository.GetByIdAsync(dto.StaffId);
        if (staff == null)
            throw new KeyNotFoundException("Staff member not found.");

        staff.FullName = dto.FullName;
        staff.Email = dto.Email;
        staff.PhoneNumber = dto.PhoneNumber;
        staff.Role = dto.Role;
        staff.IsActive = dto.IsActive;

        await _staffRepository.UpdateAsync(staff);
    }

    /// <summary>
    /// Deletes a specific staff account.
    /// </summary>
    /// <param name="id">Staff identifier.</param>
    public async Task DeleteStaffAsync(int id)
    {
        await _staffRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Validates employee login credentials and checks active role constraints.
    /// </summary>
    /// <param name="dto">Staff login inputs.</param>
    /// <returns>Staff response details on successful validation, else null.</returns>
    public async Task<StaffResponseDTO?> ValidateStaffLoginAsync(StaffLoginDTO dto)
    {
        var staff = await _staffRepository.GetByEmailAsync(dto.Email);
        if (staff == null || !staff.IsActive || staff.PasswordHash != HashPassword(dto.Password))
        {
            return null;
        }

        if (staff.Role != dto.Role && dto.Role != StaffRole.Admin)
        {
            return null; // Role mismatch (Admins are bypassed to access all dashboards)
        }

        return MapToDTO(staff);
    }

    /// <summary>
    /// Maps Staff model to StaffResponseDTO.
    /// </summary>
    private static StaffResponseDTO MapToDTO(Staff staff) => new()
    {
        StaffId = staff.StaffId,
        FullName = staff.FullName,
        Email = staff.Email,
        PhoneNumber = staff.PhoneNumber,
        Role = staff.Role,
        IsActive = staff.IsActive,
        CreatedAt = staff.CreatedAt
    };

    /// <summary>
    /// Helper to compute SHA256 base64 hashed password values.
    /// </summary>
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
