using System;
using System.ComponentModel.DataAnnotations;

namespace CogStayMVC.DTOs;

/// <summary>
/// Data Transfer Object representing Guest response details.
/// </summary>
public class GuestResponseDTO
{
    /// <summary>
    /// Gets or sets the unique Guest ID.
    /// </summary>
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the guest full name.
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the guest unique email.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the guest phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the guest physical address.
    /// </summary>
    public string Address { get; set; } = null!;

    /// <summary>
    /// Gets or sets the registration date and time.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Parameters needed to register a new Guest.
/// </summary>
public class CreateGuestDTO
{
    /// <summary>
    /// Gets or sets the guest full name.
    /// </summary>
    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique email address.
    /// </summary>
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the phone number contact.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the physical address location.
    /// </summary>
    [Required(ErrorMessage = "Address is required.")]
    [StringLength(500)]
    public string Address { get; set; } = null!;

    /// <summary>
    /// Gets or sets the plain-text password to hash.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = null!;
}

/// <summary>
/// Parameters needed to update guest profile information.
/// </summary>
public class UpdateGuestDTO
{
    /// <summary>
    /// Gets or sets the unique Guest identifier to modify.
    /// </summary>
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the updated full name.
    /// </summary>
    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the updated email address.
    /// </summary>
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the updated phone number contact.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required.")]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the updated physical address.
    /// </summary>
    [Required(ErrorMessage = "Address is required.")]
    [StringLength(500)]
    public string Address { get; set; } = null!;
}

/// <summary>
/// Parameters submitted by a guest to sign in.
/// </summary>
public class GuestLoginDTO
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
}
