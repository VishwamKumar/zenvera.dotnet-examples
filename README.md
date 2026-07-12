# Zenvera .NET Examples

`zenvera.dotnet-examples` is a reference-example monorepo for learning and comparing .NET API styles, authentication mechanisms, infrastructure integrations, gateways, UI frameworks, and architecture patterns.

## Repository classification

This is a teaching and comparison repository—not one enterprise application, production bounded context, or deployable platform. Examples are independently runnable and intentionally vary in structure so the pattern under study remains visible.

## Architecture principles

- Preserve focused examples; do not impose Clean Architecture on a small technology demonstration.
- Use layering only when it clarifies delivery, application/data, or shared domain responsibilities.
- Preserve Domain, Application, Infrastructure, host, and tests for architecture references.
- Share only materially identical Todo, Weather, contracts, or test utilities.
- Keep protocol contracts and example-specific behavior local.
- Prefer one root solution, category filters, central build defaults, and explicit compatibility exceptions.

## Example maturity levels

### Level 1 — Focused Pattern Example

Demonstrates one technology or implementation concern with minimal ceremony.

### Level 2 — Layered Example

Demonstrates separation between delivery, application/data, or shared domain concerns.

### Level 3 — Architecture Reference

Demonstrates full Clean Architecture with Domain, Application, Infrastructure, host, and tests.

Maturity describes teaching scope, not production readiness.

## Category catalog

| Category | Examples | Maturity | Start here |
|---|---:|---|---|
| REST styles | 4 | Level 2 | Minimal API, then MVC and endpoint organization |
| gRPC styles | 2 | Level 2 | Native gRPC, then JSON transcoding |
| GraphQL and SOAP | 2 | Level 2 | Compare schema/query and contract/WSDL models |
| Authentication | 11 | Level 1 | API key, JWT, then managed/PKI mechanisms |
| Infrastructure | 6 | Level 1 | Caching, messaging, secrets, logging, hosted work |
| API gateways | 2 | Level 1 | Ocelot and YARP routing boundaries |
| Clean Architecture | 3 | Level 3 | Simple, custom CQRS, then MediatR |
| User interface | 1 | Level 1 | .NET MAUI REST client |

See the [complete example catalog](docs/catalog.md) for paths, dependencies, validation status, and readiness notes.

## Learning paths

The recommended progression starts with REST endpoint organization, adds protocol and authentication choices, introduces infrastructure and integration boundaries, and finishes with architecture comparison and UI consumption. Follow the [learning path](docs/learning-path.md) for the full sequence.

## Prerequisites

- .NET SDK 10.0.301 or a compatible feature band selected by `global.json`.
- .NET 8 and .NET 9 targeting packs used by preserved examples.
- Platform-specific .NET MAUI workloads for the MAUI client. Its current .NET 8 targets are reported out of support by SDK 10 and are retained pending a dedicated upgrade.
- Docker Desktop or another Compose-compatible engine for local Redis, RabbitMQ, MongoDB, and SQL Server.
- A trusted development HTTPS certificate for HTTPS/gRPC examples.
- Example-specific external credentials only where documented, such as Firebase or Azure Key Vault.

## How to build

Restore and build the root solution:

```powershell
dotnet restore zenvera.dotnet-examples.slnx --configfile NuGet.config -p:NuGetAudit=false
dotnet build zenvera.dotnet-examples.slnx --no-restore -p:NuGetAudit=false
```

The complete solution currently reports documented blockers from unavailable `Zenvera.Shared.*` packages and preserved .NET 8 MAUI workloads. Use a validated category filter for an isolated learning area:

```powershell
dotnet build solutions/api-styles.slnf -p:NuGetAudit=false
dotnet build solutions/authentication.slnf -p:NuGetAudit=false
dotnet build solutions/architecture.slnf -p:NuGetAudit=false
```

Current results and exceptions are recorded in the [repository consistency report](docs/repository-consistency-report.md).

## How to run an example

Run the project path shown in the catalog, optionally selecting a launch profile:

```powershell
dotnet run --project src/ApiStyles/Rest/Exp.Todo.RestApi.Minimal --launch-profile https
```

Read the category and project README first. It identifies the port, protocol client, configuration, dependencies, intentional omissions, and production concerns. Examples using SQLite create a local ignored database file.

## External infrastructure

Local Redis, RabbitMQ, MongoDB, and SQL Server are defined in `deploy/local/compose.yml`:

```powershell
Copy-Item deploy/local/.env.example deploy/local/.env
docker compose --env-file deploy/local/.env -f deploy/local/compose.yml up -d
```

Azure Key Vault, Firebase, gateway downstream APIs, certificates, and some identity databases remain external by design. See the [local infrastructure runbook](docs/runbooks/local-infrastructure.md).

## Solution filters

| Filter | Scope |
|---|---|
| `solutions/api-styles.slnf` | REST, gRPC, GraphQL, SOAP, shared Todo persistence |
| `solutions/authentication.slnf` | REST, gRPC, and GraphQL authentication |
| `solutions/architecture.slnf` | Three Clean Architecture variants and tests |
| `solutions/infrastructure.slnf` | Infrastructure pattern hosts |
| `solutions/ui.slnf` | .NET MAUI client |

All maintained projects also belong to the root `.slnx`.

## Testing

Automated tests currently belong to the three Level 3 Clean Architecture variants:

```powershell
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/Simple/tests/Exp.Todo.Tests
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/Cqrs/tests/Exp.Todo.Tests
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/CqrsMediatR/tests/Exp.Todo.Tests
```

Focused examples use documented manual scenarios. Adding a focused integration test is welcome when it clarifies the demonstrated behavior without turning the example into an unrelated test architecture.

## Production-use disclaimer

These examples are learning references, not production templates. They may omit threat modeling, authorization depth, migrations, resilience, telemetry, secret rotation, certificate lifecycle, scale testing, accessibility, deployment policy, and operational ownership. Treat every production consideration section as a review starting point, not a certification.

## Contribution rules

- Keep the educational objective narrow and state its maturity level.
- Do not redesign unrelated examples for stylistic uniformity.
- Preserve public routes and protocol contracts unless the change explicitly teaches contract evolution.
- Add projects to the root solution and the relevant filter.
- Use `Exp` project names and the repository naming conventions.
- Never commit credentials, user files, generated databases, or build outputs.
- Update the catalog, example README, port map, tests, and comparison documentation with the change.
- Use the [example README template](docs/templates/example-readme-template.md).

## License

Licensed under the [MIT License](LICENSE).
