using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComplyTest.Application.DTOs;
using ComplyTest.Application.Features.Department.Services;
using ComplyTest.Application.Mappers.Department;
using ComplyTest.Domain.Entities;
using ComplyTest.Domain.Interfaces;
using Moq;
using Xunit;

namespace ComplyTest.Test.Features.Department.Services;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _mockDepartmentRepository;
    private readonly Mock<IDepartmentMapper> _mockDepartmentMapper;
    private readonly DepartmentService _departmentService;

    public DepartmentServiceTests()
    {
        _mockDepartmentRepository = new Mock<IDepartmentRepository>();
        _mockDepartmentMapper = new Mock<IDepartmentMapper>();
        
        _departmentService = new DepartmentService(
            _mockDepartmentRepository.Object,
            _mockDepartmentMapper.Object);
    }

    [Fact]
    public async Task GetDepartmentAsync_WhenDepartmentExists_ReturnsDepartmentDto()
    {
        // Arrange
        var departmentId = 1;
        var department = new ComplyTest.Domain.Entities.Department 
        { 
            Id = departmentId, 
            Name = "IT Department",
            OfficeLocation = "Building A",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        var departmentDto = new DepartmentDto 
        { 
            Id = departmentId, 
            Name = "IT Department",
            OfficeLocation = "Building A"
        };
        
        _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
            .ReturnsAsync(department);
        _mockDepartmentMapper.Setup(mapper => mapper.ToDto(department))
            .Returns(departmentDto);

        // Act
        var result = await _departmentService.GetDepartmentAsync(departmentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(departmentId, result.Id);
        _mockDepartmentRepository.Verify(repo => repo.GetByIdAsync(departmentId), Times.Once);
        _mockDepartmentMapper.Verify(mapper => mapper.ToDto(department), Times.Once);
    }

    [Fact]
    public async Task GetDepartmentAsync_WhenDepartmentDoesNotExist_ReturnsNull()
    {
        // Arrange
        var departmentId = 1;
        _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
            .Returns(Task.FromResult<ComplyTest.Domain.Entities.Department?>(null));

        // Act
        var result = await _departmentService.GetDepartmentAsync(departmentId);

        // Assert
        Assert.Null(result);
        _mockDepartmentRepository.Verify(repo => repo.GetByIdAsync(departmentId), Times.Once);
        _mockDepartmentMapper.Verify(mapper => mapper.ToDto(It.IsAny<ComplyTest.Domain.Entities.Department>()), Times.Never);
    }

    [Fact]
    public async Task GetAllDepartmentsAsync_ReturnsAllDepartmentsAsDtos()
    {
        // Arrange
        var departments = new List<ComplyTest.Domain.Entities.Department>
        {
            new ComplyTest.Domain.Entities.Department 
            { 
                Id = 1, 
                Name = "IT Department",
                OfficeLocation = "Building A",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new ComplyTest.Domain.Entities.Department 
            { 
                Id = 2, 
                Name = "HR Department",
                OfficeLocation = "Building B",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            }
        };
        
        var departmentDtos = new List<DepartmentDto>
        {
            new DepartmentDto 
            { 
                Id = 1, 
                Name = "IT Department",
                OfficeLocation = "Building A"
            },
            new DepartmentDto 
            { 
                Id = 2, 
                Name = "HR Department",
                OfficeLocation = "Building B"
            }
        };
        
        _mockDepartmentRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(departments);
        _mockDepartmentMapper.Setup(mapper => mapper.ToDto(It.IsAny<ComplyTest.Domain.Entities.Department>()))
            .Returns<ComplyTest.Domain.Entities.Department>(d => departmentDtos.First(dto => dto.Id == d.Id));

        // Act
        var result = await _departmentService.GetAllDepartmentsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockDepartmentRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateDepartmentAsync_CreatesDepartment()
    {
        // Arrange
        var createDepartmentDto = new CreateDepartmentDto
        {
            Name = "IT Department",
            OfficeLocation = "Building A"
        };
        
        var department = new ComplyTest.Domain.Entities.Department
        {
            Id = 1,
            Name = createDepartmentDto.Name,
            OfficeLocation = createDepartmentDto.OfficeLocation,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        
        var departmentDto = new DepartmentDto
        {
            Id = 1,
            Name = createDepartmentDto.Name,
            OfficeLocation = createDepartmentDto.OfficeLocation
        };
        
        _mockDepartmentRepository.Setup(repo => repo.AddAsync(It.IsAny<ComplyTest.Domain.Entities.Department>()))
            .ReturnsAsync(department);
        _mockDepartmentMapper.Setup(mapper => mapper.ToDto(department))
            .Returns(departmentDto);

        // Act
        var result = await _departmentService.CreateDepartmentAsync(createDepartmentDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDepartmentDto.Name, result.Name);
        _mockDepartmentRepository.Verify(repo => repo.AddAsync(It.IsAny<ComplyTest.Domain.Entities.Department>()), Times.Once);
        _mockDepartmentMapper.Verify(mapper => mapper.ToDto(department), Times.Once);
    }

    [Fact]
    public async Task UpdateDepartmentAsync_WhenDepartmentExists_ReturnsUpdatedDepartmentDto()
    {
        // Arrange
        var departmentId = 1;
        var updateDepartmentDto = new UpdateDepartmentDto
        {
            Name = "Updated IT Department",
            OfficeLocation = "Building C"
        };
        
        var department = new ComplyTest.Domain.Entities.Department
        {
            Id = departmentId,
            Name = updateDepartmentDto.Name,
            OfficeLocation = updateDepartmentDto.OfficeLocation,
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            UpdatedDate = DateTime.UtcNow
        };
        
        var departmentDto = new DepartmentDto
        {
            Id = departmentId,
            Name = updateDepartmentDto.Name,
            OfficeLocation = updateDepartmentDto.OfficeLocation
        };
        
        _mockDepartmentRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Department>()))
            .ReturnsAsync(department);
        _mockDepartmentMapper.Setup(mapper => mapper.ToDto(department))
            .Returns(departmentDto);

        // Act
        var result = await _departmentService.UpdateDepartmentAsync(departmentId, updateDepartmentDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateDepartmentDto.Name, result.Name);
        Assert.Equal(updateDepartmentDto.OfficeLocation, result.OfficeLocation);
        _mockDepartmentRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Department>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDepartmentAsync_WhenDepartmentDoesNotExist_ReturnsNull()
    {
        // Arrange
        var departmentId = 1;
        var updateDepartmentDto = new UpdateDepartmentDto
        {
            Name = "Updated IT Department",
            OfficeLocation = "Building C"
        };
        
        _mockDepartmentRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Department>()))
            .Returns(Task.FromResult<ComplyTest.Domain.Entities.Department?>(null));

        // Act
        var result = await _departmentService.UpdateDepartmentAsync(departmentId, updateDepartmentDto);

        // Assert
        Assert.Null(result);
        _mockDepartmentRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Department>()), Times.Once);
    }

    [Fact]
    public async Task DeleteDepartmentAsync_WhenDepartmentExists_ReturnsDepartmentDto()
    {
        // Arrange
        var departmentId = 1;
        var department = new ComplyTest.Domain.Entities.Department 
        { 
            Id = departmentId, 
            Name = "IT Department",
            OfficeLocation = "Building A",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        var departmentDto = new DepartmentDto 
        { 
            Id = departmentId, 
            Name = "IT Department",
            OfficeLocation = "Building A"
        };
        
        _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
            .ReturnsAsync(department);
        _mockDepartmentRepository.Setup(repo => repo.DeleteAsync(departmentId))
            .ReturnsAsync(true);
        _mockDepartmentMapper.Setup(mapper => mapper.ToDto(department))
            .Returns(departmentDto);

        // Act
        var result = await _departmentService.DeleteDepartmentAsync(departmentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(departmentId, result.Id);
        _mockDepartmentRepository.Verify(repo => repo.GetByIdAsync(departmentId), Times.Once);
        _mockDepartmentRepository.Verify(repo => repo.DeleteAsync(departmentId), Times.Once);
    }

    [Fact]
    public async Task DeleteDepartmentAsync_WhenDepartmentDoesNotExist_ReturnsNull()
    {
        // Arrange
        var departmentId = 1;
        _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
            .Returns(Task.FromResult<ComplyTest.Domain.Entities.Department?>(null));

        // Act
        var result = await _departmentService.DeleteDepartmentAsync(departmentId);

        // Assert
        Assert.Null(result);
        _mockDepartmentRepository.Verify(repo => repo.GetByIdAsync(departmentId), Times.Once);
        _mockDepartmentRepository.Verify(repo => repo.DeleteAsync(departmentId), Times.Never);
    }

    [Fact]
    public async Task GetTotalProjectBudgetAsync_ReturnsTotalBudget()
    {
        // Arrange
        var departmentId = 1;
        var expectedBudget = 100000m;
        
        _mockDepartmentRepository.Setup(repo => repo.GetTotalProjectBudgetAsync(departmentId))
            .ReturnsAsync(expectedBudget);

        // Act
        var result = await _departmentService.GetTotalProjectBudgetAsync(departmentId);

        // Assert
        Assert.Equal(expectedBudget, result);
        _mockDepartmentRepository.Verify(repo => repo.GetTotalProjectBudgetAsync(departmentId), Times.Once);
    }
}