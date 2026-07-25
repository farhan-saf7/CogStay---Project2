using System.ComponentModel.DataAnnotations;
using CogStayMVC.Enums;

namespace CogStayMVC.DTOs;

/// <summary>
/// Data Transfer Object representing an invoice response.
/// </summary>
public class BillingResponseDTO
{
    /// <summary>
    /// Gets or sets the unique Bill ID.
    /// </summary>
    public int BillId { get; set; }

    /// <summary>
    /// Gets or sets the linked Stay Record ID.
    /// </summary>
    public int StayId { get; set; }

    /// <summary>
    /// Gets or sets the guest full name on the invoice.
    /// </summary>
    public string GuestName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the room number associated with the stay.
    /// </summary>
    public string RoomNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the total charge amount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the payment status of the bill (Pending/Paid).
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Gets or sets the custom invoice remarks or description details.
    /// </summary>
    public string? Remarks { get; set; }
}

/// <summary>
/// Parameters needed to manually create a bill record.
/// </summary>
public class CreateBillDTO
{
    /// <summary>
    /// Gets or sets the stay record identifier.
    /// </summary>
    [Required(ErrorMessage = "Stay ID is required.")]
    public int StayId { get; set; }

    /// <summary>
    /// Gets or sets the total invoice cost amount.
    /// </summary>
    [Required]
    [Range(0.00, 1000000.00)]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets additional invoice notes or remarks.
    /// </summary>
    public string? Remarks { get; set; }
}

/// <summary>
/// Parameters needed to process a payment and settle an outstanding bill.
/// </summary>
public class ProcessPaymentDTO
{
    /// <summary>
    /// Gets or sets the bill record identifier to pay.
    /// </summary>
    [Required(ErrorMessage = "Bill ID is required.")]
    public int BillId { get; set; }

    /// <summary>
    /// Gets or sets the payment reference details (e.g. Card, Cash).
    /// </summary>
    [Required(ErrorMessage = "Payment method/remarks is required.")]
    public string Remarks { get; set; } = "Paid via Cash / Card at Front Desk";
}
