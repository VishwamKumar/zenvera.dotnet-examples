# Infrastructure pattern examples

## Objective

Demonstrate infrastructure capabilities one at a time around a small Weather host so registration, configuration, and runtime dependency remain visible.

## Maturity level

All six examples are **Level 1 — Focused Pattern Example**. They are not four-project architecture templates.

## Catalog

| Capability | Example | Runtime dependency | Current build |
|---|---|---|---|
| Redis caching | `Caching/Redis/Exp.Weather.RestApi.RedisCaching` | Redis | Enabled; authenticated private-package restore required |
| RabbitMQ messaging | `Messaging/RabbitMq/Exp.Weather.RestApi.RabbitMq` | RabbitMQ | Enabled; authenticated private-package restore required |
| Azure Key Vault | `Secrets/AzureKeyVault/Exp.Weather.BlazorServer.KeyVault` | Azure Key Vault | Enabled; authenticated private-package restore required |
| REST logging | `Logging/Serilog/Exp.Weather.RestApi.Serilog` | SQL Server | Enabled; authenticated private-package restore required |
| Blazor logging | `Logging/Serilog/Exp.Weather.BlazorServer.Serilog` | MongoDB | Enabled; authenticated private-package restore required |
| Hosted background work | `BackgroundProcessing/BackgroundService/Exp.Weather.Worker.BackgroundService` | None | Pass |

## How to run

Start dependencies with the [local infrastructure runbook](../../docs/runbooks/local-infrastructure.md), export the documented `EXP_*` settings, then run one project from the repository root. Do not start every host merely because they share a category.

## Production considerations

The examples omit or simplify high availability, retries, telemetry, credential lifecycle, durable messaging, cache failure policy, database provisioning, retention, and operational ownership. See the [comparison matrix](../../docs/comparison-matrices/infrastructure-patterns.md) and focused [RabbitMQ LLD](../../docs/architecture/lld/rabbitmq-flow.md).
