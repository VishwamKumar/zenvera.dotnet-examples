# Local infrastructure runbook

Copy `deploy/local/.env.example` to an untracked `deploy/local/.env`, change the development-only values if desired, and export the application-facing variables in the shell used to launch .NET projects.

```powershell
docker compose --env-file deploy/local/.env -f deploy/local/compose.yml up -d
docker compose --env-file deploy/local/.env -f deploy/local/compose.yml ps
```

## Port map

| Port | Owner | Purpose |
|---:|---|---|
| 1433 | SQL Server | Serilog SQL sink database |
| 5672 | RabbitMQ | AMQP |
| 6379 | Redis | Cache |
| 15672 | RabbitMQ | Local management UI |
| 27017 | MongoDB | Serilog Mongo sink |
| 5111 | Redis caching API | HTTP launch profile |
| 5112 | RabbitMQ API | HTTP launch profile |
| 5113 | Key Vault Blazor app | HTTP launch profile |
| 5114 | Serilog REST API | HTTP launch profile |
| 5115 | Serilog Blazor app | HTTP launch profile |
| 5116 | Background service/API | HTTP launch profile |
| 6111–6116 | Same .NET hosts | HTTPS launch profiles |

Redis, RabbitMQ, MongoDB, and SQL Server are local conveniences, not production configurations. The SQL logging example expects its database/tables to be provisioned according to the logging package. Azure Key Vault has no local emulator in this stack: create a development vault, use least-privilege access, and provide `EXP_AZURE_KEY_VAULT_URL`, `EXP_AZURE_TENANT_ID`, `EXP_AZURE_CLIENT_ID`, and `EXP_AZURE_CLIENT_SECRET` only through local environment configuration. Prefer managed/workload identity outside this demonstration.

Stop services with `docker compose -f deploy/local/compose.yml down`. Do not commit `deploy/local/.env`.
