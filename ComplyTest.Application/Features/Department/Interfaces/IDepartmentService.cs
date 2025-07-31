using ComplyTest.Application.DTOs;

namespace ComplyTest.Application.Features.Department.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentDto?> GetDepartmentAsync(int id);
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto);
    Task<DepartmentDto?> UpdateDepartmentAsync(int id, UpdateDepartmentDto updateDepartmentDto);
    Task<DepartmentDto?> DeleteDepartmentAsync(int id);
    Task<decimal> GetTotalProjectBudgetAsync(int departmentId);
}