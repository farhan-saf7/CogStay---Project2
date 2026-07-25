using System;
using CogStayMVC.Enums;

namespace CogStayMVC.Models;

/// <summary>
/// Represents an internal hotel staff member. Staff are classified into roles (Admin,
/// Manager, FrontDesk, Housekeeping) and are granted specific operational access.
/// </summary>
public class Staff
{
    /// <summary>
    /// Gets or sets the unique identifier for the staff member.
    /// </summary>
    public int StaffId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the staff member.
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique email address of the staff member used for login.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the contact phone number of the staff member.
    /// </summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the hashed password for staff portal authentication.
    /// </summary>
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// Gets or sets the security role assigned to this staff member (e.g., Admin, Manager, FrontDesk, Housekeeping).
    /// </summary>
    public StaffRole Role { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the staff member's account is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the staff account was registered.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
