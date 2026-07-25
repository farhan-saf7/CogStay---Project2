namespace CogStayMVC.Enums;

/// <summary>
/// Specifies the role and authorization scope of an internal hotel employee.
/// </summary>
public enum StaffRole
{
    /// <summary>
    /// Processes guest check-ins, check-outs, room reservation requests, and coordinates housekeeping.
    /// </summary>
    FrontDesk,

    /// <summary>
    /// Manages room cleaning tasks and updates status variables (Dirty/Available).
    /// </summary>
    Housekeeping,

    /// <summary>
    /// Monitors reservations, guest feedback summaries, room occupancy, and revenue sheets.
    /// </summary>
    Manager,

    /// <summary>
    /// Grants access to staff account registers, configurations, reporting tools, and database records.
    /// </summary>
    Admin
}
