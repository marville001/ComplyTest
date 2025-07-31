using ComplyTest.Application.DTOs;
using ComplyTest.Application.Features.Employee.Interfaces;
using ComplyTest.Application.Mappers.Employee;
using ComplyTest.Domain.Interfaces;

namespace ComplyTest.Application.Features.Employee.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeMapper _employeeMapper;
    private readonly IDepartmentRepository _departmentRepository;

    public EmployeeService(
        IEmployeeRepository employeeRepository, 
        IEmployeeMapper employeeMapper,
        IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _employeeMapper = employeeMapper;
        _departmentRepository = departmentRepository;
    }

    public async Task<EmployeeDto?> GetEmployeeAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Employee ID must be a positive number", nameof(id));
            
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null) return null;
        return _employeeMapper.ToDto(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(_employeeMapper.ToDto);
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
    {
        if (createEmployeeDto == null)
            throw new ArgumentNullException(nameof(createEmployeeDto));

        // Validate department exists
        var department = await _departmentRepository.GetByIdAsync(createEmployeeDto.DepartmentId);
        if (department == null)
            throw new ArgumentException($"Department with ID {createEmployeeDto.DepartmentId} does not exist");

        // Check if email already exists
        var existingEmployee = await _employeeRepository.GetByEmailAsync(createEmployeeDto.Email);
        if (existingEmployee != null)
            throw new ArgumentException($"Employee with email {createEmployeeDto.Email} already exists");

        var employee = new ComplyTest.Domain.Entities.Employee
        {
            FirstName = createEmployeeDto.FirstName,
            LastName = createEmployeeDto.LastName,
            Email = createEmployeeDto.Email,
            Salary = createEmployeeDto.Salary,
            DepartmentId = createEmployeeDto.DepartmentId
        };
        
        var createdEmployee = await _employeeRepository.AddAsync(employee);
        return _employeeMapper.ToDto(createdEmployee);
    }

    public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
    {
        if (id <= 0)
            throw new ArgumentException("Employee ID must be a positive number", nameof(id));
            
        if (updateEmployeeDto == null)
            throw new ArgumentNullException(nameof(updateEmployeeDto));

        // Check if employee exists
        var existingEmployee = await _employeeRepository.GetByIdAsync(id);
        if (existingEmployee == null)
            throw new ArgumentException($"Employee with ID {id} does not exist");

        // Validate department exists
        var department = await _departmentRepository.GetByIdAsync(updateEmployeeDto.DepartmentId);
        if (department == null)
            throw new ArgumentException($"Department with ID {updateEmployeeDto.DepartmentId} does not exist");

        var employeeWithEmail = await _employeeRepository.GetByEmailAsync(updateEmployeeDto.Email);
        if (employeeWithEmail != null && employeeWithEmail.Id != id)
            throw new ArgumentException($"Employee with email {updateEmployeeDto.Email} already exists");

        var employee = new ComplyTest.Domain.Entities.Employee
        {
            Id = id,
            FirstName = updateEmployeeDto.FirstName,
            LastName = updateEmployeeDto.LastName,
            Email = updateEmployeeDto.Email,
            Salary = updateEmployeeDto.Salary,
            DepartmentId = updateEmployeeDto.DepartmentId
        };
        
        var updatedEmployee = await _employeeRepository.UpdateAsync(employee);
        if (updatedEmployee == null) return null;
        return _employeeMapper.ToDto(updatedEmployee);
    }

    public async Task<EmployeeDto?> DeleteEmployeeAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Employee ID must be a positive number", nameof(id));

        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null) return null;
        
        var deleted = await _employeeRepository.DeleteAsync(id);
        if (!deleted) return null;
        
        return _employeeMapper.ToDto(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(int departmentId)
    {
        if (departmentId <= 0)
            throw new ArgumentException("Department ID must be a positive number", nameof(departmentId));

        var employees = await _employeeRepository.GetEmployeesByDepartmentAsync(departmentId);
        return employees.Select(_employeeMapper.ToDto);
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesByProjectAsync(int projectId)
    {
        if (projectId <= 0)
            throw new ArgumentException("Project ID must be a positive number", nameof(projectId));

        var employees = await _employeeRepository.GetEmployeesByProjectAsync(projectId);
        return employees.Select(_employeeMapper.ToDto);
    }
}