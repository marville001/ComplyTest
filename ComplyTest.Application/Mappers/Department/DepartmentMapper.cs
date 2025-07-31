using ComplyTest.Application.DTOs;
using AutoMapper;

namespace ComplyTest.Application.Mappers.Department;

public class DepartmentMapper : IDepartmentMapper
{
    private readonly IMapper _mapper;

    public DepartmentMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public DepartmentDto ToDto(ComplyTest.Domain.Entities.Department department)
        => _mapper.Map<DepartmentDto>(department);

    public ComplyTest.Domain.Entities.Department ToDomain(DepartmentDto departmentDto)
        => _mapper.Map<ComplyTest.Domain.Entities.Department>(departmentDto);
}