using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Services.Interfaces;

public interface IHousekeepingService
{
    Task<HousekeepingTask?> GetByIdAsync(int id);
    Task<IEnumerable<HousekeepingTask>> GetAllTasksAsync();
    Task AssignTaskAsync(HousekeepingTask task);
    Task UpdateTaskStatusAsync(int taskId, CogStayMVC.Enums.TaskStatus status);
}
