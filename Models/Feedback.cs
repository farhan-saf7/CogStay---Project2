using System;

namespace CogStayMVC.Models;

/// <summary>
/// Represents a guest feedback entry. Guests can submit a rating and
/// comments concerning their hotel stay and room experience.
/// </summary>
public class Feedback
{
    /// <summary>
    /// Gets or sets the unique identifier for the feedback record.
    /// </summary>
    public int FeedbackId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the guest who submitted this feedback.
    /// </summary>
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the specific reservation this feedback pertains to, if any.
    /// </summary>
    public int? ReservationId { get; set; }

    /// <summary>
    /// Gets or sets the rating score (typically 1 to 5 stars).
    /// </summary>
    public int Rating { get; set; } // 1 to 5

    /// <summary>
    /// Gets or sets the guest comments, reviews, or complaints.
    /// </summary>
    public string Comments { get; set; } = null!;

    /// <summary>
    /// Gets or sets the timestamp when the feedback was created. Defaults to the current local time.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation Properties

    /// <summary>
    /// Gets or sets the guest who authored this feedback.
    /// </summary>
    public virtual Guest Guest { get; set; } = null!;

    /// <summary>
    /// Gets or sets the optional reservation link associated with this feedback.
    /// </summary>
    public virtual Reservation? Reservation { get; set; }
}
