using CogStayMVC.Enums;

namespace CogStayMVC.Models;

/// <summary>
/// Represents a housekeeping maintenance or cleaning task assigned to a room.
/// Housekeepers update these tasks to communicate room readiness back to the front desk.
/// </summary>
public class HousekeepingTask
{
    /// <summary>
    /// Gets or sets the unique identifier for the housekeeping task.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the room that requires housekeeping services.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the description of the tasks needed (e.g., "Full turnover clean", "laundry request", etc.).
    /// </summary>
    public string TaskDescription { get; set; } = null!;

    /// <summary>
    /// Gets or sets the current status of the task (e.g., Pending, InProgress, Completed).
    /// </summary>
    public CogStayMVC.Enums.TaskStatus TaskStatus { get; set; }

    // Navigation Properties

    /// <summary>
    /// Gets or sets the Room object associated with this housekeeping task.
    /// Represents a Many-to-One relationship (multiple tasks can be scheduled for a single room over time).
    /// </summary>
    public virtual Room Room { get; set; } = null!;
}
