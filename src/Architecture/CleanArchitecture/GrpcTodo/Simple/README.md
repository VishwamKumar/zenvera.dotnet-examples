# Simple service-layer variant

## Maturity level

**Level 3 — Architecture Reference.** This variant preserves Domain, Application, Infrastructure, host, and tests. See the [catalog](../../../../../docs/catalog.md).


This variant retains Domain, Application, Infrastructure, gRPC host, and tests while using an application service directly from the gRPC service. Direct dependency injection is not itself a Clean Architecture violation. The important constraint is dependency direction: the API calls Application abstractions, Infrastructure implements inward-facing abstractions, and Domain remains independent of outer layers.

Run from the monorepo root:

```powershell
dotnet run --project src/Architecture/CleanArchitecture/GrpcTodo/Simple/src/Exp.Todo.GrpcApi
dotnet test src/Architecture/CleanArchitecture/GrpcTodo/Simple/tests/Exp.Todo.Tests
```

- HTTP: `http://localhost:6401`
- HTTPS/Swagger: `https://localhost:8401/swagger`
- SQLite: local `todo.db`, created relative to the host process working directory

This is the lowest-ceremony option. It favors a straightforward service API over command/query dispatch. See the parent README and comparison matrix for selection guidance.
