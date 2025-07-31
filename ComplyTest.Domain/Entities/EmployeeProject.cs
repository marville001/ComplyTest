using System.ComponentModel.DataAnnotations;

namespace ComplyTest.Domain.Entities;

public class EmployeeProject
{
    [Required]
    public int EmployeeId { get; set; }
    
    [Required]
    public int ProjectId { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public required string Role { get; set; }
    
    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
    public virtual Project Project { get; set; } = null!;
}