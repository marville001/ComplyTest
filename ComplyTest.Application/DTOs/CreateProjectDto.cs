using System.ComponentModel.DataAnnotations;

namespace ComplyTest.Application.DTOs;

public class CreateProjectDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive number")]
    public decimal Budget { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "DepartmentId must be a positive number")]
    public int DepartmentId { get; set; }
}