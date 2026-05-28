# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.1.0] — 2026-05-28

### ADDED

- `README.md` — full template documentation: architecture, configuration, prerequisites, Docker, CI/CD, renaming guide, and Template Extension Points table.
- `CHANGELOG.md` — full release history.

### UPDATED

- `Api.csproj` version bumped to `2.1.0`.

---

## [2.0.0] — 2026-05-26

### ADDED

- Full MySQL persistence layer implemented via EF Core + `Pomelo.EntityFrameworkCore.MySql` `9.0.0`:
  - `MySqlOption.cs` — typed options binding `MySQL` config section (`Database`, `User`, `Password`, `ServerReader`, `ServerWriter`, `Port`). Builds connection string via `GetConnectionString(isReader)`.
  - `EntityFrameworkOption.cs` — typed options binding `EntityFramework` section (`SensitiveDataLogging`).
  - `DatabaseExtension.cs` — registers `ReadDataContext` (`NoTrackingWithIdentityResolution`) and `WriteDataContext` (change-tracked), both with `UseSnakeCaseNamingConvention()` and `ServerVersion.AutoDetect`.
  - `ReadDataContext.cs` and `WriteDataContext.cs` — dual DbContext pattern via `ApplyConfigurationsFromAssembly(...)`.
  - `PersonMapping.cs` and `DocumentMapping.cs` — `IEntityTypeConfiguration<T>` mappings with owned value objects, `varchar`/`char` column types, and FK `DeleteBehavior.Restrict`.
  - Initial migration `20260121132713_Init` — creates `person` and `document` tables with charset `utf8mb4`.
- Domain layer fully implemented — `People` aggregate:
  - `Person` (aggregate root, extends `EntityUlidInt32`) with `FirstName`, `MiddleName?`, `LastName`, and `IReadOnlyCollection<Document> Documents`.
  - `Document` (entity, extends `EntityUlidInt32`) with `PersonId`, `Value`, `Issuer`.
  - Value objects: `FirstNameValue`, `LastNameValue`, `MiddleNameValue`, `IssuerValue`, `ValueValue`.
- `appsettings.json` updated: replaced `ConnectionStrings: {}` with `MySQL` section (`Database`, `User`, `Password`, `ServerReader`, `ServerWriter`, `Port`) and `EntityFramework.SensitiveDataLogging: true`.
- `ExceptionPipelineBehavior` from `IATec.Shared.Behaviors` registered in MediatR pipeline alongside `ValidatorPipelineBehavior`.
- `Microsoft.Extensions.Options` `10.0.8` added to `Application.csproj`.

### UPDATED

- **BREAKING:** Replaced `Swashbuckle.AspNetCore` with `Scalar.AspNetCore` `2.14.14`. Removed `SwaggerExtension.cs` and all Swagger configuration.
- **BREAKING:** `IATec.Shared.Application` `1.1.0` → `2.0.0`.
- **BREAKING:** `IATec.Shared.Domain` `1.2.0` → `2.0.1`.
- **BREAKING:** `IATec.Shared.Behaviors` `1.2.0` → `1.3.0`.
- **BREAKING:** `IATec.Shared.HttpClient` `2.1.0` → `3.0.0`.
- **BREAKING:** `CreateAssetCommand` and `CheckIfExistsAssetQuery` converted from `sealed class` to `readonly record struct`.
- `IATec.Shared.Api` `1.1.0` → `1.2.0`.
- `Microsoft.AspNetCore.OpenApi` `10.0.1` → `10.0.8`.
- `Asp.Versioning.Mvc` `8.1.1` → `10.0.0`.
- `Asp.Versioning.Mvc.ApiExplorer` `8.1.1` → `10.0.0`.
- `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.1` → `10.0.8`.
- `Microsoft.Extensions.Http` `10.0.1` → `10.0.8`.
- `Microsoft.Extensions.Configuration` `10.0.1` → `10.0.8`.
- `Microsoft.Extensions.Configuration.Binder` `10.0.1` → `10.0.8`.
- `launchUrl` in `launchSettings.json` changed from `swagger` to `documentation`.

### FIXED

- `LogDispatcher.cs`: `Content = content?.ToString()!` replaced with `Content = content?.ToString() ?? string.Empty`.

### REMOVED

- `Swashbuckle.AspNetCore` package and `SwaggerExtension.cs`.

---

## [1.3.0] — 2026-01-27

### ADDED

- MySQL persistence fork scaffold: `PersistenceDependencyInjectionConfig.cs` and empty `DatabaseExtension.cs` prepared for MySQL/EF Core integration.
- `appsettings.json` with `ConnectionStrings: {}` placeholder ready for MySQL connection string.

---

## [1.2.0] — 2025-04-29

### UPDATED

- `ValidatorPipelineBehavior<TRequest, TResponse>` constraint changed from `where TResponse : Result` to `where TResponse : ResultBase, new()` — enables generic return values.

---

## [1.1.0] — 2024-09-23

### UPDATED

- `Microsoft.AspNetCore.OpenApi` `8.0.6` → `8.0.8`.
- `Swashbuckle.AspNetCore` `6.6.2` → `6.8.0`.
- `FluentResults` `3.15.2` → `3.16.0`.
- `FluentValidation` `11.9.1` → `11.10.0`.
- `FluentValidation.DependencyInjectionExtensions` `11.9.1` → `11.10.0`.
- `MediatR` `12.2.0` → `12.4.1`.

---

## [1.0.0] — 2024-05-28

### ADDED

- Initial project structure forked from `IATec.Standard.Net.Api`:
  - **Api** — ASP.NET Core entrypoint with `Program.cs`, `ApiDependencyInjectionConfig.cs`, configuration extensions, and `launchSettings.json`.
  - **Application** — `ApplicationDependencyInjectionConfig.cs`, `MediatorConfig`, `ValidatorConfig`, `ValidatorFactory`, `LogDispatcher`, Assets feature stubs.
  - **CrossCutting** — `ValidatorPipelineBehavior` (local).
  - **Domain** — Contracts, seedwork entities, shared options, error/success result types.
  - **Persistence** — `PersistenceDependencyInjectionConfig.cs`, empty `DatabaseExtension` (prepared for MySQL).
  - **AntiCorruption** — `LoggingConfig`, `LogService`.
  - **MessageQueue** — `MessageQueueDependencyInjectionConfig.cs` (stub).
  - **Domain.Tests** and **Application.Tests** — empty test projects.
- `launchSettings.json` — port `5015`, environment `Local`.
- `appsettings.json` — `TimeZone`, `Container`, `Logging`, `ConnectionStrings: {}`.
- `docker/Dockerfile` and `docker/Local.Dockerfile` (empty).
- `secrets/secrets.yml` — Kubernetes Secret manifest template.
