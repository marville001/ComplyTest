using ComplyTest.Application.DTOs;
using ComplyTest.Application.Features.Department.Interfaces;
using ComplyTest.Application.Mappers.Department;
using ComplyTest.Domain.Interfaces;

namespace ComplyTest.Application.Features.Department.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDepartmentMapper _departmentMapper;

    public DepartmentService(IDepartmentRepository departmentRepository, IDepartmentMapper departmentMapper)
    {
        _departmentRepository = departmentRepository;
        _departmentMapper = departmentMapper;
    }

    public async Task<DepartmentDto?> GetDepartmentAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null) return null;
        return _departmentMapper.ToDto(department);
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(_departmentMapper.ToDto);
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
    {
        var department = new Domain.Entities.Department
        {
            Name = createDepartmentDto.Name,
            OfficeLocation = createDepartmentDto.OfficeLocation
        };
        
        var createdDepartment = await _departmentRepository.AddAsync(department);
        return _departmentMapper.ToDto(createdDepartment);
    }

    public async Task<DepartmentDto?> UpdateDepartmentAsync(int id, UpdateDepartmentDto updateDepartmentDto)
    {
        var department = new ComplyTest.Domain.Entities.Department
        {
            Name = updateDepartmentDto.Name,
            OfficeLocation = updateDepartmentDto.OfficeLocation
        };
        
        department.Id = id;
        var updatedDepartment = await _departmentRepository.UpdateAsync(department);
        if (updatedDepartment == null) return null;
        return _departmentMapper.ToDto(updatedDepartment);
    }

    public async Task<DepartmentDto?> DeleteDepartmentAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null) return null;
        
        var deleted = await _departmentRepository.DeleteAsync(id);
        if (!deleted) return null;
        
        return _departmentMapper.ToDto(department);
    }

    public async Task<decimal> GetTotalProjectBudgetAsync(int departmentId)
    {
        return await _departmentRepository.GetTotalProjectBudgetAsync(departmentId);
    }
}