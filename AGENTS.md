# AGENTS.md

Agent instructions for this repository. Every item here is non-obvious or easy to get wrong.

## Runtime & SDK

- Target: `net10.0` (ASP.NET Core 10)
- EF Core is pinned at **9.0.12** (not 10) — Pomelo MySQL driver only supports EF Core 9.x. Do not upgrade EF Core to 10.

## Solution Layout

```
src/
  Api/            # ASP.NET Core entrypoint (Presentation)
  Application/    # MediatR CQRS use cases, FluentValidation, FluentResults
  Domain/         # Domain model — People aggregate, value objects
  Persistence/    # EF Core + Pomelo MySQL, dual DbContext (Read/Write), migrations
  AntiCorruption/ # External service adapters (IATec Log Service typed HttpClient)
  CrossCutting/   # Currently empty — just wires IATec.Shared.Behaviors + MediatR
  MessageQueue/   # Empty stub — ConfigureMessageQueue() does nothing yet
  Domain.Tests/   # Empty scaffold — no test framework installed
  Application.Tests/ # Empty scaffold — no test framework installed
```

## Commands

```bash
# Build
dotnet restore
dotnet build

# Run locally (environment is "Local", not "Development")
dotnet run --project src/Api/Api.csproj

# Add a migration (use WriteDataContext)
dotnet ef migrations add <Name> --project src/Persistence --startup-project src/Api --context WriteDataContext

# Apply migrations manually
dotnet ef database update --project src/Persistence --startup-project src/Api --context WriteDataContext

# Tests (currently no test framework installed — passes with 0 tests)
dotnet test
```

## Key Quirks

- **ASPNETCORE_ENVIRONMENT is `Local`**, not `Development`. `launchSettings.json` sets this. Code that checks `env.IsDevelopment()` will be false locally.
- **Migrations run automatically on startup** via `MigrationExtensions.ApplyMigrations()` which calls `Database.Migrate()` unconditionally. The guard restricting this to Local is commented out — migrations apply in all environments.
- **Two DbContexts**: `WriteDataContext` (writes) and `ReadDataContext` (reads), both using MySQL with separate connection strings (`ServerWriter` vs `ServerReader` in config).
- **EFCore.NamingConventions** is active — all column names are `snake_case` automatically. Do not add manual `[Column("...")]` attributes expecting PascalCase.
- **`appsettings.json` has blank MySQL password** checked in — intended for local dev. Production secrets go through Kubernetes Secret (`secrets/secrets.yml`) with `#{token}#` placeholder substitution in CI.
- **`EntityFramework.SensitiveDataLogging: true`** is checked in — must be set to `false` for production.
- **No `appsettings.Local.json` or `appsettings.Development.json`** — only the base `appsettings.json`. Override via environment variables or user secrets.
- **No authentication/authorization** configured in the middleware pipeline.
- **CORS is fully open** (`AllowAnyOrigin + AllowAnyMethod + AllowAnyHeader`).
- **Scalar** is used instead of Swagger UI. API docs at `/documentation`, OpenAPI JSON at `/openapi/v1.json`.
- `InvariantGlobalization=false` — full ICU/culture data is required.

## Adding Tests

Test projects are empty scaffolds. To add tests you must first install packages:

```bash
dotnet add src/Domain.Tests package xunit
dotnet add src/Domain.Tests package Microsoft.NET.Test.Sdk
dotnet add src/Domain.Tests package xunit.runner.visualstudio
```

Integration tests require a live MySQL instance — no in-memory EF Core provider or test containers are set up.

## Internal NuGet Packages

The following `IATec.Shared.*` packages are used and must be resolvable from the configured NuGet source:

- `IATec.Shared.Api` 1.2.0
- `IATec.Shared.Application` 2.0.0
- `IATec.Shared.Domain` 2.0.1
- `IATec.Shared.HttpClient` 3.0.0
- `IATec.Shared.Behaviors` 1.3.0

If `dotnet restore` fails, verify the private NuGet feed is configured in `NuGet.Config` or the environment.

## Health Check

`/_healthcheck/status` — wired up via `AspNetCore.HealthChecks.UI.Client`.
