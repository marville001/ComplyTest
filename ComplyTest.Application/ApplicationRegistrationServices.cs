using Microsoft.Extensions.DependencyInjection;
using ComplyTest.Application.Features.Employee.Interfaces;
using ComplyTest.Application.Features.Employee.Services;
using ComplyTest.Application.Features.Department.Interfaces;
using ComplyTest.Application.Features.Department.Services;
using ComplyTest.Application.Features.Project.Interfaces;
using ComplyTest.Application.Features.Project.Services;
using ComplyTest.Application.Mappers.Employee;
using ComplyTest.Application.Mappers.Department;
using ComplyTest.Application.Mappers.Project;
using ComplyTest.Application.MappingProfiles;

namespace ComplyTest.Application;

public static class ApplicationRegistrationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddTransient<IEmployeeMapper, EmployeeMapper>();
        services.AddTransient<IDepartmentMapper, DepartmentMapper>();
        services.AddTransient<IProjectMapper, ProjectMapper>();

        services.AddAutoMapper(typeof(EmployeeMappingProfile));
        services.AddAutoMapper(typeof(DepartmentMappingProfile));
        services.AddAutoMapper(typeof(ProjectMappingProfile));

        return services;
    }
    
}