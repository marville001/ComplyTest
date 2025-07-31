using Microsoft.AspNetCore.Mvc;
using ComplyTest.Application.DTOs;
using ComplyTest.Application.Features.Department.Interfaces;

namespace ComplyTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    /// <summary>
    /// Get all departments
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        return Ok(departments);
    }

    /// <summary>
    /// Get department by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
    {
        var department = await _departmentService.GetDepartmentAsync(id);
        if (department == null)
        {
            return NotFound();
        }
        return Ok(department);
    }

    /// <summary>
    /// Create a new department
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentDto createDepartmentDto)
    {
        try
        {
            var department = await _departmentService.CreateDepartmentAsync(createDepartmentDto);
            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update an existing department
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<DepartmentDto>> UpdateDepartment(int id, UpdateDepartmentDto updateDepartmentDto)
    {
        try
        {
            var department = await _departmentService.UpdateDepartmentAsync(id, updateDepartmentDto);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete a department
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<DepartmentDto>> DeleteDepartment(int id)
    {
        var department = await _departmentService.DeleteDepartmentAsync(id);
        if (department == null)
        {
            return NotFound();
        }
        return Ok(department);
    }

    /// <summary>
    /// Get total project budget for a department
    /// </summary>
    [HttpGet("{id}/total-budget")]
    public async Task<ActionResult<decimal>> GetTotalProjectBudget(int id)
    {
        var totalBudget = await _departmentService.GetTotalProjectBudgetAsync(id);
        return Ok(totalBudget);
    }
}