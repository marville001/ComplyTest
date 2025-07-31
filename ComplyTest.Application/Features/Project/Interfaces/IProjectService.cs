using ComplyTest.Application.DTOs;

namespace ComplyTest.Application.Features.Project.Interfaces;

public interface IProjectService
{
    Task<ProjectDto?> GetProjectAsync(int id);
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
    Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto);
    Task<ProjectDto?> DeleteProjectAsync(int id);
    Task<IEnumerable<ProjectDto>> GetProjectsByEmployeeAsync(int employeeId);
    Task<bool> AssignEmployeeToProjectAsync(int employeeId, int projectId, string role);
    Task<bool> RemoveEmployeeFromProjectAsync(int employeeId, int projectId);
}