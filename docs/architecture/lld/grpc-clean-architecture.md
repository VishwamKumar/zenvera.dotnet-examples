# LLD — gRPC Clean Architecture variants

## Scope

Applies to `Simple`, `Cqrs`, and `CqrsMediatR` under `src/Architecture/CleanArchitecture/GrpcTodo`. Each variant is a self-contained Level 3 reference with Domain, Application, Infrastructure, gRPC host, and tests.

## Dependency design

```mermaid
flowchart LR
  Client[gRPC / transcoded client] --> Host[Exp.Todo.GrpcApi]
  Host --> App[Exp.Todo.Application]
  Host --> Infra[Exp.Todo.Infrastructure]
  Infra --> App
  App --> Domain[Exp.Todo.Domain]
  Infra --> Domain
  Tests[Exp.Todo.Tests] --> App
  Tests --> Infra
```

The host is the composition root and may reference Infrastructure to register implementations. Domain remains independent. Application owns use-case abstractions and behavior; Infrastructure implements persistence concerns inward-facing abstractions require.

## Dispatch variants

- **Simple:** the gRPC service calls an Application service directly. Dependency injection does not violate Clean Architecture; dependency direction remains inward.
- **CQRS:** gRPC maps requests to commands/queries dispatched through repository-owned abstractions.
- **CQRS + MediatR:** handlers use MediatR dispatch and can use its pipeline model for cross-cutting behavior.

## Request flow

```mermaid
sequenceDiagram
  participant C as Client
  participant G as gRPC host
  participant A as Application service/handler
  participant R as Infrastructure repository
  participant D as SQLite
  C->>G: Protobuf request
  G->>A: DTO/service call or command/query
  A->>R: Persistence abstraction
  R->>D: EF Core operation
  D-->>R: Result
  R-->>A: Domain/application result
  A-->>G: DTO/result
  G-->>C: Protobuf response or gRPC status
```

## Error and validation boundaries

Transport code maps exceptions to response/status behavior. Application validation protects use cases. Database and deployment failures remain outer concerns. Production work includes consistent status contracts, cancellation/deadline propagation, migrations, concurrency, telemetry, and integration tests over the actual gRPC transport.

See the [comparison matrix](../../comparison-matrices/clean-architecture-options.md).
