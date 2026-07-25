using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogStayMVC.DTOs;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;
using TaskStatus = CogStayMVC.Enums.TaskStatus;

namespace CogStayMVC.Services.Housekeeping;

/// <summary>
/// Service implementation managing housekeeping operations. Creates tasks, tracks their lifecycle,
/// and updates Room status values based on cleaning progress (e.g. transitioning from InProgress to Completed).
/// </summary>
public class HousekeepingService : IHousekeepingService
{
    private readonly IHousekeepingTaskRepository _taskRepository;
    private readonly IRoomRepository _roomRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="HousekeepingService"/> class.
    /// </summary>
    /// <param name="taskRepository">Repository handling HousekeepingTask models.</param>
    /// <param name="roomRepository">Repository handling Room database models.</param>
    public HousekeepingService(
        IHousekeepingTaskRepository taskRepository,
        IRoomRepository roomRepository)
    {
        _taskRepository = taskRepository;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Retrieves all scheduled cleaning tasks.
    /// </summary>
    /// <returns>Collection of housekeeping task response DTOs.</returns>
    public async Task<IEnumerable<HousekeepingTaskResponseDTO>> GetAllTasksAsync()
    {
        var tasks = await _taskRepository.GetTasksWithDetailsAsync();
        return tasks.Select(MapToDTO);
    }

    /// <summary>
    /// Retrieves details for a specific cleaning task.
    /// </summary>
    /// <param name="id">Task identifier.</param>
    /// <returns>The task response DTO, or null if not found.</returns>
    public async Task<HousekeepingTaskResponseDTO?> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetTaskWithDetailsAsync(id);
        return task != null ? MapToDTO(task) : null;
    }

    /// <summary>
    /// Retrieves cleaning tasks created for a specific room.
    /// </summary>
    /// <param name="roomId">Room identifier.</param>
    /// <returns>Collection of task responses.</returns>
    public async Task<IEnumerable<HousekeepingTaskResponseDTO>> GetTasksByRoomIdAsync(int roomId)
    {
        var tasks = await _taskRepository.GetTasksByRoomIdAsync(roomId);
        return tasks.Select(MapToDTO);
    }

    /// <summary>
    /// Schedules a new housekeeping or cleaning task for a room.
    /// </summary>
    /// <param name="dto">The task parameters DTO.</param>
    /// <returns>Created task response details.</returns>
    public async Task<HousekeepingTaskResponseDTO> CreateTaskAsync(CreateHousekeepingTaskDTO dto)
    {
        var room = await _roomRepository.GetByIdAsync(dto.RoomId);
        if (room == null)
            throw new InvalidOperationException("Room not found.");

        var task = new HousekeepingTask
        {
            RoomId = dto.RoomId,
            TaskDescription = dto.TaskDescription,
            TaskStatus = TaskStatus.Pending
        };

        await _taskRepository.AddAsync(task);
        var created = await _taskRepository.GetTaskWithDetailsAsync(task.TaskId);
        return MapToDTO(created ?? task);
    }

    /// <summary>
    /// Updates the status of a housekeeping task and syncs Room statuses dynamically.
    /// </summary>
    /// <param name="dto">The update status parameters DTO.</param>
    public async Task UpdateTaskStatusAsync(UpdateTaskStatusDTO dto)
    {
        var task = await _taskRepository.GetTaskWithDetailsAsync(dto.TaskId);
        if (task == null)
            throw new KeyNotFoundException("Housekeeping task not found.");

        task.TaskStatus = dto.TaskStatus;
        await _taskRepository.UpdateAsync(task);

        // Room status state machine update
        if (task.Room != null)
        {
            if (dto.TaskStatus == TaskStatus.InProgress)
            {
                task.Room.Status = RoomStatus.CleaningInProgress;
                await _roomRepository.UpdateAsync(task.Room);
            }
            else if (dto.TaskStatus == TaskStatus.Completed)
            {
                task.Room.Status = RoomStatus.Available; // Visible again for public booking!
                await _roomRepository.UpdateAsync(task.Room);
            }
        }
    }

    /// <summary>
    /// Deletes a housekeeping task.
    /// </summary>
    /// <param name="id">Task identifier.</param>
    public async Task DeleteTaskAsync(int id)
    {
        await _taskRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Maps a HousekeepingTask model to a HousekeepingTaskResponseDTO.
    /// </summary>
    private static HousekeepingTaskResponseDTO MapToDTO(HousekeepingTask task) => new()
    {
        TaskId = task.TaskId,
        RoomId = task.RoomId,
        RoomNumber = task.Room?.RoomNumber ?? "N/A",
        TaskDescription = task.TaskDescription,
        TaskStatus = task.TaskStatus
    };
}
