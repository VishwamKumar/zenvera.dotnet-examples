# Infrastructure and UI migration report

## Scope and mapping

The original repositories remain untouched.

Post-migration source checks showed `exp.infra.patterns` clean. `exp.ui-styles.frameworks` already reports a deleted `src/ToDoApp.MauiMobile.UsingRestApi/nuget.config`; the migration did not write to or repair that source worktree, preserving the existing local state.

| Source | Migrated project | Target |
|---|---|---|
| `WeatherApp.RestApi.UsingCache` | `Exp.Weather.RestApi.RedisCaching` | `src/Infrastructure/Caching/Redis` |
| `WeatherApp.RestApi.UsingRabbitMQ` | `Exp.Weather.RestApi.RabbitMq` | `src/Infrastructure/Messaging/RabbitMq` |
| `WeatherApp.BlazorServer.UsingKeyVault` | `Exp.Weather.BlazorServer.KeyVault` | `src/Infrastructure/Secrets/AzureKeyVault` |
| `WeatherApp.RestApi.UsingSerilog` | `Exp.Weather.RestApi.Serilog` | `src/Infrastructure/Logging/Serilog` |
| `WeatherApp.BlazorServer.UsingSerilog` | `Exp.Weather.BlazorServer.Serilog` | `src/Infrastructure/Logging/Serilog` |
| `WeatherApp.RestApi.UsingBackgroundService` | `Exp.Weather.Worker.BackgroundService` | `src/Infrastructure/BackgroundProcessing/BackgroundService` |
| `ToDoApp.MauiMobile.UsingRestApi` | `Exp.Todo.Maui.RestApiClient` | `src/UserInterface/Maui` |

Project files, assembly names, root namespaces, XAML namespaces, and launch profiles were normalized. Nested solutions, licenses, repository metadata, generated outputs, IDE state, and user/database files were excluded. A filename with a trailing space before `.cs` was corrected. Public HTTP behavior was otherwise preserved.

## Sharing and configuration decisions

No shared Weather project was introduced. The Weather shapes are small but not materially identical across all examples, and local ownership preserves each focused demonstration. Infrastructure implementations remain independent. Committed Azure tenant/client identifiers and literal Redis/RabbitMQ/SQL/Mongo credentials or connection strings were replaced with `ENV:EXP_*` references. Compose defaults are explicitly development-only; `.env.example` contains no live credential.

The local stack includes only services actually referenced: Redis, RabbitMQ, MongoDB, and SQL Server. Azure Key Vault remains external. The MAUI client stays on .NET 8 because projects are not being upgraded during migration.

## Validation

Validation results are recorded after migration commands complete:

- Restore: failed for the complete 44-project solution. RabbitMQ, Key Vault, and Blazor Serilog depend on unavailable `Zenvera.Shared.Queuing`, `Zenvera.Shared.Secrets`, and `Zenvera.Shared.ErrorHandling` packages. SDK 10.0.301 rejects the unchanged .NET 8 MAUI targets as out of support (`NETSDK1202`). Existing Clean Architecture projects also emit pre-existing duplicate-reference warnings.
- Build: background service, Redis caching, and REST Serilog succeeded. RabbitMQ, Key Vault, and Blazor Serilog failed because those packages could not restore. MAUI failed with `NETSDK1202`. Background service initially exposed a namespace/type collision after normalization; its framework base type was explicitly qualified and then built with zero warnings/errors. Redis resolved unavailable `Zenvera.Shared.Caching` 0.0.25 to 1.0.17, and REST Serilog resolved unavailable `Zenvera.Shared.Logging` 1.0.8 to 1.0.17; both substitutions produced `NU1603` warnings.
- Tests: neither migrated source repository contained a test project. A complete solution test attempt did not finish and was stopped; the existing Simple Clean Architecture suite was then run as a test-runner check and passed 27/27 tests with analyzer warnings.
- Docker Compose syntax: `docker compose -f deploy/local/compose.yml config --quiet` succeeded.
- Secret scan: no migrated live Azure GUID, original Redis/RabbitMQ password, or literal SQL/Mongo connection string remained. Configuration uses documented `EXP_*` placeholders.

## Known operational considerations

- Infrastructure examples require their documented services at runtime, except the background service example.
- Key Vault needs a real development Azure resource and local identity configuration.
- SQL logging may require database/table provisioning by the package or operator.
- MAUI builds require installed platform workloads and its REST backend; device/emulator loopback differs from desktop localhost.
