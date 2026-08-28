# Shared infrastructure adapters

These class libraries replace private `Zenvera.Shared.*` NuGet packages for the infrastructure examples. They are **example-sized adapters**, not production platform libraries.

Each adapter:

- Lives in this repository and is consumed by `ProjectReference`
- Registers with the same `AddXxxService` / `AddCustomLogging` style the examples already use
- Wraps public packages (Redis, RabbitMQ, Azure Key Vault, Serilog) so restore does not need a private feed
- Resolves `ENV:VARIABLE_NAME` configuration placeholders through `Exp.Shared.Configuration`

| Project | Public packages | Used by |
|---|---|---|
| `Exp.Shared.Configuration` | Configuration abstractions | The four adapters below |
| `Exp.Shared.Caching` | `Microsoft.Extensions.Caching.StackExchangeRedis` | Redis Weather API |
| `Exp.Shared.Queuing` | `RabbitMQ.Client` | RabbitMQ Weather API |
| `Exp.Shared.Secrets` | `Azure.Identity`, `Azure.Security.KeyVault.Secrets` | Key Vault Blazor host |
| `Exp.Shared.Logging` | Serilog + Console/File/SQL/MongoDB sinks | REST and Blazor Serilog hosts |

Do not publish these projects as NuGet packages. Copying them into a production system without a threat model, resilience policy, and operational ownership is not the intended use.
