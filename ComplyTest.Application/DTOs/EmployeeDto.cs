namespace ComplyTest.Application.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime CreatedDate { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}