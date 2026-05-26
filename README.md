# 🚀 {API_NAME}

> Robust .NET API development template at IATec, promoting standard practices, efficiency and security. Ideal for scalable, high-performance APIs.

---

## 📋 Index

- [About the Project](#about-the-project)
- [Technologies and Stack](#technologies-and-stack)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [How to Run](#how-to-run)
- [Configuration](#configuration)
- [Health Checks](#health-checks)
- [API Documentation (Scalar)](#api-documentation-scalar)
- [Authentication](#authentication)
- [Tests](#tests)
- [Renaming the API](#renaming-the-api)
- [Docker](#docker)
- [Contributing](#contributing)

---

## About the Project

This repository is a **base template** for creating new .NET APIs following IATec standards. It comes pre-configured with:

- Decoupled layered architecture (Domain, Application, Persistence, AntiCorruption, MessageQueue, CrossCutting, Api).
- API versioning.
- Automatic documentation via **Scalar/OpenAPI**.
- Health Checks with JSON response.
- CORS configuration.
- Integration with shared libraries (`IATec.Shared.*`).
- Validation and fluent results (`FluentValidation`, `FluentResults`).
- MediatR for inter-layer communication.

> **Note:** Whenever creating a new API from this template, read the [Renaming the API](#renaming-the-api) section to adjust names and references.

---

## Technologies and Stack

| Technology | Version |
|------------|--------|
| .NET | 10.0 |
| ASP.NET Core | 10.0.x |
| Scalar.AspNetCore | 2.14.14 |
| Microsoft.AspNetCore.OpenApi | 10.0.8 |
| API Versioning (Asp.Versioning.Mvc) | 10.0.0 |
| HealthChecks UI Client | 9.0.0 |
| MediatR | 14.1.0 |
| FluentValidation | 12.1.1 |
| FluentResults | 4.0.0 |
| IATec.Shared.* | As per `csproj` files |

---

## Architecture

The project follows a layered organization inside the `src/` folder:

```
src/
├── Api/                    → ASP.NET Core entrypoint (Controllers, Startup, Configs)
├── Application/            → Use cases, handlers, application logic
├── CrossCutting/           → Shared behaviors, MediatR pipelines
├── Domain/                 → Entities, contracts, validations, pure business rules
├── Persistence/            → Data access, EF Core (configurable), repositories
├── AntiCorruption/         → Adapters for external services (HTTP Clients)
├── MessageQueue/           → Message producers/consumers (ready for expansion)
├── Domain.Tests/           → Domain layer unit tests
└── Application.Tests/      → Application layer unit tests
```

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (or compatible higher version)
- (Optional) Docker for building/publishing images
- Editor of your choice (VS, VS Code, Rider)

---

## How to Run

### 1. Clone the repository

```bash
git clone <repository-url>
cd {API_NAME}
```

### 2. Restore packages

```bash
dotnet restore
```

### 3. Run the API

```bash
dotnet run --project src/Api/Api.csproj
```

By default, the application will be available at:
- `http://localhost:5015`

> The exact URL may vary depending on the active launch profile. Check `src/Api/Properties/launchSettings.json` or the console output when starting the project.

---

## Configuration

Settings are located in `src/Api/appsettings.json` (and its environment overrides, such as `appsettings.Development.json`).

**Current structure example:**

```json
{
  "TimeZone": "UTC",
  "Container": {
    "Name": "Vertical-ContextContainerType",
    "ContainerId": "ContainerId"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "ConnectionStrings": {}
}
```

### What to configure when starting a new API

| Section | Description | Example |
|---------|-----------|---------|
| `TimeZone` | Application time zone | `"America/Sao_Paulo"` |
| `Container` | Deployment/container metadata | Adjust `Name` and `ContainerId` according to your environment |
| `Logging` | ASP.NET Core log level | `"Debug"`, `"Information"`, `"Warning"` |
| `ConnectionStrings` | Database and service connection strings | `"DefaultConnection": "Server=..."` |

> **Tip:** Add new configuration sections in `src/Api/Configurations/Extensions/OptionsExtension.cs` for typed injection via `IOptions<T>`.

---

## Health Checks

The project has a health check endpoint that returns API version information:

```
GET /_healthcheck/status
```

Features:
- Returns `Healthy`/`Unhealthy` status.
- Includes the API assembly version in the response body.
- Response in `HealthChecks.UI.Client` visual format.

---

## API Documentation (Scalar)

Interactive documentation powered by **Scalar** and native ASP.NET Core **OpenAPI** is available in **non-Production** environments:

- OpenAPI JSON: `/openapi/v1.json`
- Scalar UI: `/documentation`

### Configured features

- Automatically generated from native `Microsoft.AspNetCore.OpenApi`.
- **Mars theme** with forced dark mode.
- Tags expanded and sorted alphabetically.
- Default HTTP client configured as **C# HttpClient**.
- Title includes environment name (e.g. `{API_NAME} - Development`).

---

## Authentication

Scalar UI already supports sending JWT tokens in the `Authorization` header with the `Bearer` scheme.

To enable actual token validation in the API:
1. Add the desired authentication package (e.g. `Microsoft.AspNetCore.Authentication.JwtBearer`).
2. Configure token validation in `ApiDependencyInjectionConfig.cs` or in a new extension method.
3. Insert `app.UseAuthentication()` before `app.UseAuthorization()` in `UseApi()`.

---

## Tests

The template includes two test projects:

| Project | Layer Tested | Framework |
|---------|--------------|-----------|
| `Domain.Tests` | Domain | xUnit / MSTest (to be configured as needed) |
| `Application.Tests` | Application | xUnit / MSTest (to be configured as needed) |

To run all tests:

```bash
dotnet test
```

---

## Renaming the API

> Whenever using this project as a base for a new API, follow the steps below to adjust names and references. The text `{API_NAME}` used throughout this README acts as a placeholder for the **actual project name** you want to use.

### Step-by-step guide

#### 1. Clone the repository and enter the folder

```bash
git clone <repository-url>
cd {API_NAME}
```

#### 2. Rename the Solution file

```bash
mv IATec.Standard.Net.Api.sln {API_NAME}.sln
```

#### 3. Rename `AssemblyName` and `RootNamespace` in `.csproj` files

Open `src/Api/Api.csproj` and other `.csproj` files and change:

```xml
<PropertyGroup>
    <AssemblyName>{API_NAME}</AssemblyName>
    <RootNamespace>{API_NAME}</RootNamespace>
</PropertyGroup>
```

> By default these fields are optional and inherit the file name. Explicitly defining them prevents assembly name mismatches after renaming.

#### 4. Adjust C# namespaces

Run a **Replace All** in the `src/` folder for each project layer. Example:

| From | To (example) |
|------|--------------|
| `namespace Api;` | `namespace ProjectName.Api;` |
| `namespace Application;` | `namespace ProjectName.Application;` |
| `namespace Domain;` | `namespace ProjectName.Domain;` |

Or keep simplified namespaces (`Api`, `Domain`, `Application`, etc.) — this is a team preference.

#### 5. Update Scalar title

Open `src/Api/Configurations/Extensions/ScalarConfiguration.cs` and change:

```csharp
document.Info = new()
{
    Title = "{API_NAME}",
```

And also:

```csharp
.WithTitle($"{{API_NAME}} - {environment.EnvironmentName}")
```

#### 6. Update README

Replace **all** occurrences of `{API_NAME}` in this `README.md` with the actual project name.

You can use your editor's `Find & Replace` (usually `Ctrl+Shift+H`) with the following text:

- **Find:** `{API_NAME}`
- **Replace:** `MyNewProjectName`

#### 7. Review and commit

After all changes, run a full build to ensure everything compiles:

```bash
dotnet build
```

Then commit your changes:

```bash
git add .
git commit -m "refactor: rename template API to {API_NAME}"
```

---

### Quick Checklist

Use this checklist to ensure you didn't miss any step:

- [ ] Repository cloned and folder renamed to new API name
- [ ] `.sln` file renamed to `{API_NAME}.sln`
- [ ] `AssemblyName` updated in all `.csproj` files
- [ ] `RootNamespace` updated in all `.csproj` files
- [ ] Namespaces adjusted in source code (`src/`)
- [ ] Scalar `Title` updated in `ScalarConfiguration.cs`
- [ ] All `{API_NAME}` placeholders replaced in `README.md`
- [ ] `dotnet build` runs successfully with zero errors
- [ ] Changes committed to version control

---

## Docker

There is a `docker/Dockerfile` prepared for building the application.

> **Attention:** The Dockerfile is currently empty. When creating a new API from this template, fill it according to your build pipeline. Basic example:

```dockerfile
# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Api/Api.csproj"
RUN dotnet build "src/Api/Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Api/Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
```

---

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository.
2. Create a branch for your feature or fix: `git checkout -b feature/feature-name`.
3. Commit your changes with clear messages.
4. Open a Pull Request for review.

---

> **Note:** This is a base template. Feel free to add/remove layers, packages and configurations according to your business domain needs.
