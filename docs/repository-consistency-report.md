# Repository consistency report

> Historical note: this report records the pre-upgrade consistency pass. Framework, package, and build-status statements below are superseded by the [.NET 10 and package upgrade report](migration/dotnet-10-package-upgrade-report.md).

## Scope and outcome

This pass reviewed all 44 maintained projects in `zenvera.dotnet-examples` without redesigning working examples or upgrading target frameworks. Every project is present in the root `zenvera.dotnet-examples.slnx`; no project is excluded. No nested `.sln` or `.slnx` files remain.

## Changes made

- Removed generated `bin`, `obj`, `.vs`, and `.idea` directories (55 directories at the start of the pass).
- Removed a copied `CqrsMediatR/Directory.Build.props` that duplicated four project-level package references; affected projects already declare their framework and compiler settings.
- Confirmed there is one root solution and all 44 discovered project files belong to it.
- Added independently openable category solutions for API styles, authentication, architecture, integration, infrastructure, and UI. These supersede the earlier `.slnf` navigation files.
- Repaired copied README license links to point to the root license and fixed the Clean Architecture comparison link.
- Renamed three stale HTTP request files that still used source-repository project names and updated their variables/ports.
- Corrected stale authentication test-document ports to their migrated launch-profile assignments.
- Corrected the MAUI client default backend from the gRPC port 5201 to the Todo Minimal REST API at 5101.
- Retained central deterministic builds, recommended analyzer level, and non-fatal warnings. No global nullable, implicit-usings, or language-version switch was imposed.

## Naming and retained exceptions

All project names use the `Exp` prefix, and test projects end in `.Tests`. Primary API-style hosts use `RestApi`, `GrpcApi`, `GraphQLApi`, and `SoapApi` markers. C# source folders use PascalCase except conventional per-variant `src` and `tests` folders, retained to preserve the independently runnable Clean Architecture layouts.

Several focused examples place the mechanism after the host marker, for example `Exp.Auth.RestApi.JwtBearer`, `Exp.Todo.GrpcApi.Native`, and `Exp.Weather.BlazorServer.Serilog`. Strictly moving the host marker to the final segment would require broad namespace, XAML, protocol, launch-profile, and documentation rewrites without improving discoverability. These names are retained deliberately. `Exp.Weather.Worker.BackgroundService` is also retained: it is an ASP.NET Core host that demonstrates a hosted background service alongside an endpoint, not a pure Worker SDK process.

The API gateway projects (`Exp.ApiGateway.Ocelot` and `Exp.ApiGateway.Yarp`) do not use a protocol-host suffix because they represent gateway products rather than downstream REST hosts. The MAUI project uses `Exp.Todo.Maui.RestApiClient`, which describes its UI framework and dependency.

## Central build configuration

| Setting | Decision |
|---|---|
| SDK | `10.0.301`, `latestFeature`, no previews |
| Deterministic build | Enabled centrally |
| CI build metadata | Enabled when `CI=true` |
| Warnings as errors | Disabled |
| Analyzers | `latest-recommended`; individual analyzer warnings remain educational findings |
| Nullable | Project-local; not all imported examples have been proven compatible with a global switch |
| Implicit usings | Project-local for the same reason |
| C# language version | Framework/SDK default; no global `LangVersion` override |
| Central package management | Opt-in and disabled by default; project-level versions remain authoritative |
| Test stack | Three Clean Architecture test projects use xUnit 2.9.3, test SDK 18.0.1, runner 3.1.5, coverlet 6.0.4, FluentAssertions 8.8.0, and Moq 4.20.72 |

`Directory.Packages.props` remains cautious. Its optional version set covers packages already common across compatible examples, but enabling it globally would incorrectly collapse intentional .NET 8, 9, and 10 package lines.

## Framework versions

| Target | Projects / purpose | Status |
|---|---|---|
| .NET 8 | Ocelot gateway; .NET MAUI Android/iOS/Mac Catalyst/Windows | Preserved; SDK 10 reports MAUI net8 workloads as out of support (`NETSDK1202`) |
| .NET 9 | API styles, authentication, YARP, shared SQLite, and Simple Domain | Preserved; no framework upgrade performed |
| .NET 10 | Clean Architecture applications/hosts/tests and infrastructure examples | Current selected SDK line |

The Simple Clean Architecture Domain remains `net9.0` while its outer projects target `net10.0`. This compatible inward dependency is retained to avoid a framework-only edit during a consistency pass.

## Package compatibility and conflicts

