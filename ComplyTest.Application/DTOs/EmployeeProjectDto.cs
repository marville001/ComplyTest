namespace ComplyTest.Application.DTOs;

public class EmployeeProjectDto
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; }
}