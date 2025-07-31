using ComplyTest.Application.DTOs;
using AutoMapper;

namespace ComplyTest.Application.Mappers.Employee;

public class EmployeeMapper : IEmployeeMapper
{
    private readonly IMapper _mapper;

    public EmployeeMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public EmployeeDto ToDto(ComplyTest.Domain.Entities.Employee employee)
        => _mapper.Map<EmployeeDto>(employee);

    public ComplyTest.Domain.Entities.Employee ToDomain(EmployeeDto employeeDto)
        => _mapper.Map<ComplyTest.Domain.Entities.Employee>(employeeDto);
}