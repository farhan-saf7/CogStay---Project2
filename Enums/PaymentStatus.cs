namespace CogStayMVC.Enums;

/// <summary>
/// Specifies the payment state of a billing record.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Bill has been generated but not paid yet.
    /// </summary>
    Pending,

    /// <summary>
    /// Bill has been paid and settled.
    /// </summary>
    Paid
}
