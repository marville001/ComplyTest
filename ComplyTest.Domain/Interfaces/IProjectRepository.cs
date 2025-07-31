using ComplyTest.Domain.Entities;

namespace ComplyTest.Domain.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<Project> AddAsync(Project project);
    Task<Project> UpdateAsync(Project project);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Project>> GetProjectsByEmployeeAsync(int employeeId);
    Task<bool> AssignEmployeeToProjectAsync(int employeeId, int projectId, string role);
    Task<bool> RemoveEmployeeFromProjectAsync(int employeeId, int projectId);
    Task<Project> AddProjectWithCodeAsync(Project project, Func<Task<string>> generateCodeFunction);
}