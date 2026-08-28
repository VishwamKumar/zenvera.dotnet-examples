# Infrastructure pattern examples

These are focused technology examples. They deliberately keep infrastructure code beside a small host so the integration point remains visible.

| Capability | Project | External requirement | Demonstrates | Production gaps |
|---|---|---|---|---|
| Redis caching | `Exp.Weather.RestApi.RedisCaching` | Redis 7; `EXP_REDIS_CONNECTION_STRING` and `EXP_REDIS_PASSWORD` | Cache registration, read-through Weather response caching | Key rotation, cache stampede control, telemetry, failure policy |
| RabbitMQ messaging | `Exp.Weather.RestApi.RabbitMq` | RabbitMQ; `EXP_RABBITMQ_USERNAME` and `EXP_RABBITMQ_PASSWORD` | Publishing a Weather response to an exchange/routing key | Durable topology review, publisher confirms, retries, dead-lettering, idempotent consumers |
| Azure Key Vault | `Exp.Weather.BlazorServer.KeyVault` | Azure vault and four `EXP_AZURE_*` variables; optional in-memory secret cache | Fetching a named secret through `Exp.Shared.Secrets` over the Azure SDK | Prefer workload identity, restrict access, avoid displaying values, add audit/rotation procedures |
| Serilog to SQL Server | `Exp.Weather.RestApi.Serilog` | SQL Server; `EXP_SQLSERVER_CONNECTION_STRING` | Structured application/transaction categories and PII redaction options | Database provisioning, retention, backpressure, health and alerting |
| Serilog to MongoDB | `Exp.Weather.BlazorServer.Serilog` | MongoDB; `EXP_MONGODB_CONNECTION_STRING` | Structured UI-side logging and multiple categories | Authentication/TLS, retention, ingestion failure handling, operational dashboards |
| Background service | `Exp.Weather.Worker.BackgroundService` | None | An ASP.NET Core host running a periodic `BackgroundService` beside an API | Durable scheduling, distributed coordination, graceful retries, job persistence |

No shared Weather domain library was created: DTOs and behavior differ, and keeping host code local makes each infrastructure integration independently understandable. Cross-cutting registration lives in `shared/Infrastructure` adapters so Redis, RabbitMQ, Key Vault, and Serilog examples restore from nuget.org without private packages.
