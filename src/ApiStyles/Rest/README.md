# REST API style examples

These focused, layered Todo examples compare four ways to organize ASP.NET Core REST endpoints. They share the same SQLite persistence implementation but intentionally keep transport code separate so each style remains visible.

## Examples and ports

| Project | Pattern | HTTP | HTTPS | Run |
|---|---|---:|---:|---|
| `Exp.Todo.RestApi.Minimal` | Minimal API routes in `Program.cs` | 5101 | 7101 | `dotnet run --project src/ApiStyles/Rest/Exp.Todo.RestApi.Minimal` |
| `Exp.Todo.RestApi.MvcControllers` | Attribute-routed MVC controller | 5102 | 7102 | `dotnet run --project src/ApiStyles/Rest/Exp.Todo.RestApi.MvcControllers` |
| `Exp.Todo.RestApi.EndpointPerFile` | Route-handler endpoint organization | 5103 | 7103 | `dotnet run --project src/ApiStyles/Rest/Exp.Todo.RestApi.EndpointPerFile` |
| `Exp.Todo.RestApi.FastEndpoints` | FastEndpoints request/handler classes | 5104 | 7104 | `dotnet run --project src/ApiStyles/Rest/Exp.Todo.RestApi.FastEndpoints` |

The HTTPS launch profiles open Swagger. Use `--launch-profile https` to select one explicitly.

## Dependencies and architecture level

All four `net10.0` projects reference `shared/Todo/Exp.Todo.Infrastructure.Sqlite`. The shared project uses EF Core SQLite and creates a local `todo.db` on first use. The examples use AutoMapper; three use Swashbuckle/OpenAPI, while FastEndpoints uses its own Swagger integration. No external service is required.

These are focused layered examples, not Clean Architecture references. DTOs, mapping, and endpoint behavior remain in each host; persistence is shared because the implementation is materially identical across the examples.

## Intentional simplifications and production considerations

- Authentication, validation strategy, concurrency control, pagination, and integration tests are omitted.
- SQLite is local and its relative location depends on the process working directory.
- Error handling and logging are demonstration-level and generate analyzer warnings.
- Production services should add durable migrations, health checks, observability, validation, security, rate limiting, API versioning, and deployment-specific configuration.

## Comparison summary

Minimal APIs offer the least ceremony; MVC controllers provide established conventions and filters; endpoint-per-file organization keeps built-in route handlers modular; FastEndpoints supplies an opinionated request/handler pipeline. Routes remain as implemented in the sources rather than being normalized across examples.
