# ComplyTest - Employee, Department & Project Management API

A RESTful API built with ASP.NET Core Web API for managing employees, departments, and projects in a company.

## Features

- **Employee Management**: CRUD operations for employees with department assignments
- **Department Management**: CRUD operations for departments with office locations
- **Project Management**: CRUD operations for projects with budgets and unique project codes
- **Employee-Project Assignments**: Assign/remove employees to/from projects with specific roles
- **Budget Tracking**: Calculate total project budgets by department
- **External Integration**: Random string generation for project codes via external API

## Technology Stack

- **.NET 9** with ASP.NET Core Web API
- **Entity Framework Core** with SQL Server
- **Clean Architecture** with layered structure
- **Docker** for containerization
- **SQL Server** as the database

## Project Structure

```
ComplyTest/
├── ComplyTest.API/          # Web API layer
├── ComplyTest.Application/   # Business logic layer
├── ComplyTest.Domain/        # Domain entities and interfaces
├── ComplyTest.Infrastructure/# Data access and external services
└── ComplyTest.Test/         # Unit tests
```

## Prerequisites

- .NET 9 SDK
- Docker and Docker Compose
- SQL Server (or use Docker)

## Running the Application

### Option 1: Using Docker Compose (Recommended)

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd ComplyTest
   ```

2. **Create environment file (optional)**
   ```bash
   cp env.example .env
   # Edit .env file if you want to customize the database password, leaving as is will still work
   ```

3. **Run with Docker Compose**
   ```bash
   docker-compose up -d
   ```

4. **Wait for initialization**
   The application will automatically:
   - Wait for SQL Server to be ready
   - Apply database migrations
   - Seed initial data
   - Start the API

5. **Access the API**
   - API: http://localhost:5144
   - Swagger UI: http://localhost:5144/swagger

### Option 2: Local Development

1. **Set up the database**

   ```bash
   # Using SQL Server LocalDB
   # Update connection string in appsettings.Development.json
   ```

2. **Run Entity Framework migrations**
The EF Core tools is needed to create a database and run migrations. Runs this command to install the EF core tools `dotnet tool install --global dotnet-ef`. THEN:
   ```bash
   cd ComplyTest.API
   dotnet ef database update
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the API**
   - API: https://localhost:7001
   - Swagger UI: https://localhost:7001/swagger

## Postman Collection

This project includes a Postman collection for easy API testing:

1. **Import the Collection**
   - Open Postman
   - Click "Import" in the top left
   - Select "Upload Files"
   - Choose `ComplyTest.API/Postman/ComplyTest API.postman_collection.json`

2. **Configure Environment**
   - The collection uses a `base_url` variable set to `http://localhost:5144` by default
   - This matches the Docker deployment URL
   - For local development, you may need to change it to `https://localhost:7001`

3. **Testing with Postman**
   After starting the application using either Docker or local development:
   - Run the requests in sequential order (create entities before referencing them)
   - Use the example request bodies provided in each request
   - Check response status codes and data to verify functionality

## API Endpoints

### Employees
- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Delete employee
- `GET /api/employees/department/{departmentId}` - Get employees by department
- `GET /api/employees/project/{projectId}` - Get employees by project

### Departments
- `GET /api/departments` - Get all departments
- `GET /api/departments/{id}` - Get department by ID
- `POST /api/departments` - Create new department
- `PUT /api/departments/{id}` - Update department
- `DELETE /api/departments/{id}` - Delete department
- `GET /api/departments/{id}/total-budget` - Get total project budget

### Projects
- `GET /api/projects` - Get all projects
- `GET /api/projects/{id}` - Get project by ID
- `POST /api/projects` - Create new project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project
- `GET /api/projects/employee/{employeeId}` - Get projects by employee
- `POST /api/projects/{projectId}/assign-employee` - Assign employee to project
- `POST /api/projects/{projectId}/remove-employee` - Remove employee from project

## Database Schema

### Entities
- **Employee**: Id, FirstName, LastName, Email, Salary, DepartmentId
- **Department**: Id, Name, OfficeLocation
- **Project**: Id, Name, Budget, ProjectCode, DepartmentId
- **EmployeeProject**: EmployeeId, ProjectId, Role (Many-to-Many relationship)

## Testing

Run the unit tests:
```bash
dotnet test
```

## Troubleshooting

1. **Check if containers are running:**
   ```bash
   docker-compose ps
   ```

2. **Check container logs:**
   ```bash
   docker-compose logs sqlserver
   docker-compose logs api
   ```

3. **Restart the services:**
   ```bash
   docker-compose down
   docker-compose up -d
   ```

4. **Reset the database:**
   ```bash
   docker-compose down -v  # This removes volumes
   docker-compose up -d
   ```

## Bonus Features Implemented

### Bonus Task 1: Enhanced Project Code Generation
- Project codes are generated using external API + project ID
- Transaction-based approach ensures data consistency
- Automatic rollback on failure

### Bonus Task 2: Docker Deployment
- Complete Docker Compose setup
- SQL Server container with health checks
- API container with proper networking
- Environment variable configuration
- Automatic database initialization and seeding

