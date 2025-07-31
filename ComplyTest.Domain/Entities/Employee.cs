using System.ComponentModel.DataAnnotations;

namespace ComplyTest.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public required string FirstName { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public required string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public int DepartmentId { get; set; }
    public virtual Department Department { get; set; } = null!;
    public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
}