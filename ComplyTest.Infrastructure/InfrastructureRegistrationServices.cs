using Microsoft.Extensions.DependencyInjection;
using ComplyTest.Domain.Interfaces;
using ComplyTest.Infrastructure.Repositories;
using ComplyTest.Infrastructure.Services;

namespace ComplyTest.Infrastructure;

public static class InfrastructureRegistrationServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Register new repositories
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        
        // Register RandomStringGenerator service
        services.AddHttpClient<IRandomStringGeneratorService, RandomStringGeneratorService>();

        return services;
    }
}