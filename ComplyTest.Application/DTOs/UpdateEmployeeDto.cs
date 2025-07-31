using System.ComponentModel.DataAnnotations;

namespace ComplyTest.Application.DTOs;

public class UpdateEmployeeDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
    public decimal Salary { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "DepartmentId must be a positive number")]
    public int DepartmentId { get; set; }
}