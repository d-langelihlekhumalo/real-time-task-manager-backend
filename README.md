# Real-Time Task Manager API

A modern, production-ready ASP.NET Core 8.0 Web API for managing tasks and notes with real-time updates using SignalR. This application demonstrates best practices in building scalable, maintainable web APIs with comprehensive logging, health checks, and real-time communication capabilities.

## 🚀 Features

- **RESTful API**: Complete CRUD operations for tasks and notes
- **Real-Time Updates**: SignalR integration for instant client notifications
- **Activity Tracking**: Comprehensive activity logging and history
- **Dashboard Analytics**: Statistics and insights about tasks and notes
- **Entity Framework Core**: Code-first database approach with PostgreSQL
- **Health Checks**: Database and application health monitoring
- **Structured Logging**: Serilog with file and console logging
- **Auto-Mapping**: AutoMapper for DTO transformations
- **Swagger/OpenAPI**: Interactive API documentation
- **CORS Support**: Configured for frontend integration
- **Global Exception Handling**: Centralized error management
- **Security Headers**: HSTS, X-Frame-Options, and more
- **Connection Pooling**: Optimized database performance
- **Automatic Data Seeding**: Demo data for quick testing

## 📋 Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [SignalR Integration](#signalr-integration)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Development](#development)
- [Testing](#testing)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [License](#license)

## 🔧 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [PostgreSQL 12+](https://www.postgresql.org/download/) (or use managed PostgreSQL service)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- Optional: [Postman](https://www.postman.com/) or similar API testing tool

## 💿 Installation

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd RealTimeTaskManager
   ```

2. **Restore NuGet packages**

   ```bash
   dotnet restore
   ```

3. **Configure the database connection**

   Update the connection string in `appsettings.json` or set the environment variable:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=RealTimeTaskManager;Username=postgres;Password=postgres"
   }
   ```

4. **Build the project**
   ```bash
   dotnet build
   ```

## ⚙️ Configuration

### Database Connection

The application supports two ways to configure the database connection:

1. **appsettings.json** (default)

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=RealTimeTaskManager;Username=postgres;Password=postgres"
   }
   ```

2. **Environment Variable** (takes precedence)
   ```bash
   set ConnectionStrings__DefaultConnection=Host=your-host;Port=5432;Database=RealTimeTaskManager;Username=postgres;Password=your-password
   ```

### CORS Configuration

Configure allowed origins in `appsettings.json`:

```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:3000",
    "http://localhost:3001",
    "http://localhost:4200",
    "http://localhost:5173"
  ],
  "AllowCredentials": true
}
```

### Health Checks

Enable/disable health checks:

```json
"HealthChecks": {
  "Enabled": true,
  "DetailedErrors": true
}
```

### Swagger UI

Control Swagger documentation:

```json
"SwaggerUI": {
  "Enabled": true
}
```

### Logging

Serilog configuration in `appsettings.json`:

```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft": "Warning",
      "System": "Warning"
    }
  }
}
```

Logs are stored in the `/logs` directory with daily rolling files: `log-YYYYMMDD.txt`

## 🏃 Running the Application

### Using .NET CLI

```bash
dotnet run
```

### Using Visual Studio

- Press `F5` to run with debugging
- Press `Ctrl+F5` to run without debugging

### Application URLs

- **HTTPS**: `https://localhost:7075`
- **HTTP**: `http://localhost:5090`
- **Swagger UI**: `https://localhost:7075/swagger`
- **Health Check**: `https://localhost:7075/health`
- **SignalR Hub**: `https://localhost:7075/taskManagerHub`

The application will automatically:

- Create the database if it doesn't exist
- Apply any pending migrations
- Seed demo data for testing

## 📡 API Endpoints

### Task Management

| Method | Endpoint                           | Description              |
| ------ | ---------------------------------- | ------------------------ |
| GET    | `/api/Task`                        | Get all tasks with notes |
| GET    | `/api/Task/{id}`                   | Get task by ID           |
| POST   | `/api/Task`                        | Create new task          |
| PUT    | `/api/Task/{id}`                   | Update task              |
| DELETE | `/api/Task/{id}`                   | Delete task              |
| PATCH  | `/api/Task/{id}/toggle-completion` | Toggle task completion   |

