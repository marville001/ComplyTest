using ComplyTest.Domain.Entities;

namespace ComplyTest.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByEmailAsync(string email);
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId);
    Task<IEnumerable<Employee>> GetEmployeesByProjectAsync(int projectId);
}