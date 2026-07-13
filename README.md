# Zenvera .NET Examples

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![PR validation](https://github.com/VishwamKumar/zenvera.dotnet-examples/actions/workflows/pr-validation.yml/badge.svg)](https://github.com/VishwamKumar/zenvera.dotnet-examples/actions/workflows/pr-validation.yml)
[![Build examples](https://github.com/VishwamKumar/zenvera.dotnet-examples/actions/workflows/build-examples.yml/badge.svg)](https://github.com/VishwamKumar/zenvera.dotnet-examples/actions/workflows/build-examples.yml)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

Learn modern .NET by comparing working implementations side by side.

`zenvera.dotnet-examples` is a .NET 10 reference-example monorepo covering API protocols, authentication mechanisms, infrastructure integrations, API gateways, native UI, and three approaches to Clean Architecture. Every example is intentionally scoped, independently understandable, and documented with its trade-offs and production considerations.

**Created and maintained by [Vishwa Kumar](https://vishwa.me).** Visit [vishwa.me](https://vishwa.me) for more engineering articles, projects, and contact information.

> This is a learning and comparison repository—not one enterprise application, a production bounded context, or a drop-in production platform.

## Start here

Choose a path based on what you want to learn:

| I want to… | Start with | Continue with |
|---|---|---|
| Learn ASP.NET Core APIs | [Minimal REST API](src/ApiStyles/Rest/Exp.Todo.RestApi.Minimal/README.md) | [MVC](src/ApiStyles/Rest/Exp.Todo.RestApi.MvcControllers/README.md), [endpoint-per-file](src/ApiStyles/Rest/Exp.Todo.RestApi.EndpointPerFile/README.md), [FastEndpoints](src/ApiStyles/Rest/Exp.Todo.RestApi.FastEndpoints/README.md) |
| Compare API protocols | [Native gRPC](src/ApiStyles/Grpc/Exp.Todo.GrpcApi.Native/README.md) | [Transcoding](src/ApiStyles/Grpc/Exp.Todo.GrpcApi.Transcoding/README.md), [GraphQL](src/ApiStyles/GraphQL/Exp.Todo.GraphQLApi/README.md), [SOAP](src/ApiStyles/Soap/Exp.Todo.SoapApi/README.md) |
| Understand authentication | [REST API key](src/Authentication/Rest/ApiKey/Exp.Auth.RestApi.ApiKey/README.md) | [JWT](src/Authentication/Rest/JwtBearer/Exp.Auth.RestApi.JwtBearer/README.md), [Identity](src/Authentication/Rest/JwtBearerIdentity/Exp.Auth.RestApi.JwtBearerIdentity/README.md), [OAuth 2.0](src/Authentication/Rest/OAuth2Duende/Exp.Auth.RestApi.OAuth2Duende/README.md) |
| Learn Clean Architecture | [Simple service layer](src/Architecture/CleanArchitecture/GrpcTodo/Simple/src/Exp.Todo.GrpcApi/README.md) | [Custom CQRS](src/Architecture/CleanArchitecture/GrpcTodo/Cqrs/src/Exp.Todo.GrpcApi/README.md), [CQRS with MediatR](src/Architecture/CleanArchitecture/GrpcTodo/CqrsMediatR/src/Exp.Todo.GrpcApi/README.md) |
| Explore infrastructure patterns | [Infrastructure overview](src/Infrastructure/README.md) | Redis, RabbitMQ, Key Vault, Serilog, and background processing |
| Explore gateways and UI | [API gateway comparison](src/Integration/ApiGateway/RestApis/README.md) | [Ocelot](src/Integration/ApiGateway/RestApis/Ocelot/Exp.ApiGateway.Ocelot/README.md), [YARP](src/Integration/ApiGateway/RestApis/Yarp/Exp.ApiGateway.Yarp/README.md), [.NET MAUI](src/UserInterface/Maui/Exp.Todo.Maui.RestApiClient/README.md) |

For a guided progression through the entire repository, follow the [recommended learning path](docs/learning-path.md).

## What is included

### API styles

Use the same Todo domain to see how delivery style changes the shape of an application.

| Style | Examples | What you can compare |
|---|---:|---|
| [REST](src/ApiStyles/Rest/README.md) | 4 | Minimal APIs, MVC controllers, endpoint-per-file organization, and FastEndpoints |
| [gRPC](src/ApiStyles/Grpc/README.md) | 2 | Native Protocol Buffers and JSON transcoding |
| [GraphQL](src/ApiStyles/GraphQL/README.md) | 1 | Schema, queries, mutations, and resolver-based delivery |
| [SOAP](src/ApiStyles/Soap/README.md) | 1 | CoreWCF contracts, operations, data contracts, and WSDL interoperability |

### Authentication

Compare where credentials travel, how they are validated, and what must be hardened for real systems.

| Protocol | Demonstrated mechanisms |
|---|---|
| [REST](src/Authentication/Rest/README.md) | API key, Basic authentication, JWT bearer, ASP.NET Core Identity with JWT, OAuth 2.0 with Duende |
| [gRPC](src/Authentication/Grpc/README.md) | API-key metadata, JWT bearer metadata, mutual TLS |
| [GraphQL](src/Authentication/GraphQL/README.md) | API key, JWT bearer, Firebase authentication |

See the [authentication comparison matrix](docs/comparison-matrices/authentication-styles.md) for credential transport, rotation, revocation, relative strength, and recommended use.

### Architecture, integration, infrastructure, and UI

| Area | Examples | Focus |
|---|---:|---|
| [Clean Architecture](src/Architecture/CleanArchitecture/GrpcTodo/README.md) | 3 | Simple service layer, CQRS with a custom dispatcher, and CQRS with MediatR |
| [API gateways](src/Integration/ApiGateway/RestApis/README.md) | 2 | Ocelot and YARP routing boundaries |
| [Infrastructure](src/Infrastructure/README.md) | 6 | Redis, RabbitMQ, Azure Key Vault, Serilog, MongoDB/SQL sinks, and hosted background work |
| [User interface](src/UserInterface/README.md) | 1 | .NET MAUI native client consuming the Todo REST API |

The [complete catalog](docs/catalog.md) lists every example, project path, framework, external dependency, build status, test status, and recommended learning sequence.

## Example maturity levels

Maturity describes the teaching scope—not production readiness.

| Level | Meaning | Typical examples |
|---|---|---|
| **Level 1 — Focused Pattern Example** | Demonstrates one technology or implementation concern with minimal ceremony | Authentication, gateways, infrastructure, MAUI |
| **Level 2 — Layered Example** | Separates delivery from reusable domain or persistence concerns | REST, gRPC, GraphQL, and SOAP Todo examples |
| **Level 3 — Architecture Reference** | Preserves Domain, Application, Infrastructure, host, and tests | Three gRPC Clean Architecture variants |

Small examples are deliberately not forced into four Clean Architecture projects. Architecture-reference examples retain the complete separation because dependency direction is the lesson.

## Five-minute quick start

### Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0); `global.json` selects SDK 10.0.301 with feature-band roll-forward.
- A trusted ASP.NET Core development certificate for HTTPS examples.
- Docker Desktop or another Compose-compatible engine only for examples that use Redis, RabbitMQ, MongoDB, or SQL Server.
- Platform-specific .NET MAUI workloads only for the native UI example.

### Run the first example

The Minimal REST API is self-contained and uses a local SQLite database:

```powershell
git clone https://github.com/VishwamKumar/zenvera.dotnet-examples.git
cd zenvera.dotnet-examples
dotnet restore solutions/zenvera.api-styles.slnx
dotnet run --project src/ApiStyles/Rest/Exp.Todo.RestApi.Minimal --launch-profile https
```

Open the address printed by ASP.NET Core and use the Swagger/OpenAPI UI to exercise Todo CRUD operations. The generated SQLite database is ignored by Git.

Build the public, self-contained learning categories independently:

```powershell
dotnet build solutions/zenvera.api-styles.slnx
dotnet build solutions/zenvera.authentication.slnx
dotnet build solutions/zenvera.architecture.slnx
dotnet build solutions/zenvera.integration.slnx
```

## Solutions and repository navigation

The repository provides one authoritative root solution and smaller category solutions for easier exploration.

| Solution | Scope |
|---|---|
| `zenvera.dotnet-examples.slnx` | All 44 maintained projects |
| `solutions/zenvera.api-styles.slnx` | REST, gRPC, GraphQL, SOAP, and shared Todo persistence |
| `solutions/zenvera.authentication.slnx` | REST, gRPC, and GraphQL authentication examples |
| `solutions/zenvera.architecture.slnx` | Clean Architecture variants and their tests |
| `solutions/zenvera.integration.slnx` | Ocelot and YARP gateways |
| `solutions/zenvera.infrastructure.slnx` | Infrastructure-focused Weather hosts |
| `solutions/zenvera.user-interface.slnx` | .NET MAUI client |

Open the root or a category `.slnx` in Visual Studio, Rider, or the .NET CLI. Every maintained project belongs to the root solution.

## External services and private packages

Most API-style, authentication, gateway, and architecture examples compile without starting external infrastructure. Runtime requirements are documented in each example README.

Local service definitions are available for Redis, RabbitMQ, MongoDB, and SQL Server:

```powershell
Copy-Item deploy/local/.env.example deploy/local/.env
docker compose --env-file deploy/local/.env -f deploy/local/compose.yml up -d
```

Read the [local infrastructure runbook](docs/runbooks/local-infrastructure.md) for ports, safe development defaults, health checks, and shutdown commands.

### A note for public visitors

Several infrastructure examples use the author's private `Zenvera.Shared.*` packages. Their source projects remain visible for architectural study, but restoring them requires authorized access to the configured GitHub Packages feed. No token or package credential is stored in this repository.

If you do not have access, use the API styles, authentication, architecture, integration, or background-service examples; they remain independently explorable. Maintainers can configure private-feed access using the [CI validation runbook](docs/runbooks/ci-validation.md).

Azure Key Vault, Firebase, mutual-TLS certificates, and gateway downstream services require their own development resources or credentials at runtime.

## Testing and validation

The three Level 3 architecture references contain automated tests:

```powershell
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/Simple/tests/Exp.Todo.Tests
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/Cqrs/tests/Exp.Todo.Tests
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/CqrsMediatR/tests/Exp.Todo.Tests
```

Focused examples include reproducible manual scenarios in their READMEs. CI validates the tracked example manifest, repository hygiene, restore, build, tests, formatting, Markdown links, and Docker Compose configuration. See the [CI validation runbook](docs/runbooks/ci-validation.md).

## Architecture principles

- Keep each example focused on the technology or decision it teaches.
- Use layering only when it makes delivery, application, domain, or persistence boundaries clearer.
- Preserve full dependency direction in architecture-reference examples.
- Share only materially identical Todo, Weather, contracts, or test utilities.
- Keep protocol contracts and example-specific behavior local.
- Prefer one root solution plus category solutions over copied repository-level solutions.
- Document intentional omissions and production hardening instead of presenting samples as production-ready.

The repository itself is described in the [high-level design](docs/architecture/hld.md), [C4 documentation](docs/architecture/c4/level-1-context.md), and [reference-example monorepo ADR](docs/architecture/adr/ADR-001-reference-example-monorepo.md).

## Production-use disclaimer

These examples are learning references, not certified production templates. Depending on the example, they may simplify or omit threat modeling, authorization depth, secret rotation, certificate lifecycle, database migrations, resilience, telemetry, high availability, accessibility, load testing, deployment policy, and operational ownership.

Use each README's **Production considerations** section as the beginning of an engineering review—not as evidence that the example is production-ready.

## Contributing

Contributions are welcome when they preserve the repository's comparison-first purpose.

- Keep an example's learning objective narrow and assign a maturity level.
- Do not redesign unrelated examples merely for stylistic uniformity.
- Preserve public routes and protocol contracts unless the change explicitly teaches contract evolution.
- Add new projects to the root solution, category solution, and [build manifest](build/example-build-manifest.json).
- Follow the `Exp` naming conventions and documented port map.
- Never commit credentials, generated databases, build outputs, or user-specific files.
- Update the catalog, README, tests, comparison matrix, and runbook affected by the change.
- Use the [example README template](docs/templates/example-readme-template.md).

## About the author

This repository is created and maintained by **Vishwa Kumar**.

- Website: [vishwa.me](https://vishwa.me)
- GitHub: [VishwamKumar](https://github.com/VishwamKumar)

If these examples help you, consider starring the repository and sharing the learning path with other .NET developers.

## License

Licensed under the [MIT License](LICENSE).
