using ComplyTest.Application.DTOs;

namespace ComplyTest.Application.Features.Employee.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto?> GetEmployeeAsync(int id);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
    Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto);
    Task<EmployeeDto?> DeleteEmployeeAsync(int id);
    Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(int departmentId);
    Task<IEnumerable<EmployeeDto>> GetEmployeesByProjectAsync(int projectId);
}