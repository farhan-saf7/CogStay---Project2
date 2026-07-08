using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CogStayMVC.Models;

public class Role : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty; // Admin, Manager, Receptionist, Housekeeping, Customer

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
