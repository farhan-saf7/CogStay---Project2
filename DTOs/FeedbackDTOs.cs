using System;
using System.ComponentModel.DataAnnotations;

namespace CogStayMVC.DTOs;

/// <summary>
/// Data Transfer Object representing submitted guest feedback response details.
/// </summary>
public class FeedbackResponseDTO
{
    /// <summary>
    /// Gets or sets the unique Feedback ID.
    /// </summary>
    public int FeedbackId { get; set; }

    /// <summary>
    /// Gets or sets the linked Guest ID.
    /// </summary>
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the guest full name.
    /// </summary>
    public string GuestName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the optional associated Reservation ID.
    /// </summary>
    public int? ReservationId { get; set; }

    /// <summary>
    /// Gets or sets the rating score (1-5 stars).
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Gets or sets the detailed comments or reviews.
    /// </summary>
    public string Comments { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the feedback was submitted.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Parameters needed to submit guest feedback reviews.
/// </summary>
public class CreateFeedbackDTO
{
    /// <summary>
    /// Gets or sets the guest account ID.
    /// </summary>
    [Required(ErrorMessage = "Guest ID is required.")]
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the optional reservation ID to link the review to a stay.
    /// </summary>
    public int? ReservationId { get; set; }

    /// <summary>
    /// Gets or sets the star rating (1 to 5).
    /// </summary>
    [Required(ErrorMessage = "Rating is required.")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
    public int Rating { get; set; }

    /// <summary>
    /// Gets or sets the textual comment/review.
    /// </summary>
    [Required(ErrorMessage = "Comments are required.")]
    [StringLength(1000)]
    public string Comments { get; set; } = null!;
}
