using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComplyTest.Application.DTOs;
using ComplyTest.Application.Features.Employee.Services;
using ComplyTest.Application.Mappers.Employee;
using ComplyTest.Domain.Entities;
using ComplyTest.Domain.Interfaces;
using Moq;
using Xunit;

namespace ComplyTest.Test.Features.Employee.Services;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IEmployeeMapper> _mockEmployeeMapper;
    private readonly Mock<IDepartmentRepository> _mockDepartmentRepository;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _mockEmployeeMapper = new Mock<IEmployeeMapper>();
        _mockDepartmentRepository = new Mock<IDepartmentRepository>();
        
        _employeeService = new EmployeeService(
            _mockEmployeeRepository.Object,
            _mockEmployeeMapper.Object,
            _mockDepartmentRepository.Object);
    }

    [Fact]
    public async Task GetEmployeeAsync_WhenEmployeeExists_ReturnsEmployeeDto()
    {
        // Arrange
        var employeeId = 1;
        var employee = new ComplyTest.Domain.Entities.Employee 
        { 
            Id = employeeId, 
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Salary = 50000,
            DepartmentId = 1,
            CreatedDate = DateTime.UtcNow
        };
        var employeeDto = new EmployeeDto 
        { 
            Id = employeeId, 
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Salary = 50000
        };
        
        _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
            .ReturnsAsync(employee);
        _mockEmployeeMapper.Setup(mapper => mapper.ToDto(employee))
            .Returns(employeeDto);

        // Act
        var result = await _employeeService.GetEmployeeAsync(employeeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employeeId, result.Id);
        _mockEmployeeRepository.Verify(repo => repo.GetByIdAsync(employeeId), Times.Once);
        _mockEmployeeMapper.Verify(mapper => mapper.ToDto(employee), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeAsync_WhenEmployeeDoesNotExist_ReturnsNull()
    {
        // Arrange
        var employeeId = 1;
        _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
            .ReturnsAsync((ComplyTest.Domain.Entities.Employee?)null);

        // Act
        var result = await _employeeService.GetEmployeeAsync(employeeId);

        // Assert
        Assert.Null(result);
        _mockEmployeeRepository.Verify(repo => repo.GetByIdAsync(employeeId), Times.Once);
        _mockEmployeeMapper.Verify(mapper => mapper.ToDto(It.IsAny<ComplyTest.Domain.Entities.Employee>()), Times.Never);
    }

    [Fact]
    public async Task GetAllEmployeesAsync_ReturnsAllEmployeesAsDtos()
    {
        // Arrange
        var employees = new List<ComplyTest.Domain.Entities.Employee>
        {
            new ComplyTest.Domain.Entities.Employee 
            { 
                Id = 1, 
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Salary = 50000,
                DepartmentId = 1,
                CreatedDate = DateTime.UtcNow
            },
            new ComplyTest.Domain.Entities.Employee 
            { 
                Id = 2, 
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Salary = 60000,
                DepartmentId = 1,
                CreatedDate = DateTime.UtcNow
            }
        };
        
        var employeeDtos = new List<EmployeeDto>
        {
            new EmployeeDto 
            { 
                Id = 1, 
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Salary = 50000
            },
            new EmployeeDto 
            { 
                Id = 2, 
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Salary = 60000
            }
        };
        
        _mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(employees);
        _mockEmployeeMapper.Setup(mapper => mapper.ToDto(It.IsAny<ComplyTest.Domain.Entities.Employee>()))
            .Returns<ComplyTest.Domain.Entities.Employee>(e => employeeDtos.First(dto => dto.Id == e.Id));

        // Act
        var result = await _employeeService.GetAllEmployeesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockEmployeeRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateEmployeeAsync_CreatesEmployee()
    {
        // Arrange
        var createEmployeeDto = new CreateEmployeeDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Salary = 50000,
            DepartmentId = 1
        };
        
        var department = new ComplyTest.Domain.Entities.Department
        {
            Id = 1,
            Name = "IT",
            OfficeLocation = "Building A"
        };
        
        var employee = new ComplyTest.Domain.Entities.Employee
        {
            Id = 1,
            FirstName = createEmployeeDto.FirstName,
            LastName = createEmployeeDto.LastName,
            Email = createEmployeeDto.Email,
            Salary = createEmployeeDto.Salary,
            DepartmentId = createEmployeeDto.DepartmentId,
            CreatedDate = DateTime.UtcNow
        };
        
        var employeeDto = new EmployeeDto
        {
            Id = 1,
            FirstName = createEmployeeDto.FirstName,
            LastName = createEmployeeDto.LastName,
            Email = createEmployeeDto.Email,
            Salary = createEmployeeDto.Salary
        };
        
        _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(createEmployeeDto.DepartmentId))
            .ReturnsAsync(department);
        _mockEmployeeRepository.Setup(repo => repo.GetByEmailAsync(createEmployeeDto.Email))
            .ReturnsAsync((ComplyTest.Domain.Entities.Employee?)null);
        _mockEmployeeRepository.Setup(repo => repo.AddAsync(It.IsAny<ComplyTest.Domain.Entities.Employee>()))
            .ReturnsAsync(employee);
        _mockEmployeeMapper.Setup(mapper => mapper.ToDto(employee))
            .Returns(employeeDto);

        // Act
        var result = await _employeeService.CreateEmployeeAsync(createEmployeeDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createEmployeeDto.FirstName, result.FirstName);
        _mockEmployeeRepository.Verify(repo => repo.AddAsync(It.IsAny<ComplyTest.Domain.Entities.Employee>()), Times.Once);
        _mockEmployeeMapper.Verify(mapper => mapper.ToDto(employee), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_WhenEmployeeExists_ReturnsUpdatedEmployeeDto()
    {
        // Arrange
        var employeeId = 1;
        var updateEmployeeDto = new UpdateEmployeeDto
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "john.smith@example.com",
            Salary = 55000,
            DepartmentId = 2
        };
        
        var department = new ComplyTest.Domain.Entities.Department
        {
            Id = 2,
            Name = "HR",
            OfficeLocation = "Building B"
        };
        
        var existingEmployee = new ComplyTest.Domain.Entities.Employee
        {
            Id = employeeId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Salary = 50000,
            DepartmentId = 1,
            CreatedDate = DateTime.UtcNow.AddDays(-1)
        };
        
        var employee = new ComplyTest.Domain.Entities.Employee
        {
            Id = employeeId,
            FirstName = updateEmployeeDto.FirstName,
            LastName = updateEmployeeDto.LastName,
            Email = updateEmployeeDto.Email,
            Salary = updateEmployeeDto.Salary,
            DepartmentId = updateEmployeeDto.DepartmentId,
            CreatedDate = DateTime.UtcNow.AddDays(-1)
        };
        
        var employeeDto = new EmployeeDto
        {
            Id = employeeId,
            FirstName = updateEmployeeDto.FirstName,
            LastName = updateEmployeeDto.LastName,
            Email = updateEmployeeDto.Email,
            Salary = updateEmployeeDto.Salary
        };
        
        _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
            .ReturnsAsync(existingEmployee);
        _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(updateEmployeeDto.DepartmentId))
            .ReturnsAsync(department);
        _mockEmployeeRepository.Setup(repo => repo.GetByEmailAsync(updateEmployeeDto.Email))
            .ReturnsAsync((ComplyTest.Domain.Entities.Employee?)null);
        _mockEmployeeRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Employee>()))
            .ReturnsAsync(employee);
        _mockEmployeeMapper.Setup(mapper => mapper.ToDto(employee))
            .Returns(employeeDto);

        // Act
        var result = await _employeeService.UpdateEmployeeAsync(employeeId, updateEmployeeDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateEmployeeDto.FirstName, result.FirstName);
        Assert.Equal(updateEmployeeDto.Salary, result.Salary);
        _mockEmployeeRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Employee>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_WhenEmployeeDoesNotExist_ThrowsException()
    {
        // Arrange
        var employeeId = 1;
        var updateEmployeeDto = new UpdateEmployeeDto
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "john.smith@example.com",
            Salary = 55000,
            DepartmentId = 2
        };
        
        _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
            .Returns(Task.FromResult<ComplyTest.Domain.Entities.Employee?>(null));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _employeeService.UpdateEmployeeAsync(employeeId, updateEmployeeDto));
        _mockEmployeeRepository.Verify(repo => repo.GetByIdAsync(employeeId), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeAsync_WhenEmployeeExists_ReturnsEmployeeDto()
    {
        // Arrange
        var employeeId = 1;
        var employee = new ComplyTest.Domain.Entities.Employee 
        { 
            Id = employeeId, 
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Salary = 50000,
            DepartmentId = 1,
            CreatedDate = DateTime.UtcNow
        };
        var employeeDto = new EmployeeDto 
        { 
            Id = employeeId, 
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Salary = 50000
        };
        
        _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
            .ReturnsAsync(employee);
        _mockEmployeeRepository.Setup(repo => repo.DeleteAsync(employeeId))
            .ReturnsAsync(true);
        _mockEmployeeMapper.Setup(mapper => mapper.ToDto(employee))
            .Returns(employeeDto);

        // Act
        var result = await _employeeService.DeleteEmployeeAsync(employeeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employeeId, result.Id);
        _mockEmployeeRepository.Verify(repo => repo.GetByIdAsync(employeeId), Times.Once);
        _mockEmployeeRepository.Verify(repo => repo.DeleteAsync(employeeId), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeAsync_WhenEmployeeDoesNotExist_ReturnsNull()
    {
        // Arrange
        var employeeId = 1;
        _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
            .ReturnsAsync((ComplyTest.Domain.Entities.Employee?)null);

        // Act
        var result = await _employeeService.DeleteEmployeeAsync(employeeId);

        // Assert
        Assert.Null(result);
        _mockEmployeeRepository.Verify(repo => repo.GetByIdAsync(employeeId), Times.Once);
        _mockEmployeeRepository.Verify(repo => repo.DeleteAsync(employeeId), Times.Never);
    }

    [Fact]
    public async Task GetEmployeesByDepartmentAsync_ReturnsEmployeesForDepartment()
    {
        // Arrange
        var departmentId = 1;
        var employees = new List<ComplyTest.Domain.Entities.Employee>
        {
            new ComplyTest.Domain.Entities.Employee 
            { 
                Id = 1, 
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Salary = 50000,
                DepartmentId = departmentId,
                CreatedDate = DateTime.UtcNow
            },
            new ComplyTest.Domain.Entities.Employee 
            { 
                Id = 2, 
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Salary = 60000,
                DepartmentId = departmentId,
                CreatedDate = DateTime.UtcNow
            }
        };
        
        var employeeDtos = new List<EmployeeDto>
        {
            new EmployeeDto 
            { 
                Id = 1, 
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Salary = 50000
            },
            new EmployeeDto 
            { 
                Id = 2, 
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Salary = 60000
            }
        };
        
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeesByDepartmentAsync(departmentId))
            .ReturnsAsync(employees);
        _mockEmployeeMapper.Setup(mapper => mapper.ToDto(It.IsAny<ComplyTest.Domain.Entities.Employee>()))
            .Returns<ComplyTest.Domain.Entities.Employee>(e => employeeDtos.First(dto => dto.Id == e.Id));

        // Act
        var result = await _employeeService.GetEmployeesByDepartmentAsync(departmentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockEmployeeRepository.Verify(repo => repo.GetEmployeesByDepartmentAsync(departmentId), Times.Once);
    }

    [Fact]
    public async Task GetEmployeesByProjectAsync_ReturnsEmployeesForProject()
    {
        // Arrange
        var projectId = 1;
        var employees = new List<ComplyTest.Domain.Entities.Employee>
        {
            new ComplyTest.Domain.Entities.Employee 
            { 
                Id = 1, 
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Salary = 50000,
                DepartmentId = 1,
                CreatedDate = DateTime.UtcNow
            },
            new ComplyTest.Domain.Entities.Employee 
            { 
                Id = 2, 
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Salary = 60000,
                DepartmentId = 1,
                CreatedDate = DateTime.UtcNow
            }
        };
        
        var employeeDtos = new List<EmployeeDto>
        {
            new EmployeeDto 
            { 
                Id = 1, 
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Salary = 50000
            },
            new EmployeeDto 
            { 
                Id = 2, 
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Salary = 60000
            }
        };
        
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeesByProjectAsync(projectId))
            .ReturnsAsync(employees);
        _mockEmployeeMapper.Setup(mapper => mapper.ToDto(It.IsAny<ComplyTest.Domain.Entities.Employee>()))
            .Returns<ComplyTest.Domain.Entities.Employee>(e => employeeDtos.First(dto => dto.Id == e.Id));

        // Act
        var result = await _employeeService.GetEmployeesByProjectAsync(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockEmployeeRepository.Verify(repo => repo.GetEmployeesByProjectAsync(projectId), Times.Once);
    }
}