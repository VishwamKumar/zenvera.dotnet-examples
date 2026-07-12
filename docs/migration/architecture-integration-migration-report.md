# Architecture and integration migration report

Date: 2026-07-12

## Source resolution

The requested singular repository directories do not exist locally. The corresponding repositories identified during consolidation were used:

| Requested name | On-disk source used |
|---|---|
| `exp.grpc-api.clean-arch` | `D:\GitHub\VishwamKumar\example\exp.apis.clean-arch` |
| `exp.api-gateway.rest-apis` | `D:\GitHub\VishwamKumar\example\exp.api-gateways.rest-apis` |

Both source repositories were treated as read-only and remain in place.

## Part A — Clean Architecture migration

### Content migrated

Three complete variants were copied to `src/Architecture/CleanArchitecture/GrpcTodo`:

- `Simple`: Domain, Application, Infrastructure, gRPC API, tests, Dockerfile, settings, protos, migrations, and supporting code.
- `Cqrs`: Domain, Application, Infrastructure, gRPC API, tests, custom dispatcher, Dockerfile, settings, protos, migrations, and health-check code.
- `CqrsMediatR`: Domain, Application, Infrastructure, gRPC API, tests, MediatR handlers, Dockerfile, settings, protos, and migrations.

Generated databases, `bin`, `obj`, IDE state, nested repository metadata/configuration, child solutions, and nested licenses were excluded.

### Names and references

`Exp.TodoApp.*` directories, project files, namespaces, proto C# namespaces, project references, Docker paths, and test namespaces were normalized to:

- `Exp.Todo.Domain`
- `Exp.Todo.Application`
- `Exp.Todo.Infrastructure`
- `Exp.Todo.GrpcApi`
- `Exp.Todo.Tests`

The new root namespace `Exp.Todo` conflicts with the source domain type named `Todo` under C# name resolution. The entity and behavior were preserved; non-Domain projects now use an explicit `TodoEntity` alias for `Exp.Todo.Domain.Entities.Todo`. Proto contracts were not changed.

All variants retain their own layers and tests. No Application code, handlers, dispatcher code, or test fixtures were shared because independent completeness and comparison clarity outweigh the small duplication reduction.

The Simple Domain project remains `net9.0` while its other projects are `net10.0`, matching the source rather than silently upgrading it.

### Architecture documentation review

The copied variant READMEs were replaced with monorepo-correct commands and ports. The parent README and comparison matrix clarify that direct dependency injection is not itself a Clean Architecture violation. The reviewed criterion is inward dependency direction:

- API invokes Application policy/abstractions.
- Infrastructure implements Application/Domain-facing abstractions and may be wired by the outer composition root.
- Domain remains independent of gRPC, ASP.NET Core, EF Core, and Infrastructure.

CQRS and MediatR are organization/dispatch choices, not prerequisites for Clean Architecture.

### Docker assets

All three Dockerfiles were retained and repaired for renamed project paths and referenced-project restore. They expect their variant directory as Docker build context. Docker image builds were not requested or run in this validation.

### Test placement

Tests remain under each variant's `tests/Exp.Todo.Tests` directory as explicitly required. Moving them to the monorepo root would weaken the self-contained comparison and require less intuitive cross-tree references. They are discoverable through the root `.slnx` logical folders.

## Part B — API gateway migration

### Content migrated

- Ocelot -> `src/Integration/ApiGateway/RestApis/Ocelot/Exp.ApiGateway.Ocelot`
- YARP -> `src/Integration/ApiGateway/RestApis/Yarp/Exp.ApiGateway.Yarp`

Project files, assembly/root namespaces, C# namespaces, launch profiles, and documentation were normalized. Ocelot remains `net8.0`; YARP remains `net9.0`.

The gateway source contained no downstream service projects. Boundaries were preserved by retaining route/cluster configuration:

- Ocelot points to configured API Audit and Hour Tracker HTTPS services.
- YARP points to the separately run Hour Tracker API at `https://localhost:7198`.

