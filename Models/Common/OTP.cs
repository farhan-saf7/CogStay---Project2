using System;
using System.ComponentModel.DataAnnotations;

namespace CogStayMVC.Models.Common;

public class OTP
{
    [Key]
    public int OtpId { get; set; }

    [Required]
    [MaxLength(100)]
    public string EmailOrPhone { get; set; } = string.Empty;

    [Required]
    [MaxLength(6)]
    public string OtpCode { get; set; } = string.Empty;

    [Required]
    public DateTime ExpiryTime { get; set; }

    public bool IsUsed { get; set; } = false;
}