### Note Management

| Method | Endpoint                  | Description              |
| ------ | ------------------------- | ------------------------ |
| GET    | `/api/Note/task/{taskId}` | Get all notes for a task |
| GET    | `/api/Note/{id}`          | Get note by ID           |
| POST   | `/api/Note`               | Create new note          |
| PUT    | `/api/Note/{id}`          | Update note              |
| DELETE | `/api/Note/{id}`          | Delete note              |

### Dashboard

| Method | Endpoint                 | Description                     |
| ------ | ------------------------ | ------------------------------- |
| GET    | `/api/Dashboard`         | Get dashboard statistics        |
| GET    | `/api/Dashboard/{count}` | Get recent activities (max 100) |

### Example Requests

#### Create a Task

```bash
POST /api/Task
Content-Type: application/json

{
  "title": "Complete project documentation",
  "description": "Write comprehensive README and API documentation"
}
```

#### Create a Note

```bash
POST /api/Note
Content-Type: application/json

{
  "taskId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "content": "Remember to include code examples"
}
```

For detailed API documentation with request/response examples, see [API-Documentation.md](API-Documentation.md) or visit the Swagger UI at `/swagger`.

## 🔌 SignalR Integration

### Connecting to the Hub

**JavaScript/TypeScript Example:**

```javascript
import * as signalR from '@microsoft/signalr'

const connection = new signalR.HubConnectionBuilder()
	.withUrl('https://localhost:7075/taskManagerHub')
	.withAutomaticReconnect()
	.configureLogging(signalR.LogLevel.Information)
	.build()

await connection.start()
console.log('SignalR Connected!')
```

### Real-Time Events

The hub broadcasts the following events to all connected clients:

#### Task Events

- `TaskCreated` - Fired when a new task is created
- `TaskUpdated` - Fired when a task is updated
- `TaskDeleted` - Fired when a task is deleted
- `TaskCompletionChanged` - Fired when task completion status changes

#### Note Events

- `NoteAdded` - Fired when a note is added to a task
- `NoteUpdated` - Fired when a note is updated
- `NoteDeleted` - Fired when a note is deleted

#### Activity Events

- `ActivityUpdate` - Fired when any activity occurs

### Event Listeners Example

```javascript
// Listen for task creation
connection.on('TaskCreated', (message) => {
	console.log('New task created:', message)
	// Update UI with new task
})

// Listen for task updates
connection.on('TaskUpdated', (message) => {
	console.log('Task updated:', message)
	// Update task in UI
})

// Listen for task completion changes
connection.on('TaskCompletionChanged', (message) => {
	console.log('Task completion changed:', message)
	// Update task completion status in UI
})

// Listen for note additions
connection.on('NoteAdded', (message) => {
	console.log('Note added:', message)
	// Add note to task in UI
})

// Listen for activity updates
connection.on('ActivityUpdate', (activity) => {
	console.log('New activity:', activity)
	// Add activity to feed
})
```

## 📁 Project Structure

