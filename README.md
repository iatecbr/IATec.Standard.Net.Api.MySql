# {API_NAME}

> Robust .NET API development template at IATec with MySQL persistence, promoting standard practices, efficiency and security. Ideal for scalable, high-performance APIs backed by MySQL.

**Current Version:** `2.1.0`

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Technologies and Stack](#technologies-and-stack)
- [Project Structure](#project-structure)
- [Architecture Layers](#architecture-layers)
- [Prerequisites](#prerequisites)
- [How to Run](#how-to-run)
- [Configuration](#configuration)
- [API Documentation (Scalar)](#api-documentation-scalar)
- [Health Checks](#health-checks)
- [API Versioning](#api-versioning)
- [CORS](#cors)
- [Authentication](#authentication)
- [Feature: Assets](#feature-assets)
- [Logging Dispatcher](#logging-dispatcher)
- [External Services (AntiCorruption)](#external-services-anticorruption)
- [Persistence Layer (MySQL)](#persistence-layer-mysql)
- [Domain Layer](#domain-layer)
- [Message Queue Layer](#message-queue-layer)
- [CrossCutting Layer](#crosscutting-layer)
- [Test Projects](#test-projects)
- [Docker](#docker)
- [CI/CD](#cicd)
- [Renaming the API](#renaming-the-api)
- [Template Extension Points](#template-extension-points)
- [Contributing](#contributing)
- [Changelog](#changelog)

---

## About the Project

This repository is a **base template / scaffolding project** for creating new .NET APIs with **MySQL** persistence, following IATec standards. It is a fork of `IATec.Standard.Net.Api` with a fully implemented MySQL persistence layer using **EF Core + Pomelo**, including dual Read/Write DbContext, entity mappings, snake_case naming convention, and an initial EF Core migration.

### What comes pre-configured

- Clean Architecture / Vertical Slices with **MediatR CQRS**.
- **API Versioning** via `Asp.Versioning.Mvc` (query string `api-version`).
- **Scalar** interactive API documentation (replaces Swagger).
- **Health Checks** with version endpoint.
- **CORS** policy (`AllowAnyOrigin`, `AllowAnyMethod`, `AllowAnyHeader`).
- **FluentValidation** pipeline behavior (validators run before handlers).
- **Exception pipeline behavior** via `IATec.Shared.Behaviors`.
- **Logging dispatcher** sending structured logs to IATec Log Service.
- **Typed HttpClient** for external IATec Log Service integration.
- **MySQL** persistence with `Pomelo.EntityFrameworkCore.MySql` — dual `ReadDataContext` and `WriteDataContext` registered.
- **Snake case naming convention** via `EFCore.NamingConventions`.
- **Domain model** — `People` aggregate with `Person` (aggregate root), `Document` (entity), and 5 value objects.
- **EF Core mappings** — `PersonMapping` and `DocumentMapping` via `IEntityTypeConfiguration<T>`.
- **Initial EF Core migration** (`20260121132713_Init`) included.
- Integration with internal `IATec.Shared.*` packages.

### What is NOT implemented (placeholder scaffolding)

- All command/query handlers in `Application/Features/Assets/` throw `NotImplementedException`.
- `MessageQueueDependencyInjectionConfig` is **empty**.
- `docker/Dockerfile` and `docker/Local.Dockerfile` are **empty (0 bytes)**.
- Test projects have **no test framework packages** (xUnit, NUnit, MSTest) and **zero tests**.
- No CI/CD pipelines (no `.github/workflows`).

> **Note:** Whenever creating a new API from this template, read the [Renaming the API](#renaming-the-api) section to adjust names and references.

---

## Technologies and Stack

| Technology | Version |
|------------|---------|
| .NET | 10.0 |
| ASP.NET Core | 10.0.8 |
| Scalar.AspNetCore | 2.14.14 |
| Microsoft.AspNetCore.OpenApi | 10.0.8 |
| API Versioning (Asp.Versioning.Mvc) | 10.0.0 |
| Asp.Versioning.Mvc.ApiExplorer | 10.0.0 |
| AspNetCore.HealthChecks.UI.Client | 9.0.0 |
| MediatR | 14.1.0 |
| FluentValidation | 12.1.1 |
| FluentValidation.DependencyInjectionExtensions | 12.1.1 |
| FluentResults | 4.0.0 |
| Microsoft.EntityFrameworkCore | 9.0.12 |
| Microsoft.EntityFrameworkCore.Relational | 9.0.12 |
| Microsoft.EntityFrameworkCore.Tools | 9.0.12 |
| Pomelo.EntityFrameworkCore.MySql | 9.0.0 |
| EFCore.NamingConventions | 9.0.0 |
| Microsoft.Extensions.Configuration | 10.0.8 |
| Microsoft.Extensions.Configuration.Binder | 10.0.8 |
| Microsoft.Extensions.Http | 10.0.8 |
| Microsoft.Extensions.Options | 10.0.8 |
| IATec.Shared.Api | 1.2.0 |
| IATec.Shared.Application | 2.0.0 |
| IATec.Shared.Domain | 2.0.1 |
| IATec.Shared.HttpClient | 3.0.0 |
| IATec.Shared.Behaviors | 1.3.0 |

> **CSPROJ Settings:** `Nullable=enable`, `ImplicitUsings=enable`, `InvariantGlobalization=false`, `GenerateDocumentationFile=True`.

---

## Project Structure

```
IATec.Standard.Net.Api.MySql/
├── src/
│   ├── Api/
│   │   ├── Configurations/
│   │   │   ├── ApiDependencyInjectionConfig.cs
│   │   │   └── Extensions/
│   │   │       ├── CorsPolicyExtension.cs
│   │   │       ├── HealthCheckExtension.cs
│   │   │       ├── MigrationExtensions.cs
│   │   │       ├── OptionsExtension.cs
│   │   │       ├── ScalarConfiguration.cs
│   │   │       └── VersioningExtension.cs
│   │   ├── Controllers/
│   │   │   └── (empty folder)
│   │   ├── Properties/
│   │   │   └── launchSettings.json
│   │   ├── appsettings.json
│   │   └── Program.cs
│   │
│   ├── Application/
│   │   ├── Configurations/
│   │   │   ├── ApplicationDependencyInjectionConfig.cs
│   │   │   ├── Extensions/
│   │   │   │   ├── MediatorConfig.cs
│   │   │   │   └── ValidatorConfig.cs
│   │   │   └── Factories/
│   │   │       └── ValidatorFactory.cs
│   │   ├── Dispatchers/
│   │   │   └── Logging/
│   │   │       └── LogDispatcher.cs
│   │   └── Features/
│   │       └── Assets/
│   │           ├── Commands/
│   │           │   ├── CreateAssetCommand.cs
│   │           │   └── CreateAssetCommandHandler.cs
│   │           ├── Queries/
│   │           │   ├── CheckIfExistsAssetQuery.cs
│   │           │   └── CheckIfExistsAssetQueryHandler.cs
│   │           └── Validators/
│   │               └── CreateAssetValidator.cs
│   │
│   ├── CrossCutting/
│   │   └── (empty — no .cs files)
│   │
│   ├── Domain/
│   │   └── Models/
│   │       └── People/
│   │           └── PeopleAggregate/
│   │               ├── Entities/
│   │               │   ├── Person.cs
│   │               │   └── Document.cs
│   │               └── ValueObjects/
│   │                   ├── Document/
│   │                   │   ├── IssuerValue.cs
│   │                   │   └── ValueValue.cs
│   │                   └── Person/
│   │                       ├── FirstNameValue.cs
│   │                       ├── LastNameValue.cs
│   │                       └── MiddleNameValue.cs
│   │
│   ├── Persistence/
│   │   ├── Configurations/
│   │   │   ├── PersistenceDependencyInjectionConfig.cs
│   │   │   ├── Extensions/
│   │   │   │   └── DatabaseExtension.cs
│   │   │   └── Options/
│   │   │       ├── EntityFrameworkOption.cs
│   │   │       └── MySqlOption.cs
│   │   ├── Context/
│   │   │   ├── ReadDataContext.cs
│   │   │   └── WriteDataContext.cs
│   │   ├── Mappings/
│   │   │   └── People/
│   │   │       ├── PersonMapping.cs
│   │   │       └── DocumentMapping.cs
│   │   └── Migrations/
│   │       ├── 20260121132713_Init.cs
│   │       ├── 20260121132713_Init.Designer.cs
│   │       └── WriteDataContextModelSnapshot.cs
│   │
│   ├── AntiCorruption/
│   │   ├── Configurations/
│   │   │   ├── AntiCorruptionDependencyInjectionConfig.cs
│   │   │   └── Extensions/
│   │   │       └── LoggingConfig.cs
│   │   └── Services/
│   │       └── Iatec/
│   │           └── LogService.cs
│   │
│   ├── MessageQueue/
│   │   └── Configurations/
│   │       └── MessageQueueDependencyInjectionConfig.cs
│   │
│   ├── Domain.Tests/
│   │   └── Domain.Tests.csproj (empty — no tests, no framework)
│   │
│   └── Application.Tests/
│       └── Application.Tests.csproj (empty — no tests, no framework)
│
├── docker/
│   ├── Dockerfile (empty — 0 bytes)
│   └── Local.Dockerfile (empty — 0 bytes)
│
├── secrets/
│   └── secrets.yml (Kubernetes Secret template)
│
├── IATec.Standard.Net.Api.MySql.sln
├── README.md
└── CHANGELOG.md
```

---

## Architecture Layers

### 1. Api (Presentation Layer)

Entrypoint ASP.NET Core Web API. Orchestrates all configurations and maps controllers.

**Program.cs flow:**
```csharp
builder.Services
    .ConfigureApi(builder.Configuration, builder.Environment)
    .ConfigureApplication()
    .ConfigureAntiCorruption()
    .ConfigureMessageQueue()
    .ConfigurePersistence(builder.Configuration);

app.UseApi()
    .Run();
```

### 2. Application Layer

Contains use cases, MediatR handlers, validators, dispatchers, and factories.

**Mediator Pipeline Behaviors (from `IATec.Shared.Behaviors`):**
1. `ValidatorPipelineBehavior<,>` — runs FluentValidation validators before the handler.
2. `ExceptionPipelineBehavior<,>` — global exception handling wrapper.

### 3. Domain Layer

Contains the **People aggregate** — the only implemented domain model in this template.

| Type | Class | Description |
|------|-------|-------------|
| Aggregate root | `Person` | Extends `EntityUlidInt32`. Has `FirstName`, `MiddleName?`, `LastName`, and a collection of `Document`. |
| Entity | `Document` | Extends `EntityUlidInt32`. Has `PersonId`, `Value`, and `Issuer`. |
| Value object | `FirstNameValue` | `string Value`, min 1 / max 50 chars. |
| Value object | `LastNameValue` | `string Value`, min 1 / max 50 chars. |
| Value object | `MiddleNameValue` | `string Value`, nullable in `Person`. |
| Value object | `IssuerValue` | `string Value`, document issuer identifier. |
| Value object | `ValueValue` | `string Value`, document value/number. |

### 4. Persistence Layer (MySQL)

Fully implemented MySQL persistence layer using EF Core + Pomelo. Registered in `PersistenceDependencyInjectionConfig.cs` via `DatabaseExtension.cs`:

| Context | Registration | Connection | Purpose |
|---------|-------------|------------|---------|
| `ReadDataContext` | `DbContext` | `ServerReader` | Read-only queries with `NoTrackingWithIdentityResolution` |
| `WriteDataContext` | `DbContext` | `ServerWriter` | Write operations with change tracking |

Both contexts apply snake_case naming via `.UseSnakeCaseNamingConvention()` and load entity mappings via `ApplyConfigurationsFromAssembly(...)`.

### 5. AntiCorruption Layer

Adapters for external services. Currently implements:
- **IATec Log Service** (`LogService.cs`) — typed `HttpClient` sending `LogDto` via `POST v1/log`.

### 6. MessageQueue Layer

**Currently empty scaffolding.** `ConfigureMessageQueue()` returns `services` with no registrations.

### 7. CrossCutting Layer

**Currently empty.** Contains only `CrossCutting.csproj` with references to `IATec.Shared.Behaviors` (`1.3.0`) and `MediatR` (`14.1.0`). **Zero `.cs` source files.**

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- MySQL instance running locally or remotely
- Editor of your choice (VS, VS Code, Rider)

---

## How to Run

### 1. Clone the repository

```bash
git clone <repository-url>
cd {API_NAME}
```

### 2. Start MySQL

Ensure a MySQL instance is running. Update `appsettings.json` with the correct `ServerReader`, `ServerWriter`, `User`, `Password`, and `Database`.

### 3. Apply migrations

```bash
dotnet ef database update --project src/Persistence --startup-project src/Api --context WriteDataContext
```

### 4. Restore packages

```bash
dotnet restore
```

### 5. Run the API

```bash
dotnet run --project src/Api/Api.csproj
```

**Default profile (from `launchSettings.json`):**
- URL: `http://localhost:5015`
- Environment: `Local`
- Auto-opens browser at `/documentation`

---

## Configuration

Settings are located in `src/Api/appsettings.json`.

### Current `appsettings.json`

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
  "MySQL": {
    "Database": "dbPeople_local",
    "User": "root",
    "Password": "",
    "ServerReader": "localhost",
    "ServerWriter": "localhost",
    "Port": "3306"
  },
  "EntityFramework": {
    "SensitiveDataLogging": true
  }
}
```

### What to configure when starting a new API

| Section | Description | Example |
|---------|-------------|---------|
| `TimeZone` | Application time zone | `"America/Sao_Paulo"` |
| `Container.Name` | Deployment container metadata name | `"MyApi-Production"` |
| `Container.ContainerId` | Container identifier | `"my-api-container"` |
| `Logging.LogLevel.Default` | ASP.NET Core log level | `"Debug"`, `"Information"`, `"Warning"` |
| `MySQL.Database` | MySQL database name | `"myapp_prod"` |
| `MySQL.User` | MySQL user | `"myuser"` |
| `MySQL.Password` | MySQL password | `"..."` |
| `MySQL.ServerReader` | Read replica host | `"mysql-read.internal"` |
| `MySQL.ServerWriter` | Primary/write host | `"mysql-write.internal"` |
| `MySQL.Port` | MySQL port | `"3306"` |
| `EntityFramework.SensitiveDataLogging` | Enable EF Core sensitive logging | `false` in production |

### Typed Options registered in DI

In `OptionsExtension.cs`:

| Option Class | Configuration Key | Purpose |
|--------------|-------------------|---------|
| `LogServiceOption` | `LogServiceOption` | URL for IATec Log Service |
| `ContainerOption` | `ContainerOption` | Container metadata for logging dispatcher |

In `PersistenceDependencyInjectionConfig.cs`:

| Option Class | Configuration Key | Purpose |
|--------------|-------------------|---------|
| `EntityFrameworkOption` | `EntityFramework` | EF Core settings (`SensitiveDataLogging`) |
| `MySqlOption` | `MySQL` | Host, port, database, credentials. Builds connection string via `GetConnectionString(isReader)` |

> **Tip:** Add new typed options in `src/Api/Configurations/Extensions/OptionsExtension.cs` for injection via `IOptions<T>`.

---

## API Documentation (Scalar)

The project uses **Scalar** (not Swagger) for interactive API documentation.

- **OpenAPI JSON:** `/openapi/v1.json`
- **Scalar UI:** `/documentation`
- **Availability:** Only in non-Production environments (`!app.Environment.IsProduction()`).
- **Theme:** Mars, dark mode, C# HttpClient as default client.

---

## Health Checks

Endpoint: `GET /_healthcheck/status`

- **VersionHealthCheck** — returns `HealthStatus.Healthy` with the assembly version string.
- Response is formatted via `HealthChecks.UI.Client` (rich JSON with UI metadata).

---

## API Versioning

Configured in `VersioningExtension.cs`:

- **Default version:** `1.0`
- **Assume default when unspecified:** `true`
- **Report API versions:** `true`
- **Version reader:** Query string parameter `api-version`
- **Explorer group format:** `'v'VVV` (e.g., `v1`)

---

## CORS

Configured in `CorsPolicyExtension.cs` with `AllowAnyHeader`, `AllowAnyMethod`, `AllowAnyOrigin`.

**Exposed headers:** `X-Custom-Header`, `Location`, `Content-Disposition`, `Content-Length`.

> ⚠️ **Warning:** `AllowAnyOrigin()` is extremely permissive. Review and restrict before deploying to production.

---

## Authentication

**Currently NOT configured.**

JWT Bearer setup is not present. To add JWT authentication:

1. Install `Microsoft.AspNetCore.Authentication.JwtBearer`.
2. Configure token validation in `ApiDependencyInjectionConfig.cs` or a new extension method.
3. Add `app.UseAuthentication()` before `app.UseAuthorization()` in `UseApi()`.
4. Add security schemes to the OpenAPI document in `ScalarConfiguration.cs`.

---

## Feature: Assets

The only implemented feature domain in the Application layer. All handlers are **placeholders** (`NotImplementedException`).

| File | Type | Details |
|------|------|---------|
| `CreateAssetCommand.cs` | `readonly record struct` | `IRequest<Result>` from MediatR + FluentResults |
| `CreateAssetCommandHandler.cs` | Handler | `throw new NotImplementedException()` |
| `CheckIfExistsAssetQuery.cs` | `readonly record struct` | `IRequest<Result<bool>>` |
| `CheckIfExistsAssetQueryHandler.cs` | Handler | `throw new NotImplementedException()` |
| `CreateAssetValidator.cs` | `AbstractValidator<CreateAssetCommand>` | **Empty — no rules defined** |

> Replace this feature with your own domain features when cloning this template.

---

## Logging Dispatcher

### `LogDispatcher.cs`

Implements `ILogDispatcher` (from `IATec.Shared.Domain.Contracts.Dispatcher`).

**Constructs a `LogDto` with:**
- `ContainerKey` — from `ContainerOption.Key`
- `Source`, `Owner`, `Action` — caller-provided
- `Content` — `object?.ToString() ?? string.Empty`
- `Date` — `DateTime.UtcNow`
- `Id` — `string.Empty`
- `UserId` — `string.Empty`

Then delegates to `ILogService.SendAsync()`.

### `ILogService` implementation

**`LogService.cs`** (AntiCorruption layer):
- Typed `HttpClient` with base address from `LogServiceOption.Url`.
- Sends `POST v1/log` as JSON.
- **Does NOT throw on failure** — errors are caught and logged silently.

---

## External Services (AntiCorruption)

### IATec Log Service

| Config | File |
|--------|------|
| DI Registration | `LoggingConfig.cs` |
| Implementation | `LogService.cs` |
| Contract | `ILogService` (from `IATec.Shared.Domain`) |

---

## Persistence Layer (MySQL)

### DbContexts

`DatabaseExtension.cs` registers two EF Core contexts backed by Pomelo:

- **`ReadDataContext`** — configured with `NoTrackingWithIdentityResolution` query tracking behavior. Use for all read queries to avoid unnecessary change tracking overhead.
- **`WriteDataContext`** — configured with default change tracking. Use for all write operations (insert, update, delete).

Both contexts use `.UseSnakeCaseNamingConvention()` (all column/table names in `snake_case`) and `.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))`.

**Example: injecting contexts in a repository:**

```csharp
public class PersonRepository(ReadDataContext read, WriteDataContext write)
{
    public async Task<Person?> GetByIdAsync(int id)
        => await read.Set<Person>().FirstOrDefaultAsync(p => p.Id == id);

    public async Task SaveAsync(Person person)
    {
        await write.Set<Person>().AddAsync(person);
        await write.SaveChangesAsync();
    }
}
```

### Migrations

One migration is included: `20260121132713_Init` — creates the initial `person` and `document` tables with charset `utf8mb4`.

To add a new migration:
```bash
dotnet ef migrations add MigrationName --project src/Persistence --startup-project src/Api --context WriteDataContext
dotnet ef database update --project src/Persistence --startup-project src/Api --context WriteDataContext
```

### Entity Mappings

EF Core mappings are implemented via `IEntityTypeConfiguration<T>` in `src/Persistence/Mappings/`:

| Mapping | Entity | Table |
|---------|--------|-------|
| `PersonMapping` | `Person` | `person` |
| `DocumentMapping` | `Document` | `document` |

`PersonMapping` maps value objects (`FirstName`, `MiddleName`, `LastName`) using `.OwnsOne(...)` and configures column types (`varchar`, `char`) and constraints. `DocumentMapping` maps `Value` and `Issuer` as owned value objects with FK to `person` using `DeleteBehavior.Restrict`.

---

## Domain Layer

Contains the **People aggregate** — the domain model for this template.

### `Person` (Aggregate Root)

Extends `EntityUlidInt32`. Holds `FirstName`, `MiddleName?`, `LastName`, and a private `List<Document>` exposed as `IReadOnlyCollection<Document> Documents`.

### `Document` (Entity)

Extends `EntityUlidInt32`. Has `PersonId`, `Value`, `Issuer`.

### Value Objects

| Class | Property | Constraints |
|-------|----------|-------------|
| `FirstNameValue` | `string Value` | `FieldMinLength = 1`, `FieldMaxLength = 50` |
| `LastNameValue` | `string Value` | `FieldMinLength = 1`, `FieldMaxLength = 50` |
| `MiddleNameValue` | `string Value` | `FieldMinLength = 1`, `FieldMaxLength = 50` |
| `IssuerValue` | `string Value` | Document issuer identifier |
| `ValueValue` | `string Value` | Document value/number |

---

## Message Queue Layer

**Status: Completely empty.** `ConfigureMessageQueue()` returns `services` with no registrations. No message queue library referenced.

---

## CrossCutting Layer

**Status: Completely empty.**

Contains only `CrossCutting.csproj` with references to `IATec.Shared.Behaviors` (`1.3.0`) and `MediatR` (`14.1.0`). **Zero `.cs` source files.**

---

## Test Projects

| Project | Framework Packages | Test Files | Status |
|---------|-------------------|------------|--------|
| `Domain.Tests` | **None** | 0 | Empty |
| `Application.Tests` | **None** | 0 | Empty |

### To add tests

```bash
dotnet add src/Domain.Tests package xunit
dotnet add src/Domain.Tests package Microsoft.NET.Test.Sdk
dotnet add src/Domain.Tests package xunit.runner.visualstudio
```

---

## Docker

### `docker/Dockerfile`
**Empty (0 bytes).**

### `docker/Local.Dockerfile`
**Empty (0 bytes).**

### Basic Dockerfile example

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

## CI/CD

**Status: No CI/CD configured.**

- No `.github/` folder.
- No GitHub Actions workflows.
- No Azure DevOps pipelines.
- No Jenkins/GitLab CI files.

The only file in `secrets/` is `secrets.yml`, a **Kubernetes Secret manifest template** with placeholders (`#{deployment_name}#`, `#{namespace}#`).

---

## Renaming the API

> Follow these steps when using this project as a base for a new API.

### 1. Solution file

```bash
mv IATec.Standard.Net.Api.MySql.sln {API_NAME}.sln
```

### 2. Assembly and namespaces

Update `Api.csproj` if desired:
```xml
<AssemblyName>{API_NAME}.Api</AssemblyName>
<RootNamespace>{API_NAME}.Api</RootNamespace>
```

Run a **Replace All** in `src/` to adjust namespaces:

| From | To (example) |
|------|--------------|
| `namespace Api;` | `namespace MyProject.Api;` |
| `namespace Application;` | `namespace MyProject.Application;` |
| `namespace Persistence;` | `namespace MyProject.Persistence;` |
| `namespace Domain;` | `namespace MyProject.Domain;` |

Or keep simplified namespaces (`Api`, `Application`, `Domain`, etc.).

### 3. Scalar / OpenAPI title

In `src/Api/Configurations/Extensions/ScalarConfiguration.cs`, replace `{API_NAME}` with the actual project name.

### 4. Database and table names

In `appsettings.json`, update `MySQL.Database` with your own database name. Update table names in entity mappings (`PersonMapping.cs`, etc.) accordingly.

### 5. README and CHANGELOG

Replace all `{API_NAME}` placeholders with the actual project name.

### 6. Version

Update `<Version>` in `Api.csproj` to `1.0.0` for the new project.

---

## Template Extension Points

This project is an **intentional scaffold/template**. The following items are **structural placeholders** — not bugs or missing features. Each represents an extension point where a new API project should add its own implementation.

| # | Extension Point | Location | Purpose |
|---|----------------|----------|---------|
| 1 | `NotImplementedException` handlers | `Application/Features/Assets/` | Example command/query structure — replace with real business logic |
| 2 | Empty `MessageQueue` layer | `src/MessageQueue/` | Add producers/consumers (RabbitMQ, Kafka, etc.) |
| 3 | Empty `CrossCutting` layer | `src/CrossCutting/` | Add shared utilities, constants, or cross-cutting concerns |
| 4 | Empty test projects | `*.Tests/` | Add xUnit/NUnit/MSTest and write tests |
| 5 | No CI/CD | Root | Add GitHub Actions / Azure DevOps when ready |
| 6 | Empty Dockerfiles | `docker/` | Add build/publish steps for containerization |
| 7 | Permissive CORS | `CorsPolicyExtension.cs` | Restrict origins/methods when deploying to production |
| 8 | No authentication | `ApiDependencyInjectionConfig.cs` | Add JWT/Auth when security requirements are defined |
| 9 | `{API_NAME}` placeholders | `ScalarConfiguration.cs`, README | Rename when cloning template |
| 10 | No `appsettings.Development.json` | `src/Api/` | Create environment-specific configs as needed |
| 11 | `People` domain model | `src/Domain/Models/People/` | Replace or extend with the actual domain aggregate of the new API |
| 12 | `SensitiveDataLogging: true` | `appsettings.json` | Set to `false` before deploying to production |

---

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository.
2. Create a branch: `git checkout -b feature/feature-name`.
3. Commit with clear messages following [Conventional Commits](https://www.conventionalcommits.org/).
4. Open a Pull Request for review.

---

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for full release history.
