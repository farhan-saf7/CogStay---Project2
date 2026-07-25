namespace CogStayMVC.Enums;

/// <summary>
/// Specifies the operational state of a hotel room.
/// </summary>
public enum RoomStatus
{
    /// <summary>
    /// Room is clean, vacant, and ready for a new booking.
    /// </summary>
    Available,

    /// <summary>
    /// Room is booked for an upcoming confirmed reservation.
    /// </summary>
    Booked,

    /// <summary>
    /// Guest has checked in; room is currently occupied.
    /// </summary>
    Occupied,

    /// <summary>
    /// Front desk has requested a checkout but payment is not yet processed.
    /// </summary>
    CheckoutPending,

    /// <summary>
    /// Guest has paid and checked out; room is dirty and needs cleaning.
    /// </summary>
    CleaningRequired,

    /// <summary>
    /// Housekeeping staff is currently cleaning the room.
    /// </summary>
    CleaningInProgress,

    /// <summary>
    /// Room is unavailable due to repairs, maintenance, or plumbing/electrical works.
    /// </summary>
    UnderMaintenance
}

