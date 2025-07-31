using Microsoft.EntityFrameworkCore;
using ComplyTest.Domain.Entities;
using ComplyTest.Domain.Interfaces;
using ComplyTest.Infrastructure.Persistence;

namespace ComplyTest.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ComplyTestDbContext _context;

    public ProjectRepository(ComplyTestDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects
            .Include(p => p.Department)
            .Include(p => p.EmployeeProjects)
            .ThenInclude(ep => ep.Employee)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.Department)
            .Include(p => p.EmployeeProjects)
            .ThenInclude(ep => ep.Employee)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project> AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> AddProjectWithCodeAsync(Project project, Func<Task<string>> generateCodeFunction)
    {
        var executionStrategy = _context.Database.CreateExecutionStrategy();

        return await executionStrategy.Execute(
            async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Projects.AddAsync(project);
                    await _context.SaveChangesAsync();

                    var randomCode = await generateCodeFunction();

                    project.ProjectCode = $"{randomCode}{project.Id}";
                    project.UpdatedDate = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return project;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        );


    }

    public async Task<Project> UpdateAsync(Project project)
    {
        var existingProject = await _context.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);

        if (existingProject != null)
        {
            existingProject.Name = project.Name;
            existingProject.Budget = project.Budget;
            existingProject.ProjectCode = project.ProjectCode;
            existingProject.DepartmentId = project.DepartmentId;
            existingProject.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        return existingProject!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return false;
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Project>> GetProjectsByEmployeeAsync(int employeeId)
    {
        return await _context.Projects
            .Include(p => p.Department)
            .Include(p => p.EmployeeProjects)
            .ThenInclude(ep => ep.Employee)
            .Where(p => p.EmployeeProjects.Any(ep => ep.EmployeeId == employeeId))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> AssignEmployeeToProjectAsync(int employeeId, int projectId, string role)
    {
        // Check if assignment already exists
        var existingAssignment = await _context.EmployeeProjects
            .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);

        if (existingAssignment != null)
        {
            return false; // Assignment already exists
        }

        var employeeProject = new EmployeeProject
        {
            EmployeeId = employeeId,
            ProjectId = projectId,
            Role = role
        };

        await _context.EmployeeProjects.AddAsync(employeeProject);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveEmployeeFromProjectAsync(int employeeId, int projectId)
    {
        var employeeProject = await _context.EmployeeProjects
            .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);

        if (employeeProject == null)
        {
            return false; // Assignment doesn't exist
        }

        _context.EmployeeProjects.Remove(employeeProject);
        await _context.SaveChangesAsync();
        return true;
    }
}