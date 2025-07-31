using Microsoft.EntityFrameworkCore;
using ComplyTest.Application;
using ComplyTest.Infrastructure;
using ComplyTest.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework with SQL Server Database
builder.Services.AddDbContext<ComplyTestDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ComplyTest.API")
            .EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null)
    )
);

// Application layer services
builder.Services.AddApplicationServices();

// Infrastructure layer services (e.g., repositories)
builder.Services.AddInfrastructureServices();

var app = builder.Build();

// Initialize database and apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ComplyTestDbContext>();
    
    // Wait for database to be available
    var maxRetries = 10;
    var retryDelay = TimeSpan.FromSeconds(2);
    
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            // Test connection
            await context.Database.CanConnectAsync();
            break;
        }
        catch (Exception)
        {
            if (i == maxRetries - 1)
            {
                throw;
            }
            
            Console.WriteLine($"Database not ready, retrying in {retryDelay.TotalSeconds} seconds... (Attempt {i + 1}/{maxRetries})");
            await Task.Delay(retryDelay);
        }
    }
    
    // Apply migrations
    await context.Database.MigrateAsync();
    
    // Seed initial data
    await context.SeedDataAsync();
    
    Console.WriteLine("Database initialized successfully!");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();