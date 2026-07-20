using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        return await _roomRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await _roomRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
    {
        return await _roomRepository.GetAvailableRoomsAsync();
    }

    public async Task<Room> CreateRoomAsync(Room room)
    {
        var existing = await _roomRepository.GetByRoomNumberAsync(room.RoomNumber);
        if (existing != null)
        {
            throw new InvalidOperationException($"Room number '{room.RoomNumber}' is already configured.");
        }

        await _roomRepository.AddAsync(room);
        return room;
    }

    public async Task UpdateRoomAsync(Room room)
    {
        var dbRoom = await _roomRepository.GetByIdAsync(room.RoomId);
        if (dbRoom == null)
        {
            throw new KeyNotFoundException("Room not found.");
        }

        if (dbRoom.RoomNumber != room.RoomNumber)
        {
            var existing = await _roomRepository.GetByRoomNumberAsync(room.RoomNumber);
            if (existing != null)
            {
                throw new InvalidOperationException($"Room number '{room.RoomNumber}' is already in use.");
            }
            dbRoom.RoomNumber = room.RoomNumber;
        }

        dbRoom.RoomType = room.RoomType;
        dbRoom.PricePerNight = room.PricePerNight;
        dbRoom.Status = room.Status;

        await _roomRepository.UpdateAsync(dbRoom);
    }

    public async Task<bool> DeleteRoomAsync(int id)
    {
        if (await _roomRepository.HasReservationsAsync(id))
        {
            throw new InvalidOperationException("Cannot delete room because it has reservation records linked to it.");
        }
        if (await _roomRepository.HasHousekeepingTasksAsync(id))
        {
            throw new InvalidOperationException("Cannot delete room because it has operational tasks assigned to it.");
        }

        await _roomRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<Room>> SearchRoomsAsync(string? query)
    {
        var rooms = await _roomRepository.GetAllAsync();
        if (string.IsNullOrWhiteSpace(query))
        {
            return rooms;
        }

        query = query.Trim().ToLower();
        return rooms.Where(r => 
            r.RoomNumber.ToLower().Contains(query) || 
            r.RoomType.ToLower().Contains(query) || 
            r.Status.ToString().ToLower().Contains(query)
        ).ToList();
    }
}
