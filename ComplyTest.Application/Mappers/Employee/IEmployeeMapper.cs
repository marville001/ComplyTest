using ComplyTest.Application.DTOs;

namespace ComplyTest.Application.Mappers.Employee;

public interface IEmployeeMapper
{
    EmployeeDto ToDto(ComplyTest.Domain.Entities.Employee employee);
    ComplyTest.Domain.Entities.Employee ToDomain(EmployeeDto employeeDto);
}