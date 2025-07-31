using Microsoft.EntityFrameworkCore;
using ComplyTest.Domain.Entities;
using ComplyTest.Domain.Interfaces;
using ComplyTest.Infrastructure.Persistence;

namespace ComplyTest.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ComplyTestDbContext _context;

    public DepartmentRepository(ComplyTestDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments
            .Include(d => d.Employees)
            .Include(d => d.Projects)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments
            .Include(d => d.Employees)
            .Include(d => d.Projects)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Department> AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
        return department;
    }

    public async Task<Department> UpdateAsync(Department department)
    {
        var existingDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Id == department.Id);
        
        if (existingDepartment != null)
        {
            existingDepartment.Name = department.Name;
            existingDepartment.OfficeLocation = department.OfficeLocation;
            existingDepartment.UpdatedDate = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }
        
        return existingDepartment!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        
        if (department == null)
        {
            return false;
        }
        
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> GetTotalProjectBudgetAsync(int departmentId)
    {
        return await _context.Projects
            .Where(p => p.DepartmentId == departmentId)
            .SumAsync(p => p.Budget);
    }
}