```
RealTimeTaskManager/
├── AutoMapper/              # AutoMapper profiles for DTO mapping
│   └── MappingProfile.cs
├── Configuration/           # Configuration models
│   └── AppConfiguration.cs
├── Controllers/             # API Controllers
│   ├── DashboardController.cs
│   ├── NoteController.cs
│   └── TaskController.cs
├── Data/                    # Database context and factories
│   ├── ApplicationDbContext.cs
│   ├── DesignTimeDBContextFactory.cs
│   ├── IDBContextFactory.cs
│   └── SqlDBContextFactory.cs
├── DTOs/                    # Data Transfer Objects
│   ├── CreateNoteDto.cs
│   ├── CreateTaskDto.cs
│   ├── NoteDto.cs
│   ├── TaskDto.cs
│   ├── UpdateNoteDto.cs
│   └── UpdateTaskDto.cs
├── Entities/                # Database entities
│   ├── ActivityEntity.cs
│   ├── NoteEntity.cs
│   └── TaskEntity.cs
├── Enums/                   # Enumeration types
│   ├── ActivityActionEnum.cs
│   └── EntityTypeEnum.cs
├── Extensions/              # Extension methods
│   └── HttpResponseExtensions.cs
├── Hubs/                    # SignalR hubs
│   └── TaskManagerHub.cs
├── Middleware/              # Custom middleware
│   └── GlobalExceptionHandlingMiddleware.cs
├── Models/                  # Request/Response models
│   ├── ActivityResponse.cs
│   ├── APIResponse.cs
│   ├── CreateNoteRequest.cs
│   ├── CreateTaskRequest.cs
│   ├── DashboardResponse.cs
│   ├── NoteAddedMessage.cs
│   ├── NoteDeletedMessage.cs
│   ├── NoteResponse.cs
│   ├── NoteUpdatedMessage.cs
│   ├── TaskCompletionChangedMessage.cs
│   ├── TaskCreatedMessage.cs
│   ├── TaskDeletedMessage.cs
│   ├── TaskResponse.cs
│   ├── TaskUpdatedMessage.cs
│   ├── UpdateNoteRequest.cs
│   └── UpdateTaskRequest.cs
├── Services/                # Business logic services
│   ├── ActivityService.cs
│   ├── DataSeedingService.cs
│   ├── IActivityService.cs
│   ├── IDataSeedingService.cs
│   ├── INoteService.cs
│   ├── ITaskManagerHubService.cs
│   ├── ITaskService.cs
│   ├── NoteService.cs
│   ├── TaskManagerHubService.cs
│   └── TaskService.cs
├── logs/                    # Application logs (auto-generated)
├── appsettings.json         # Application configuration
├── appsettings.Development.json
├── appsettings.Production.json
├── appsettings.Staging.json
├── global.json              # .NET SDK version
├── Program.cs               # Application entry point
└── RealTimeTaskManager.csproj
```

## 🛠️ Technologies Used

### Core Framework

- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core 9.0** - ORM for database access
- **SignalR** - Real-time web functionality

### Database

- **PostgreSQL** - Primary database
- **Npgsql.EntityFrameworkCore.PostgreSQL** - PostgreSQL provider for EF Core

### Mapping & Validation

- **AutoMapper 15.0** - Object-to-object mapping

### Logging

- **Serilog** - Structured logging
  - Serilog.AspNetCore
  - Serilog.Sinks.Console
  - Serilog.Sinks.File

### API Documentation

- **Swashbuckle.AspNetCore 9.0** - Swagger/OpenAPI implementation

### Monitoring

- **AspNetCore.HealthChecks.Npgsql** - PostgreSQL health checks
- **Microsoft.Extensions.Diagnostics.HealthChecks** - Health check framework

### Dependency Injection

- **NetCore.AutoRegisterDi** - Automatic service registration

### Development Tools

- **Microsoft.VisualStudio.Web.CodeGeneration.Design** - Scaffolding
- **Microsoft.EntityFrameworkCore.Tools** - EF Core tools for migrations

## 👨‍💻 Development

### Adding a New Feature

1. **Create Entity** (if needed) in `/Entities`
2. **Create DTOs** in `/DTOs` and **Models** in `/Models`
3. **Update DbContext** in `/Data/ApplicationDbContext.cs`
4. **Create Service Interface** in `/Services/I[Feature]Service.cs`
5. **Implement Service** in `/Services/[Feature]Service.cs`
6. **Create Controller** in `/Controllers/[Feature]Controller.cs`
7. **Update AutoMapper Profile** in `/AutoMapper/MappingProfile.cs`
8. **Add SignalR Events** (if needed) in `/Hubs/TaskManagerHub.cs`

### Database Migrations

Since the application uses `EnsureCreated()` for development, migrations are not currently in use. For production, consider switching to migrations:

