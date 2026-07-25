namespace CogStayMVC.Enums;

/// <summary>
/// Specifies the execution status of a housekeeping task.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Task has been assigned but not started.
    /// </summary>
    Pending,

    /// <summary>
    /// Housekeeping staff is currently performing the task.
    /// </summary>
    InProgress,

    /// <summary>
    /// Task is completed and room is now clean and available.
    /// </summary>
    Completed
}
