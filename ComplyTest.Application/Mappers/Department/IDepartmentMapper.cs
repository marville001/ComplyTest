using ComplyTest.Application.DTOs;

namespace ComplyTest.Application.Mappers.Department;

public interface IDepartmentMapper
{
    DepartmentDto ToDto(ComplyTest.Domain.Entities.Department department);
    ComplyTest.Domain.Entities.Department ToDomain(DepartmentDto departmentDto);
}