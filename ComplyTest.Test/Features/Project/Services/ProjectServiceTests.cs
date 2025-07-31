using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComplyTest.Application.DTOs;
using ComplyTest.Application.Features.Project.Services;
using ComplyTest.Application.Mappers.Project;
using ComplyTest.Domain.Entities;
using ComplyTest.Domain.Interfaces;
using Moq;
using Xunit;

namespace ComplyTest.Test.Features.Project.Services;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly Mock<IProjectMapper> _mockProjectMapper;
    private readonly Mock<IRandomStringGeneratorService> _mockRandomStringGeneratorService;
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _mockProjectMapper = new Mock<IProjectMapper>();
        _mockRandomStringGeneratorService = new Mock<IRandomStringGeneratorService>();
        
        _projectService = new ProjectService(
            _mockProjectRepository.Object,
            _mockProjectMapper.Object,
            _mockRandomStringGeneratorService.Object);
    }

    [Fact]
    public async Task GetProjectAsync_WhenProjectExists_ReturnsProjectDto()
    {
        // Arrange
        var projectId = 1;
        var project = new ComplyTest.Domain.Entities.Project 
        { 
            Id = projectId, 
            Name = "Test Project",
            ProjectCode = "PROJ001",
            Budget = 10000,
            DepartmentId = 1,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        var projectDto = new ProjectDto { Id = projectId, Name = "Test Project" };
        
        _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId))
            .ReturnsAsync(project);
        _mockProjectMapper.Setup(mapper => mapper.ToDto(project))
            .Returns(projectDto);

        // Act
        var result = await _projectService.GetProjectAsync(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.Id);
        _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);
        _mockProjectMapper.Verify(mapper => mapper.ToDto(project), Times.Once);
    }

    [Fact]
    public async Task GetProjectAsync_WhenProjectDoesNotExist_ReturnsNull()
    {
        // Arrange
        var projectId = 1;
        _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId))
            .Returns(Task.FromResult<ComplyTest.Domain.Entities.Project?>(null));

        // Act
        var result = await _projectService.GetProjectAsync(projectId);

        // Assert
        Assert.Null(result);
        _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);
        _mockProjectMapper.Verify(mapper => mapper.ToDto(It.IsAny<ComplyTest.Domain.Entities.Project>()), Times.Never);
    }

    [Fact]
    public async Task GetAllProjectsAsync_ReturnsAllProjectsAsDtos()
    {
        // Arrange
        var projects = new List<ComplyTest.Domain.Entities.Project>
        {
            new ComplyTest.Domain.Entities.Project 
            { 
                Id = 1, 
                Name = "Project 1",
                ProjectCode = "PROJ001",
                Budget = 10000,
                DepartmentId = 1,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new ComplyTest.Domain.Entities.Project 
            { 
                Id = 2, 
                Name = "Project 2",
                ProjectCode = "PROJ002",
                Budget = 15000,
                DepartmentId = 1,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            }
        };
        
        var projectDtos = new List<ProjectDto>
        {
            new ProjectDto { Id = 1, Name = "Project 1" },
            new ProjectDto { Id = 2, Name = "Project 2" }
        };
        
        _mockProjectRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(projects);
        _mockProjectMapper.Setup(mapper => mapper.ToDto(It.IsAny<ComplyTest.Domain.Entities.Project>()))
            .Returns<ComplyTest.Domain.Entities.Project>(p => projectDtos.First(dto => dto.Id == p.Id));

        // Act
        var result = await _projectService.GetAllProjectsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockProjectRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateProjectAsync_CreatesProjectWithGeneratedCode()
    {
        // Arrange
        var createProjectDto = new CreateProjectDto
        {
            Name = "New Project",
            Budget = 10000,
            DepartmentId = 1
        };
        
        var addedProject = new ComplyTest.Domain.Entities.Project
        {
            Id = 1,
            Name = createProjectDto.Name,
            Budget = createProjectDto.Budget,
            DepartmentId = createProjectDto.DepartmentId,
            ProjectCode = "",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        
        var updatedProject = new ComplyTest.Domain.Entities.Project
        {
            Id = 1,
            Name = createProjectDto.Name,
            Budget = createProjectDto.Budget,
            DepartmentId = createProjectDto.DepartmentId,
            ProjectCode = "ABC1231",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        
        var projectDto = new ProjectDto
        {
            Id = 1,
            Name = createProjectDto.Name,
            Budget = createProjectDto.Budget,
            DepartmentId = createProjectDto.DepartmentId,
            ProjectCode = "ABC1231"
        };
        
        // Mock the repository to return the same project instance for both Add and Update
        _mockProjectRepository.Setup(repo => repo.AddAsync(It.IsAny<ComplyTest.Domain.Entities.Project>()))
            .ReturnsAsync(addedProject);
        _mockRandomStringGeneratorService.Setup(service => service.GenerateRandomStringAsync())
            .ReturnsAsync("ABC123");
        _mockProjectRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Project>()))
            .ReturnsAsync(updatedProject);
        _mockProjectMapper.Setup(mapper => mapper.ToDto(It.IsAny<ComplyTest.Domain.Entities.Project>()))
            .Returns(projectDto);

        // Act
        var result = await _projectService.CreateProjectAsync(createProjectDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ABC1231", result.ProjectCode);
        _mockProjectRepository.Verify(repo => repo.AddAsync(It.IsAny<ComplyTest.Domain.Entities.Project>()), Times.Once);
        _mockRandomStringGeneratorService.Verify(service => service.GenerateRandomStringAsync(), Times.Once);
        _mockProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Project>()), Times.Once);
        _mockProjectMapper.Verify(mapper => mapper.ToDto(It.IsAny<ComplyTest.Domain.Entities.Project>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProjectAsync_WhenProjectExists_ReturnsUpdatedProjectDto()
    {
        // Arrange
        var projectId = 1;
        var updateProjectDto = new UpdateProjectDto
        {
            Name = "Updated Project",
            Budget = 15000,
            DepartmentId = 2
        };
        
        var existingProject = new ComplyTest.Domain.Entities.Project
        {
            Id = projectId,
            Name = "Original Project",
            ProjectCode = "PROJ001",
            Budget = 10000,
            DepartmentId = 1,
            UpdatedDate = DateTime.UtcNow.AddDays(-1),
            CreatedDate = DateTime.UtcNow.AddDays(-1)
        };
        
        var updatedProject = new ComplyTest.Domain.Entities.Project
        {
            Id = projectId,
            Name = updateProjectDto.Name,
            ProjectCode = "PROJ001",
            Budget = updateProjectDto.Budget,
            DepartmentId = updateProjectDto.DepartmentId,
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            UpdatedDate = DateTime.UtcNow
        };
        
        var projectDto = new ProjectDto
        {
            Id = projectId,
            Name = updateProjectDto.Name,
            Budget = updateProjectDto.Budget,
            DepartmentId = updateProjectDto.DepartmentId
        };
        
        _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId))
            .ReturnsAsync(existingProject);
        _mockProjectRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Project>()))
            .ReturnsAsync(updatedProject);
        _mockProjectMapper.Setup(mapper => mapper.ToDto(updatedProject))
            .Returns(projectDto);

        // Act
        var result = await _projectService.UpdateProjectAsync(projectId, updateProjectDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateProjectDto.Name, result.Name);
        Assert.Equal(updateProjectDto.Budget, result.Budget);
        _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);
        _mockProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Project>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProjectAsync_WhenProjectDoesNotExist_ReturnsNull()
    {
        // Arrange
        var projectId = 1;
        var updateProjectDto = new UpdateProjectDto
        {
            Name = "Updated Project",
            Budget = 15000,
            DepartmentId = 2
        };
        
        _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId))
            .Returns(Task.FromResult<ComplyTest.Domain.Entities.Project?>(null));

        // Act
        var result = await _projectService.UpdateProjectAsync(projectId, updateProjectDto);

        // Assert
        Assert.Null(result);
        _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);
        _mockProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ComplyTest.Domain.Entities.Project>()), Times.Never);
    }

    [Fact]
    public async Task DeleteProjectAsync_WhenProjectExists_ReturnsProjectDto()
    {
        // Arrange
        var projectId = 1;
        var project = new ComplyTest.Domain.Entities.Project 
        { 
            Id = projectId, 
            Name = "Test Project",
            ProjectCode = "PROJ001",
            Budget = 10000,
            DepartmentId = 1,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        var projectDto = new ProjectDto { Id = projectId, Name = "Test Project" };
        
        _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId))
            .ReturnsAsync(project);
        _mockProjectRepository.Setup(repo => repo.DeleteAsync(projectId))
            .ReturnsAsync(true);
        _mockProjectMapper.Setup(mapper => mapper.ToDto(project))
            .Returns(projectDto);

        // Act
        var result = await _projectService.DeleteProjectAsync(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.Id);
        _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);
        _mockProjectRepository.Verify(repo => repo.DeleteAsync(projectId), Times.Once);
    }

    [Fact]
    public async Task DeleteProjectAsync_WhenProjectDoesNotExist_ReturnsNull()
    {
        // Arrange
        var projectId = 1;
        _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId))
            .Returns(Task.FromResult<ComplyTest.Domain.Entities.Project?>(null));

        // Act
        var result = await _projectService.DeleteProjectAsync(projectId);

        // Assert
        Assert.Null(result);
        _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);
        _mockProjectRepository.Verify(repo => repo.DeleteAsync(projectId), Times.Never);
    }
}