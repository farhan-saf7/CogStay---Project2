using System.ComponentModel.DataAnnotations;
using TaskStatus = CogStayMVC.Enums.TaskStatus;

namespace CogStayMVC.DTOs;

/// <summary>
/// Data Transfer Object representing a housekeeping cleaning task.
/// </summary>
public class HousekeepingTaskResponseDTO
{
    /// <summary>
    /// Gets or sets the unique cleaning Task ID.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the associated Room ID.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the room number.
    /// </summary>
    public string RoomNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the detailed task description (e.g. standard clean, change linens).
    /// </summary>
    public string TaskDescription { get; set; } = null!;

    /// <summary>
    /// Gets or sets the task execution status (Pending/InProgress/Completed).
    /// </summary>
    public TaskStatus TaskStatus { get; set; }
}

/// <summary>
/// Parameters needed to assign a new housekeeping task.
/// </summary>
public class CreateHousekeepingTaskDTO
{
    /// <summary>
    /// Gets or sets the target Room ID.
    /// </summary>
    [Required(ErrorMessage = "Room selection is required.")]
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the description of cleaning actions required.
    /// </summary>
    [Required(ErrorMessage = "Task description is required.")]
    [StringLength(1000)]
    public string TaskDescription { get; set; } = null!;
}

/// <summary>
/// Parameters needed to update a task's progress state.
/// </summary>
public class UpdateTaskStatusDTO
{
    /// <summary>
    /// Gets or sets the cleaning Task ID to update.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the new status of the task.
    /// </summary>
    [Required]
    public TaskStatus TaskStatus { get; set; }
}
