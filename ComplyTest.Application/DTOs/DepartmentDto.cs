namespace ComplyTest.Application.DTOs;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string OfficeLocation { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}