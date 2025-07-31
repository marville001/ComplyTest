using ComplyTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComplyTest.Infrastructure.Persistence;

public class ComplyTestDbContext : DbContext
{
    public ComplyTestDbContext(DbContextOptions<ComplyTestDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<EmployeeProject> EmployeeProjects { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ComplyTestDbContext).Assembly);
    }

    public async Task SeedDataAsync()
    {
        // Only seed if no departments exist
        if (!Departments.Any())
        {
            var itDepartment = new Department
            {
                Name = "Information Technology",
                OfficeLocation = "Building A, Floor 3",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            
            var hrDepartment = new Department
            {
                Name = "Human Resources",
                OfficeLocation = "Building B, Floor 1",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            
            var financeDepartment = new Department
            {
                Name = "Finance",
                OfficeLocation = "Building A, Floor 2",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            var departments = new List<Department> { itDepartment, hrDepartment, financeDepartment };
            await Departments.AddRangeAsync(departments);
            await SaveChangesAsync();

            // Mapping of department names to their generated IDs
            var departmentIds = new Dictionary<string, int>
            {
                { "Information Technology", itDepartment.Id },
                { "Human Resources", hrDepartment.Id },
                { "Finance", financeDepartment.Id }
            };

            var employees = new List<Employee>
            {
                new Employee
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@gmail.com",
                    Salary = 75000,
                    DepartmentId = departmentIds["Information Technology"],
                    CreatedDate = DateTime.UtcNow
                },
                new Employee
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@gmail.com",
                    Salary = 65000,
                    DepartmentId = departmentIds["Human Resources"],
                    CreatedDate = DateTime.UtcNow
                },
                new Employee
                {
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob.johnson@gmail.com",
                    Salary = 80000,
                    DepartmentId = departmentIds["Information Technology"],
                    CreatedDate = DateTime.UtcNow
                }
            };

            await Employees.AddRangeAsync(employees);
            await SaveChangesAsync();
        }
    }
}