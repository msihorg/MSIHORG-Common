# Developer Guide

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or later (recommended) or VS Code
- SQL Server (Local DB or higher)
- Git

### First-Time Setup
1. Clone the repository:
   \\\ash
   git clone [repository-url]
   cd [repository-name]
   \\\

2. Restore dependencies:
   \\\ash
   dotnet restore
   \\\

3. Build the solution:
   \\\ash
   dotnet build
   \\\

### Development Workflow
1. Create a new feature branch from main:
   \\\ash
   git checkout -b feature/your-feature-name
   \\\

2. Make your changes and commit them:
   \\\ash
   git add .
   git commit -m "feat: your meaningful commit message"
   \\\

3. Push your changes and create a pull request:
   \\\ash
   git push origin feature/your-feature-name
   \\\

### Coding Standards
- Follow Microsoft's C# Coding Conventions
- Use meaningful names for variables, methods, and classes
- Write unit tests for business logic
- Document public APIs and complex logic
- Keep methods small and focused

### Testing
- Write unit tests using xUnit
- Place tests in corresponding test projects
- Follow the Arrange-Act-Assert pattern
- Mock external dependencies

### Debugging Tips
- Use the built-in debugger in Visual Studio/VS Code
- Enable diagnostic logging in development
- Use the Browser Dev Tools for client-side debugging

### Common Issues and Solutions
1. Build Errors
   - Clean solution and rebuild
   - Delete bin and obj folders
   - Restore NuGet packages

2. Runtime Errors
   - Check application logs
   - Verify configuration settings
   - Check database connectivity

### Useful Commands
\\\ash
# Run all tests
dotnet test

# Run specific project tests
dotnet test [ProjectName.Tests]

# Watch for changes
dotnet watch run

# Clean solution
dotnet clean
\\\
