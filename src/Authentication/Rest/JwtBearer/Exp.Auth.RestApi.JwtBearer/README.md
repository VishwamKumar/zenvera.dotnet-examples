# REST JWT bearer authentication

## Objective

Show issuing and validating a development JWT for a REST API.

## Maturity level

**Level 1 — Focused Pattern Example.** This classification describes the example's teaching scope, not production readiness. See the [catalog](../../../../../docs/catalog.md).

## What it demonstrates

- Credential exchange for a JWT.
- Bearer-token validation.
- Protected Weather endpoint.

## What it intentionally omits

Production identity and authorization design, complete resilience policy, distributed observability, deployment automation, performance testing, high-availability data design, and exhaustive automated tests unless explicitly shown. These omissions keep the demonstrated concern visible.

## Architecture

Client → token endpoint → signed JWT → bearer middleware → protected controller.

The structure is intentionally proportional to the lesson; it is not presented as a universal application architecture.

## Request or message flow

Client → token endpoint → signed JWT → bearer middleware → protected controller. Validate both the successful path and the authentication, validation, connectivity, or not-found failure relevant to this example.

## Project structure

- `Program.cs` and configuration files compose the host.
- Feature folders contain the protocol, endpoint, service, component, or infrastructure code under study.
- `Properties/launchSettings.json` contains development launch profiles when applicable.
- Generated `bin`, `obj`, databases, credentials, and user-specific files are not source assets.

## Configuration

HTTP 6103; HTTPS 8103. Set `JwtSettings__SecretKey` through environment variables or user secrets; launch profiles contain only a development placeholder.

Use environment variables or .NET user secrets for sensitive development values. Never place live credentials in committed JSON, launch profiles, or NuGet configuration.

## How to run

From the repository root:

```powershell
dotnet run --project src/Authentication/Rest/JwtBearer/Exp.Auth.RestApi.JwtBearer --launch-profile https
```

Read the console output for the bound endpoint, then use the protocol-appropriate client. External services are not started automatically.

## Test scenarios

- Exercise the primary successful operation.
- Verify one invalid, missing, or not-found input.
- Stop or misconfigure an external dependency and confirm the failure is understandable.
- Run `dotnet build src/Authentication/Rest/JwtBearer/Exp.Auth.RestApi.JwtBearer` before manual testing.

No example should be treated as fully verified solely because it compiles; consult the [build manifest](../../../../../build/example-build-manifest.json) for automated validation status.

## Production considerations

Add threat modeling, secure secret storage and rotation, least-privilege authorization, input limits, rate limiting where appropriate, retries/timeouts only at safe boundaries, health checks, structured telemetry, data lifecycle controls, load testing, and deployment-specific hardening. Review protocol and dependency compatibility before adopting the pattern.

## Related examples

- [REST authentication](../../README.md)
- [Example catalog](../../../../../docs/catalog.md)
- [Learning path](../../../../../docs/learning-path.md)

## Related standards

- [Reference-example monorepo ADR](../../../../../docs/architecture/adr/ADR-001-reference-example-monorepo.md)
- [Example README template](../../../../../docs/templates/example-readme-template.md)
- [Repository license](../../../../../LICENSE)

