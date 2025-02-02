# msihorg.Common

## Architecture Overview
This solution follows Clean Architecture principles and is structured into the following projects:

### Core Layer
- **msihorg.Common.Server.Core**: Contains enterprise business rules and entities
  - Domain entities, value objects, and events
  - Business logic and rules
  - Interface definitions for repositories and services

### Infrastructure Layer
- **msihorg.Common.Server.Infrastructure**: Contains implementations of core interfaces
  - Database configurations and repositories
  - External service implementations
  - Logging, identity, and email services
  - Background job processing

### API Layer
- **msihorg.Common.Server.WebAPI**: Contains API controllers and models
  - REST API endpoints
  - Request/response models
  - API-specific services
  - Middleware and filters

### Shared Layer
- **msihorg.Common.Shared**: Contains shared models and utilities
  - DTOs for client-server communication
  - Shared constants and enums
  - Common interfaces and helpers

### Web Client
- **msihorg.Common.Web**: Contains the Blazor WebAssembly client application
  - Pages and components
  - Client-side services
  - UI models and helpers

## Technology Stack
- .NET 8.0
- ASP.NET Core
- Blazor WebAssembly
- Entity Framework Core
- SQL Server

## Development Setup
1. Install the latest .NET 8.0 SDK
2. Clone the repository
3. Navigate to the solution directory
4. Run dotnet restore
5. Run dotnet build

## Running the Application
1. Set the startup projects to both the WebAPI and Web projects
2. Press F5 to run with debugging, or Ctrl+F5 to run without debugging

## Project Structure
Each project follows a consistent folder structure as outlined in the solution.
