# Infrastructure and UI migration report

> Historical note: framework and validation status in this migration record is superseded by the [.NET 10 and package upgrade report](dotnet-10-package-upgrade-report.md).

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

The local stack includes only services actually referenced: Redis, RabbitMQ, MongoDB, and SQL Server. Azure Key Vault remains external. The MAUI client was subsequently upgraded to .NET 10 and still requires platform-specific workloads.

## Validation

Validation results are recorded after migration commands complete:

- The initial restore failed because private `Zenvera.Shared.*` packages were unauthenticated and MAUI had not yet been upgraded. These conditions were subsequently addressed by authenticated GitHub Packages configuration and .NET 10 retargeting; multi-platform MAUI CI remains host-dependent.
- Initial build validation succeeded for the background service, Redis caching, and REST Serilog, while unauthenticated private packages and the former MAUI targets blocked the remaining examples. The background-service namespace/type collision was repaired. Subsequent GitHub Packages authentication and .NET 10 retargeting supersede those initial restore failures; current status is tracked in the CI runbook.
- Tests: neither migrated source repository contained a test project. A complete solution test attempt did not finish and was stopped; the existing Simple Clean Architecture suite was then run as a test-runner check and passed 27/27 tests with analyzer warnings.
- Docker Compose syntax: `docker compose -f deploy/local/compose.yml config --quiet` succeeded.
- Secret scan: no migrated live Azure GUID, original Redis/RabbitMQ password, or literal SQL/Mongo connection string remained. Configuration uses documented `EXP_*` placeholders.

## Known operational considerations

- Infrastructure examples require their documented services at runtime, except the background service example.
- Key Vault needs a real development Azure resource and local identity configuration.
- SQL logging may require database/table provisioning by the package or operator.
- MAUI builds require installed platform workloads and its REST backend; device/emulator loopback differs from desktop localhost.
