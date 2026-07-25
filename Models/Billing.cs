using CogStayMVC.Enums;

namespace CogStayMVC.Models;

/// <summary>
/// Represents a billing record associated with a guest's stay in the hotel.
/// Stores invoice amounts, payment statuses, and links directly to the stay record.
/// </summary>
public class Billing
{
    /// <summary>
    /// Gets or sets the unique identifier for the billing record.
    /// </summary>
    public int BillId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the stay record associated with this bill.
    /// </summary>
    public int StayId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the guest responsible for payment.
    /// </summary>
    public int GuestId { get; set; }

    /// <summary>
    /// Gets or sets the guest's full name at the time of billing.
    /// </summary>
    public string GuestName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total accumulated amount for room and services.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the current status of the payment (e.g., Pending, Paid).
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Gets or sets optional administrative remarks or invoice details.
    /// </summary>
    public string? Remarks { get; set; }

    // Navigation Properties

    /// <summary>
    /// Gets or sets the stay record that generated this bill.
    /// This establishes a One-to-One relationship with the StayRecord.
    /// </summary>
    public virtual StayRecord StayRecord { get; set; } = null!;
}
