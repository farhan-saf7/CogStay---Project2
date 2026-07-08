using System;

namespace CogStayMVC.Models;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public string CreatedBy { get; set; } = "System";
    public string? UpdatedBy { get; set; }
    
    public bool IsDeleted { get; set; } = false;
}
