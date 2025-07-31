using System.ComponentModel.DataAnnotations;

namespace ComplyTest.Domain.Entities;

public class Department
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public required string Name { get; set; }
    
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public required string OfficeLocation { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}