No downstream implementation was invented or copied.

### Capability findings

- Routing exists in both gateways.
- Authentication is not validated by either gateway; standard proxy behavior may propagate client authorization headers.
- No explicit correlation-ID generation/validation is implemented.
- Ocelot rate-limit examples are commented out; effective rate limiting is absent.
- Retry and explicit timeout policies are absent.
- Ocelot has a local `CheckStatus` endpoint but no downstream health aggregation; YARP has no explicit health endpoint/active check configuration.
- Ocelot caching is enabled with route TTLs of zero/one second.
- YARP accepts any downstream server certificate for the local target; this is not production-safe.

These gaps are documented rather than silently filled with new behavior. The gateway README includes the required Mermaid flow.

## Root solution

The 15 Clean Architecture projects and two gateway projects were added to `zenvera.dotnet-examples.slnx`. Because each architecture variant intentionally uses the same five standard project filenames, `dotnet sln add` deduplicated by project identity/name. The `.slnx` was organized manually into distinct logical folders for Simple, Cqrs, and CqrsMediatR while preserving the requested physical/project names.

The root solution now contains 37 projects.

## Validation

### Restore

```powershell
dotnet restore zenvera.dotnet-examples.slnx --configfile NuGet.config -p:NuGetAudit=false
```

Result: **passed** for the full solution.

Restore reported source-era warnings in the CQRS/MediatR variant:

- `NU1504` duplicate package references in Application and Infrastructure.
- `NU1510` likely unnecessary Microsoft.Extensions package references in the gRPC host.

They were recorded rather than mixed with this migration's behavioral changes.

### Build

The first build revealed that `Exp.Todo` as a namespace shadowed the domain `Todo` type. Explicit aliases repaired the migration-induced name-resolution issue without changing the entity or proto contract.

Final complete-solution result: **passed**, 0 errors and 79 warnings. Warnings include the duplicate/redundant package items above plus existing nullable, analyzer, naming, cancellation-token, exception, and test-style findings across the repository.

Both gateway projects compiled as part of the successful root build. No gateway test project exists.

### Tests

| Variant | Passed | Failed | Skipped |
|---|---:|---:|---:|
| Simple | 27 | 0 | 0 |
| CQRS custom dispatcher | 5 | 0 | 0 |
| CQRS with MediatR | 5 | 0 | 0 |
| **Total** | **37** | **0** | **0** |

All three test projects passed with `--no-build --no-restore` after the successful solution build.

## Unresolved issues and runtime dependencies

1. Duplicate/redundant CQRS/MediatR package references should be cleaned in a separate dependency-focused change.
2. The Simple Domain project targets `net9.0` while the rest of that variant targets `net10.0`.
3. Source-era analyzer warnings remain and should be triaged without erasing comparison differences.
4. SQLite files are process-working-directory-relative; each variant is independent but can create multiple local files when run from different directories.
5. Dockerfiles were path-reviewed but images were not built.
6. Ocelot requires reachable external API Audit/Hour Tracker endpoints and may rely on stale public destinations.
7. YARP requires Hour Tracker on port 7198 and disables downstream certificate validation.
8. Gateway authentication, correlation IDs, enabled rate limiting, retries, explicit timeout budgets, and downstream health strategies are absent.
9. Neither gateway has automated tests.
10. Runtime smoke tests against downstream services were not possible without those separately managed services.

## Documentation created or updated

- `src/Architecture/CleanArchitecture/GrpcTodo/README.md`
- per-variant READMEs under `Simple`, `Cqrs`, and `CqrsMediatR`
- `docs/comparison-matrices/clean-architecture-options.md`
- `src/Integration/ApiGateway/RestApis/README.md`
- this migration report

## Source repositories retained

- `D:\GitHub\VishwamKumar\example\exp.apis.clean-arch`
- `D:\GitHub\VishwamKumar\example\exp.api-gateways.rest-apis`

No source repository file was intentionally changed.
