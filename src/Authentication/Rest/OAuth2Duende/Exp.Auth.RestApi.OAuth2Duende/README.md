# REST OAuth 2.0 with Duende IdentityServer

## Objective

Demonstrate an OAuth 2.0 token flow and protected REST resource using Duende.

## Maturity level

**Level 1 — Focused Pattern Example.** This classification describes the example's teaching scope, not production readiness. See the [catalog](../../../../../docs/catalog.md).

## What it demonstrates

- Duende IdentityServer token issuance.
- OAuth client/user demonstration configuration.
- JWT bearer resource protection.

## What it intentionally omits

Production identity and authorization design, complete resilience policy, distributed observability, deployment automation, performance testing, high-availability data design, and exhaustive automated tests unless explicitly shown. These omissions keep the demonstrated concern visible.

## Architecture

Client → IdentityServer token endpoint → access token → bearer middleware → protected controller.

The structure is intentionally proportional to the lesson; it is not presented as a universal application architecture.

## Request or message flow

Client → IdentityServer token endpoint → access token → bearer middleware → protected controller. Validate both the successful path and the authentication, validation, connectivity, or not-found failure relevant to this example.

## Project structure

- `Program.cs` and configuration files compose the host.
- Feature folders contain the protocol, endpoint, service, component, or infrastructure code under study.
- `Properties/launchSettings.json` contains development launch profiles when applicable.
- Generated `bin`, `obj`, databases, credentials, and user-specific files are not source assets.

## Configuration

HTTP 6105; HTTPS 8105. Development clients and users are demonstrations only; production licensing, key management, durable stores, and secure secrets are required.

Use environment variables or .NET user secrets for sensitive development values. Never place live credentials in committed JSON, launch profiles, or NuGet configuration.

## How to run

From the repository root:

```powershell
dotnet run --project src/Authentication/Rest/OAuth2Duende/Exp.Auth.RestApi.OAuth2Duende --launch-profile https
```

Read the console output for the bound endpoint, then use the protocol-appropriate client. External services are not started automatically.

## Test scenarios

- Exercise the primary successful operation.
- Verify one invalid, missing, or not-found input.
- Stop or misconfigure an external dependency and confirm the failure is understandable.
- Run `dotnet build src/Authentication/Rest/OAuth2Duende/Exp.Auth.RestApi.OAuth2Duende` before manual testing.

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

