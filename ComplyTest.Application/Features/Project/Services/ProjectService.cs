using ComplyTest.Application.DTOs;
using ComplyTest.Application.Features.Project.Interfaces;
using ComplyTest.Application.Mappers.Project;
using ComplyTest.Domain.Interfaces;

namespace ComplyTest.Application.Features.Project.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMapper _projectMapper;
    private readonly IRandomStringGeneratorService _randomStringGeneratorService;

    public ProjectService(
        IProjectRepository projectRepository,
        IProjectMapper projectMapper,
        IRandomStringGeneratorService randomStringGeneratorService)
    {
        _projectRepository = projectRepository;
        _projectMapper = projectMapper;
        _randomStringGeneratorService = randomStringGeneratorService;
    }

    public async Task<ProjectDto?> GetProjectAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) return null;
        return _projectMapper.ToDto(project);
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return projects.Select(_projectMapper.ToDto);
    }

    public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto)
    {
        var project = new ComplyTest.Domain.Entities.Project
        {
            Name = createProjectDto.Name,
            Budget = createProjectDto.Budget,
            DepartmentId = createProjectDto.DepartmentId,
            ProjectCode = "",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        var addedProject = await _projectRepository.AddProjectWithCodeAsync(project, _randomStringGeneratorService.GenerateRandomStringAsync);
        
        return _projectMapper.ToDto(addedProject);
    }

    public async Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) return null;

        project.Name = updateProjectDto.Name;
        project.Budget = updateProjectDto.Budget;
        project.DepartmentId = updateProjectDto.DepartmentId;
        project.UpdatedDate = DateTime.UtcNow;

        var updatedProject = await _projectRepository.UpdateAsync(project);
        if (updatedProject == null) return null;
        
        return _projectMapper.ToDto(updatedProject);
    }

    public async Task<ProjectDto?> DeleteProjectAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) return null;
        
        var deleted = await _projectRepository.DeleteAsync(id);
        if (!deleted) return null;
        
        return _projectMapper.ToDto(project);
    }

    public async Task<IEnumerable<ProjectDto>> GetProjectsByEmployeeAsync(int employeeId)
    {
        var projects = await _projectRepository.GetProjectsByEmployeeAsync(employeeId);
        return projects.Select(_projectMapper.ToDto);
    }

    public async Task<bool> AssignEmployeeToProjectAsync(int employeeId, int projectId, string role)
    {
        return await _projectRepository.AssignEmployeeToProjectAsync(employeeId, projectId, role);
    }

    public async Task<bool> RemoveEmployeeFromProjectAsync(int employeeId, int projectId)
    {
        return await _projectRepository.RemoveEmployeeFromProjectAsync(employeeId, projectId);
    }
}