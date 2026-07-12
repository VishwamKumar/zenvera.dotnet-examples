# CQRS with custom dispatcher

## Maturity level

**Level 3 — Architecture Reference.** This variant preserves Domain, Application, Infrastructure, host, and tests. See the [catalog](../../../../../docs/catalog.md).


This variant separates commands and queries and dispatches them through a small in-repo dispatcher. It retains Domain, Application, Infrastructure, gRPC host, and tests without adding MediatR.

Run from the monorepo root:

```powershell
dotnet run --project src/Architecture/CleanArchitecture/GrpcTodo/Cqrs/src/Exp.Todo.GrpcApi
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/Cqrs/tests/Exp.Todo.Tests
```

- HTTP: `http://localhost:6402`
- HTTPS/Swagger: `https://localhost:8402/swagger`
- Health endpoints implemented by the host: `/health` and `/health/ready`
- SQLite: local `todo.db`

The custom dispatcher makes command/query flow explicit and avoids a mediator dependency, but the application owns reflection/registration/dispatch behavior and its maintenance risks. See the parent README and comparison matrix.
