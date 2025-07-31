using Microsoft.AspNetCore.Mvc;
using ComplyTest.Application.DTOs;
using ComplyTest.Application.Features.Employee.Interfaces;

namespace ComplyTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto createEmployeeDto)
    {
        try
        {
            var employee = await _employeeService.CreateEmployeeAsync(createEmployeeDto);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update an existing employee
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
    {
        try
        {
            var employee = await _employeeService.UpdateEmployeeAsync(id, updateEmployeeDto);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete an employee
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<EmployeeDto>> DeleteEmployee(int id)
    {
        var employee = await _employeeService.DeleteEmployeeAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    /// <summary>
    /// Get employees by department ID
    /// </summary>
    [HttpGet("department/{departmentId}")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByDepartment(int departmentId)
    {
        var employees = await _employeeService.GetEmployeesByDepartmentAsync(departmentId);
        return Ok(employees);
    }

    /// <summary>
    /// Get employees by project ID
    /// </summary>
    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByProject(int projectId)
    {
        var employees = await _employeeService.GetEmployeesByProjectAsync(projectId);
        return Ok(employees);
    }
}