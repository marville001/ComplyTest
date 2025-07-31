using System.ComponentModel.DataAnnotations;

namespace ComplyTest.Application.DTOs;

public class UpdateDepartmentDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string OfficeLocation { get; set; } = string.Empty;
}