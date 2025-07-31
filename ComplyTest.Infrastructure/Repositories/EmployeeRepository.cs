using Microsoft.EntityFrameworkCore;
using ComplyTest.Domain.Entities;
using ComplyTest.Domain.Interfaces;
using ComplyTest.Infrastructure.Persistence;

namespace ComplyTest.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ComplyTestDbContext _context;

    public EmployeeRepository(ComplyTestDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.EmployeeProjects)
            .ThenInclude(ep => ep.Project)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.EmployeeProjects)
            .ThenInclude(ep => ep.Project)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.EmployeeProjects)
            .ThenInclude(ep => ep.Project)
            .FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id);
        
        if (existingEmployee != null)
        {
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.DepartmentId = employee.DepartmentId;
            
            await _context.SaveChangesAsync();
        }
        
        return existingEmployee!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        
        if (employee == null)
        {
            return false;
        }
        
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.EmployeeProjects)
            .ThenInclude(ep => ep.Project)
            .Where(e => e.DepartmentId == departmentId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByProjectAsync(int projectId)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.EmployeeProjects)
            .ThenInclude(ep => ep.Project)
            .Where(e => e.EmployeeProjects.Any(ep => ep.ProjectId == projectId))
            .AsNoTracking()
            .ToListAsync();
    }
}