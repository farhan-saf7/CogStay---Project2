using System;
using System.ComponentModel.DataAnnotations;
using CogStayMVC.Enums;

namespace CogStayMVC.DTOs;

/// <summary>
/// Data Transfer Object representing internal employee response details.
/// </summary>
public class StaffResponseDTO
{
    /// <summary>
    /// Gets or sets the unique Staff member ID.
    /// </summary>
    public int StaffId { get; set; }

    /// <summary>
    /// Gets or sets the staff member's full name.
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the staff email identifier.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the contact phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the staff role (e.g. Admin, FrontDesk, Housekeeping, Manager).
    /// </summary>
    public StaffRole Role { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this account is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the account creation date and time.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Parameters needed to register a new staff account.
/// </summary>
public class CreateStaffDTO
{
    /// <summary>
    /// Gets or sets the employee full name.
    /// </summary>
    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique email address.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the contact phone number.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required.")]
    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password credential string to be hashed.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Gets or sets the role assignment.
    /// </summary>
    [Required(ErrorMessage = "Staff Role is required.")]
    public StaffRole Role { get; set; }
}

/// <summary>
/// Parameters needed to modify an existing staff account.
/// </summary>
public class UpdateStaffDTO
{
    /// <summary>
    /// Gets or sets the Staff ID to update.
    /// </summary>
    public int StaffId { get; set; }

    /// <summary>
    /// Gets or sets the updated employee full name.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the updated email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the updated phone number contact.
    /// </summary>
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the updated role assignment.
    /// </summary>
    [Required]
    public StaffRole Role { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this account is active.
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// Credentials submitted by staff members to log in.
/// </summary>
public class StaffLoginDTO
{
    /// <summary>
    /// Gets or sets the email credential.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password credential.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Gets or sets the portal role selection.
    /// </summary>
    [Required(ErrorMessage = "Role is required.")]
    public StaffRole Role { get; set; }
}
