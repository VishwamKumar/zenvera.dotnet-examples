# gRPC Todo Clean Architecture references

This directory contains three independently runnable implementations of the same gRPC Todo use case. They are full architecture references rather than small technology samples, so every approach retains:

```text
src/
|-- Exp.Todo.Domain/
|-- Exp.Todo.Application/
|-- Exp.Todo.Infrastructure/
`-- Exp.Todo.GrpcApi/
tests/
`-- Exp.Todo.Tests/
```

## Architecture accuracy and dependency direction

Clean Architecture is principally about source-code dependency direction and policy boundaries, not whether an object is obtained through dependency injection or a mediator. Directly injecting an Application service into a gRPC service does not inherently violate Clean Architecture.

For these examples, evaluate the following:

- **API -> Application:** the gRPC host translates transport messages and invokes Application services, commands, queries, or abstractions.
- **Infrastructure -> Application/Domain:** EF Core repositories and persistence implement abstractions defined inward of Infrastructure.
- **Domain -> nothing outward:** entities and domain rules do not depend on ASP.NET Core, gRPC, EF Core, or Infrastructure.
- **Composition root:** the API host may reference Infrastructure to register concrete implementations. This is an outer-layer composition concern, not an inward Domain dependency.

The examples are useful references, not proof that every class or package choice is ideal. Existing duplicate package references, analyzer warnings, exception handling, SQLite lifecycle, and cross-cutting concerns still require production review.

## Variants

| Variant | Dispatch style | Ports HTTP/HTTPS | Tests |
|---|---|---:|---:|
| [Simple](Simple/README.md) | Application service injected into the gRPC service | 6401 / 8401 | 27 |
| [CQRS](Cqrs/README.md) | Commands/queries through custom dispatcher | 6402 / 8402 | 5 |
| [CQRS + MediatR](CqrsMediatR/README.md) | Commands/queries through MediatR | 6403 / 8403 | 5 |

All hosts and supporting projects target `net10.0`. Each variant owns its Domain/Application/Infrastructure code even where files look similar so architectural comparison remains complete.

## Build, run, and test

```powershell
dotnet restore zenvera.dotnet-examples.slnx --configfile NuGet.config -p:NuGetAudit=false
dotnet build zenvera.dotnet-examples.slnx --no-restore -p:NuGetAudit=false
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/Simple/tests/Exp.Todo.Tests --no-build
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/Cqrs/tests/Exp.Todo.Tests --no-build
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/CqrsMediatR/tests/Exp.Todo.Tests --no-build
```

Each variant includes a Dockerfile retained beside its variant root. Build it with that variant directory as the Docker build context after reviewing the local database/log volume requirements.

For selection guidance, see [Clean Architecture options](../../../../docs/comparison-matrices/clean-architecture-options.md).
