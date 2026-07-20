using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Services;

public class HousekeepingService : IHousekeepingService
{
    private readonly IHousekeepingRepository _housekeepingRepository;

    public HousekeepingService(IHousekeepingRepository housekeepingRepository)
    {
        _housekeepingRepository = housekeepingRepository;
    }

    public async Task<HousekeepingTask?> GetByIdAsync(int id)
    {
        return await _housekeepingRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<HousekeepingTask>> GetAllTasksAsync()
    {
        return await _housekeepingRepository.GetAllAsync();
    }

    public async Task AssignTaskAsync(HousekeepingTask task)
    {
        task.TaskStatus = CogStayMVC.Enums.TaskStatus.Pending;
        await _housekeepingRepository.AddAsync(task);
    }

    public async Task UpdateTaskStatusAsync(int taskId, CogStayMVC.Enums.TaskStatus status)
    {
        var task = await _housekeepingRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            throw new KeyNotFoundException("Housekeeping task not found.");
        }

        task.TaskStatus = status;
        await _housekeepingRepository.UpdateAsync(task);
    }
}
