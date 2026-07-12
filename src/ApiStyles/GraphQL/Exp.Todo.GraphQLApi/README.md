# Todo GraphQL API

## Objective

Demonstrate schema-driven Todo queries and mutations with Hot Chocolate.

## Maturity level

**Level 2 — Layered Example.** This classification describes the example's teaching scope, not production readiness. See the [catalog](../../../../docs/catalog.md).

## What it demonstrates

- GraphQL query and mutation types.
- GraphQL object/input types.
- Shared EF Core SQLite persistence.

## What it intentionally omits

Production identity and authorization design, complete resilience policy, distributed observability, deployment automation, performance testing, high-availability data design, and exhaustive automated tests unless explicitly shown. These omissions keep the demonstrated concern visible.

## Architecture

GraphQL request → Hot Chocolate schema resolver → shared Todo repository → SQLite.

The structure is intentionally proportional to the lesson; it is not presented as a universal application architecture.

## Request or message flow

GraphQL request → Hot Chocolate schema resolver → shared Todo repository → SQLite. Validate both the successful path and the authentication, validation, connectivity, or not-found failure relevant to this example.

## Project structure

- `Program.cs` and configuration files compose the host.
- Feature folders contain the protocol, endpoint, service, component, or infrastructure code under study.
- `Properties/launchSettings.json` contains development launch profiles when applicable.
- Generated `bin`, `obj`, databases, credentials, and user-specific files are not source assets.

## Configuration

HTTP 5301; HTTPS 7301. SQLite is configured through `ConnectionStrings`; use the GraphQL endpoint/UI exposed by the host.

Use environment variables or .NET user secrets for sensitive development values. Never place live credentials in committed JSON, launch profiles, or NuGet configuration.

## How to run

From the repository root:

```powershell
dotnet run --project src/ApiStyles/GraphQL/Exp.Todo.GraphQLApi --launch-profile https
```

Read the console output for the bound endpoint, then use the protocol-appropriate client. External services are not started automatically.

## Test scenarios

- Exercise the primary successful operation.
- Verify one invalid, missing, or not-found input.
- Stop or misconfigure an external dependency and confirm the failure is understandable.
- Run `dotnet build src/ApiStyles/GraphQL/Exp.Todo.GraphQLApi` before manual testing.

No example should be treated as fully verified solely because it compiles; consult the [build manifest](../../../../build/example-build-manifest.json) for automated validation status.

## Production considerations

Add threat modeling, secure secret storage and rotation, least-privilege authorization, input limits, rate limiting where appropriate, retries/timeouts only at safe boundaries, health checks, structured telemetry, data lifecycle controls, load testing, and deployment-specific hardening. Review protocol and dependency compatibility before adopting the pattern.

## Related examples

- [GraphQL styles](../README.md)
- [Example catalog](../../../../docs/catalog.md)
- [Learning path](../../../../docs/learning-path.md)

## Related standards

- [Reference-example monorepo ADR](../../../../docs/architecture/adr/ADR-001-reference-example-monorepo.md)
- [Example README template](../../../../docs/templates/example-readme-template.md)
- [Repository license](../../../../LICENSE)

