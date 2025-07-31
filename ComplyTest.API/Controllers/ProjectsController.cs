using Microsoft.AspNetCore.Mvc;
using ComplyTest.Application.DTOs;
using ComplyTest.Application.Features.Project.Interfaces;

namespace ComplyTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Get all projects
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    /// <summary>
    /// Get project by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var project = await _projectService.GetProjectAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        return Ok(project);
    }

    /// <summary>
    /// Create a new project
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto createProjectDto)
    {
        try
        {
            var project = await _projectService.CreateProjectAsync(createProjectDto);
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update an existing project
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> UpdateProject(int id, UpdateProjectDto updateProjectDto)
    {
        try
        {
            var project = await _projectService.UpdateProjectAsync(id, updateProjectDto);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete a project
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ProjectDto>> DeleteProject(int id)
    {
        var project = await _projectService.DeleteProjectAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        return Ok(project);
    }

    /// <summary>
    /// Get projects by employee ID
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjectsByEmployee(int employeeId)
    {
        var projects = await _projectService.GetProjectsByEmployeeAsync(employeeId);
        return Ok(projects);
    }

    /// <summary>
    /// Assign employee to project
    /// </summary>
    [HttpPost("{projectId}/assign-employee")]
    public async Task<ActionResult<bool>> AssignEmployeeToProject(int projectId, [FromBody] AssignEmployeeDto assignEmployeeDto)
    {
        try
        {
            var result = await _projectService.AssignEmployeeToProjectAsync(assignEmployeeDto.EmployeeId, projectId, assignEmployeeDto.Role);
            if (!result)
            {
                return BadRequest("Failed to assign employee to project");
            }
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Remove employee from project
    /// </summary>
    [HttpPost("{projectId}/remove-employee")]
    public async Task<ActionResult<bool>> RemoveEmployeeFromProject(int projectId, [FromBody] RemoveEmployeeDto removeEmployeeDto)
    {
        try
        {
            var result = await _projectService.RemoveEmployeeFromProjectAsync(removeEmployeeDto.EmployeeId, projectId);
            if (!result)
            {
                return BadRequest("Failed to remove employee from project");
            }
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class AssignEmployeeDto
{
    public int EmployeeId { get; set; }
    public string Role { get; set; } = string.Empty;
}

public class RemoveEmployeeDto
{
    public int EmployeeId { get; set; }
}