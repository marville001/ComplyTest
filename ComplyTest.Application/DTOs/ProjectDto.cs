namespace ComplyTest.Application.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}