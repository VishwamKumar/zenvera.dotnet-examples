# CQRS with MediatR

## Maturity level

**Level 3 — Architecture Reference.** This variant preserves Domain, Application, Infrastructure, host, and tests. See the [catalog](../../../../../docs/catalog.md).


This variant separates commands and queries and uses MediatR for dispatch. It retains Domain, Application, Infrastructure, gRPC host, and tests.

Run from the monorepo root:

```powershell
dotnet run --project src/Architecture/CleanArchitecture/GrpcTodo/CqrsMediatR/src/Exp.Todo.GrpcApi
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/CqrsMediatR/tests/Exp.Todo.Tests
```

- HTTP: `http://localhost:6403`
- HTTPS/Swagger: `https://localhost:8403/swagger`
- SQLite: local `todo.db`

MediatR supplies a mature dispatch and pipeline model at the cost of another dependency and more indirection. The current source uses MediatR but does not automatically gain production-grade transaction, authorization, idempotency, or observability behaviors; those require explicit pipeline implementations. See the parent README and comparison matrix.
