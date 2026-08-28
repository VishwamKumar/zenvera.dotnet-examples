# Shared infrastructure adapters

These class libraries replace private `Zenvera.Shared.*` NuGet packages for the infrastructure examples. They are **example-sized adapters**, not production platform libraries.

Each adapter:

- Lives in this repository and is consumed by `ProjectReference`
- Registers with the same `AddXxxService` / `AddCustomLogging` style the examples already use
- Wraps public packages (Redis, RabbitMQ, Azure Key Vault, Serilog) so restore does not need a private feed
- Resolves `ENV:VARIABLE_NAME` configuration placeholders through `Exp.Shared.Configuration`

Hosts keep Weather DTOs and HTTP/UI behavior local. Open an adapter when you want to see registration, provider selection, and the public-package call; open the matching host when you want to see how that abstraction is used.

| Project | Registration | Abstraction | Public packages | Used by |
|---|---|---|---|---|
| `Exp.Shared.Configuration` | `BindResolved`, `GetFeatureFlags` | `ENV:` placeholder resolver | Configuration abstractions | The four adapters below |
| `Exp.Shared.Caching` | `AddCachingService` | `ICacheService` | `Microsoft.Extensions.Caching.StackExchangeRedis` | [Redis Weather API](../../src/Infrastructure/Caching/Redis/Exp.Weather.RestApi.RedisCaching/README.md) |
| `Exp.Shared.Queuing` | `AddRabbitMqService` | `IRabbitMqService` | `RabbitMQ.Client` | [RabbitMQ Weather API](../../src/Infrastructure/Messaging/RabbitMq/Exp.Weather.RestApi.RabbitMq/README.md) |
| `Exp.Shared.Secrets` | `AddSecretsService` | `ISecretService` | `Azure.Identity`, `Azure.Security.KeyVault.Secrets` | [Key Vault Blazor host](../../src/Infrastructure/Secrets/AzureKeyVault/Exp.Weather.BlazorServer.KeyVault/README.md) |
| `Exp.Shared.Logging` | `AddCustomLogging` | `ILogTypeLogger<T>` | Serilog + Console/File/SQL/MongoDB sinks | [REST Serilog](../../src/Infrastructure/Logging/Serilog/Exp.Weather.RestApi.Serilog/README.md), [Blazor Serilog](../../src/Infrastructure/Logging/Serilog/Exp.Weather.BlazorServer.Serilog/README.md) |

`FeatureFlags` can disable caching (`UseCaching`), Key Vault (`UseKeyVault`), logging (`UseLogger`), and optional PII redaction (`UsePiiRedaction`) so a host can start without the external dependency. RabbitMQ has no disable flag; an unreachable broker fails at publish/consume time.

Do not publish these projects as NuGet packages. Copying them into a production system without a threat model, resilience policy, and operational ownership is not the intended use.