```bash
# Create a migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### Code Style

- Follow C# naming conventions
- Use dependency injection for services
- Keep controllers thin, business logic in services
- Use async/await for all I/O operations
- Document public APIs with XML comments

### Logging Best Practices

```csharp
// Use structured logging
_logger.LogInformation("Task created: {TaskTitle} with ID {TaskId}", task.Title, task.Id);

// Log exceptions with context
_logger.LogError(ex, "Failed to create task: {TaskTitle}", taskTitle);
```

## 🧪 Testing

### Manual Testing with Swagger

1. Run the application
2. Navigate to `https://localhost:7075/swagger`
3. Expand an endpoint and click "Try it out"
4. Fill in the parameters and click "Execute"

### Testing SignalR

Use the demo HTML client or create your own:

```html
<!DOCTYPE html>
<html>
	<head>
		<title>SignalR Test</title>
		<script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr@latest/dist/browser/signalr.min.js"></script>
	</head>
	<body>
		<div id="messages"></div>
		<script>
			const connection = new signalR.HubConnectionBuilder()
				.withUrl('https://localhost:7075/taskManagerHub')
				.build()

			connection.on('TaskCreated', (message) => {
				const div = document.getElementById('messages')
				div.innerHTML += `<p>Task Created: ${message.title}</p>`
			})

			connection
				.start()
				.then(() => console.log('Connected!'))
				.catch((err) => console.error(err))
		</script>
	</body>
</html>
```

### Health Check

Test the health endpoint:

```bash
curl https://localhost:7075/health
```

Expected response:

```json
{
	"status": "Healthy",
	"checks": [
		{
			"name": "sqlserver",
			"status": "Healthy",
			"duration": "00:00:00.0234567"
		}
	]
}
```

## 🚀 Deployment

### Prerequisites for Production

1. Set up PostgreSQL database (managed service recommended)
2. Update `appsettings.Production.json` with production settings
3. Set environment-specific connection strings
4. Disable Swagger in production (set `SwaggerUI.Enabled` to `false`)
5. Configure proper CORS origins
6. Set up SSL certificates
7. Configure production logging

### Publish the Application

```bash
dotnet publish -c Release -o ./publish
```

### Environment Variables for Production

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Host=your-postgres-host;Port=5432;Database=RealTimeTaskManager;Username=postgres;Password=your-password
```

### Deployment Options

- **Coolify**: Self-hosted PaaS with Docker support (see [DEPLOYMENT.md](DEPLOYMENT.md))
- **Docker**: Containerized deployment using included Dockerfile
- **Azure App Service**: Deploy directly from Visual Studio or Azure DevOps
- **Kubernetes**: Use container orchestration for scalability
- **Any Docker-compatible platform**: Railway, Render, DigitalOcean App Platform, etc.

### Docker Deployment (Example)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RealTimeTaskManager.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealTimeTaskManager.dll"]
```

## 🔒 Security Considerations

- **HTTPS**: Always use HTTPS in production
- **CORS**: Restrict to specific origins in production
- **Authentication**: Consider adding JWT or OAuth2 authentication
- **Authorization**: Implement role-based access control if needed
- **Input Validation**: All inputs are validated through DTOs
- **SQL Injection**: Protected by Entity Framework parameterization
- **Security Headers**: HSTS, X-Frame-Options, X-Content-Type-Options configured

## 📝 API Response Format

All API responses follow a consistent format:

**Success Response:**

```json
{
	"success": true,
	"data": {
		/* response data */
	},
	"message": "Operation successful"
}
```

**Error Response:**

```json
{
	"success": false,
	"message": "Error description",
	"errors": ["Detailed error 1", "Detailed error 2"]
}
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 📧 Contact

For questions or support, please open an issue in the repository.

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- SignalR team for real-time communication capabilities
- Entity Framework Core team for the robust ORM
- All contributors and the open-source community

## 📚 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core)
- [SignalR Documentation](https://docs.microsoft.com/aspnet/core/signalr)
- [API Documentation](API-Documentation.md) - Detailed API reference
- [Serilog Documentation](https://serilog.net/)

---

**Built with ❤️ using ASP.NET Core 8.0**
