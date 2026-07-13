using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogStayMVC.Models.Admin;

public class Role
{
    [Key]
    public int RoleId { get; set; }

    [Required]
    [MaxLength(20)]
    public string RoleName { get; set; } = string.Empty;

    // Navigation property
    public ICollection<User> Users { get; set; } = new List<User>();
}