- AutoMapper intentionally spans 15.0.0 (API styles) and 15.1.0 (Clean Architecture).
- EF Core spans 9.0.7 and 10.0.0 according to project target framework.
- Swashbuckle spans 6.6.2 (Ocelot/.NET 8), 9.0.3 (.NET 9 and RabbitMQ), and 10.0.1 (several infrastructure hosts).
- Hot Chocolate authentication/API projects consistently use 15.1.7.
- gRPC 2.71.0 is common, while .NET 9/10 transcoding packages follow their framework lines.
- Redis requests unavailable `Zenvera.Shared.Caching` 0.0.25 and restores 1.0.17 with `NU1603`.
- REST/Blazor logging request unavailable `Zenvera.Shared.Logging` 1.0.8/1.0.6 and restore 1.0.17 with `NU1603` where restore can continue.
- `Zenvera.Shared.Queuing` 0.0.10, `Zenvera.Shared.Secrets` 1.0.21, and `Zenvera.Shared.ErrorHandling` 1.0.6 are unavailable from the configured sources and block their projects.
- Package versions were not forcibly unified because that would be a behavior and compatibility change.

## Port assignments

| Category | HTTP | HTTPS | Notes |
|---|---|---|---|
| API styles / REST | 5101–5104 | 7101–7104 | Minimal, MVC, endpoint-per-file, FastEndpoints |
| API styles / gRPC | 5201–5202 | 7201–7202 | Native and transcoding |
| API styles / GraphQL | 5301 | 7301 | `/graphql` |
| API styles / SOAP | 5401 | 7401 | SOAP host |
| Authentication / REST | 6101–6105 | 8101–8105 | API key, Basic, JWT, Identity JWT, OAuth2 |
| Authentication / gRPC | 6201–6203 | 8201–8203 | API key, JWT, mutual TLS |
| Authentication / GraphQL | 6301–6303 | 8301–8303 | API key, JWT, Firebase |
| Clean Architecture | 6401–6403 | 8401–8403 | Simple, CQRS, CQRS/MediatR |
| API gateways | 6501–6502 | 8501–8502 | Ocelot and YARP |
| Infrastructure | 5111–5116 | 6111–6116 | Redis, RabbitMQ, Key Vault, logging, background service |
| Local dependencies | 1433, 5672, 6379, 15672, 27017 | n/a | SQL Server, RabbitMQ, Redis, RabbitMQ UI, MongoDB |

The MAUI client uses REST port 5101 and does not host a port. YARP intentionally references an external Hour Tracker service on 7198. No maintained launch-profile host-port conflicts remain; repeated ports inside multiple profiles of the same project are intentional.

## Repository hygiene and documentation

No nested `.gitignore`, `.gitattributes`, copied repository licenses, user files, generated databases, or nested solutions were found after cleanup. Operational paths under `docs`, `deploy/local`, and `solutions` use kebab-case where appropriate. PascalCase source category paths are retained for .NET discoverability.

The consolidation analysis and staged migration reports remain historical records, not dead documentation. Some source-era prose and class names use `WeatherApp` as the demonstrated domain; these are not obsolete repository references. HTTP request filenames and runnable port instructions that were demonstrably stale were corrected.

## Security findings

- No live Azure tenant/client GUIDs or committed client secrets were found in migrated configuration.
- Infrastructure credentials use `ENV:EXP_*` indirection; `deploy/local/.env.example` contains explicitly development-only placeholders.
- Authentication examples contain development-only sample API keys/passwords by design and label them accordingly. They are not production credentials.
- Firebase configuration expects an ignored local `firebasesettings.json`.
- SQLite connection strings are local relative database filenames; generated databases are ignored and removed during hygiene checks.
- No absolute `C:\Users\...` or `D:\...` source/configuration path is required by maintained examples.
- Key Vault, JWT, API-key, Basic-authentication, and local Compose configurations still require the production hardening described in their category documentation.

## Excluded projects and remaining inconsistencies

No project is excluded from the root solution. Remaining inconsistencies are deliberate or externally blocked:

- Multi-target .NET 8 MAUI cannot build under the selected SDK because those workload targets are out of support; upgrading it was outside this pass.
- Three infrastructure package IDs are unavailable from configured feeds.
- Mechanism-suffixed host project names remain for comparison clarity and rename-risk control.
- Framework-specific package major versions remain separate.
- YARP requires an external downstream Hour Tracker API on port 7198.

## Validation

- `dotnet restore zenvera.dotnet-examples.slnx --configfile NuGet.config -p:NuGetAudit=false` ran and failed only at the documented MAUI support checks and unavailable `Zenvera.Shared.Queuing`, `Zenvera.Shared.Secrets`, and `Zenvera.Shared.ErrorHandling` packages. It also reported the documented `NU1603` version fallbacks.
- The restore initially identified duplicate package references injected by the nested CqrsMediatR build file. That file was removed; the subsequent build no longer reported `NU1504`.
- `dotnet build zenvera.dotnet-examples.slnx --no-restore -p:NuGetAudit=false` ran: 201 warnings and 26 errors. Errors were confined to the three unavailable infrastructure packages and the unsupported/missing .NET 8 MAUI workloads (`NETSDK1202`, `NETSDK1135`, and resulting Android reference errors).
- The earlier solution-filter validation results have been superseded by category `.slnx` validation; see the CI runbook for the current commands.
- Markdown relative-link scan completed with no broken links.
- Build outputs were removed again after validation so the repository finishes without generated `bin` or `obj` trees.
