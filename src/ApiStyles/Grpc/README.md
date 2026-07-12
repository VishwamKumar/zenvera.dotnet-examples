# gRPC API style examples

These focused, layered Todo services compare native gRPC with gRPC JSON transcoding. Proto contracts and service behavior are preserved from the source repository.

## Examples and ports

| Project | Pattern | HTTP | HTTPS | Run |
|---|---|---:|---:|---|
| `Exp.Todo.GrpcApi.Native` | Native HTTP/2 gRPC | 5201 | 7201 | `dotnet run --project src/ApiStyles/Grpc/Exp.Todo.GrpcApi.Native` |
| `Exp.Todo.GrpcApi.Transcoding` | gRPC plus JSON transcoding and Swagger | 5202 | 7202 | `dotnet run --project src/ApiStyles/Grpc/Exp.Todo.GrpcApi.Transcoding` |

Use a gRPC client or `grpcurl` for the native service. The transcoding profile opens Swagger. Development HTTPS may require `dotnet dev-certs https --trust`.

## Dependencies and architecture level

Both `net9.0` hosts use `Grpc.AspNetCore`, reflection, AutoMapper, and the shared EF Core SQLite Todo project. Transcoding adds ASP.NET Core JSON transcoding and Swagger and retains the Google annotation protos required by its contract. No external service is required.

These are focused layered transport examples, not Clean Architecture references. Generated surfaces and proto contracts stay local to each host.

## Intentional simplifications and production considerations

- Reflection is enabled for learning and should be restricted appropriately in production.
- Authentication, deadline policy, retry guidance, load balancing, telemetry, and integration clients are omitted.
- Production deployments should configure HTTP/2/TLS, health checks, observability, compatible proto evolution, client generation, and ingress/proxy support.

## Comparison summary

Native gRPC provides the canonical strongly typed binary protocol. Transcoding adds HTTP/JSON access while retaining the service contract, at the cost of extra annotations, packages, and mapping concerns.
