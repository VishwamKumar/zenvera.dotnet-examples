# API styles migration report

Date: 2026-07-12

## Scope

This migration copied the API-style examples from the following local source repositories into `zenvera.dotnet-examples`:

- `exp.rest-apis.styles`
- `exp.grpc-apis.styles`
- `exp.graphql-apis.styles`
- `exp.soap-apis.styles`

All four source repositories remain in their original locations and were not modified or deleted.

## Files migrated

| Source content | Target content |
|---|---|
| REST Minimal host, DTOs, mappings, configuration, launch profile, and project docs | `src/ApiStyles/Rest/Exp.Todo.RestApi.Minimal` |
| REST MVC host, controller, DTOs, mappings, configuration, launch profile, LibMan file, and project docs | `src/ApiStyles/Rest/Exp.Todo.RestApi.MvcControllers` |
| REST built-in endpoint host, endpoint file, DTOs, mappings, configuration, launch profile, and project docs | `src/ApiStyles/Rest/Exp.Todo.RestApi.EndpointPerFile` |
| REST FastEndpoints host, endpoint files, DTOs, mappings, configuration, launch profile, and project docs | `src/ApiStyles/Rest/Exp.Todo.RestApi.FastEndpoints` |
| Native gRPC host, proto, service, mappings, configuration, launch profile, and project docs | `src/ApiStyles/Grpc/Exp.Todo.GrpcApi.Native` |
| Transcoded gRPC host, Todo/Google protos, service, mappings, configuration, launch profile, and project docs | `src/ApiStyles/Grpc/Exp.Todo.GrpcApi.Transcoding` |
| Hot Chocolate host, schema, DTOs, mappings, configuration, launch profile, and query examples | `src/ApiStyles/GraphQL/Exp.Todo.GraphQLApi` |
| CoreWCF host, data/service contracts, implementation, mappings, configuration, launch profile, and SOAP examples | `src/ApiStyles/Soap/Exp.Todo.SoapApi` |
| Shared Todo model, service contract/implementation, EF Core DbContext, and SQLite migrations | `shared/Todo/Exp.Todo.Infrastructure.Sqlite` |

Nested `.git`, `.gitignore`, `.gitattributes`, licenses, `.sln` files, generated databases, `bin`, `obj`, `.vs`, `.idea`, secrets, and user files were excluded from the import. Docker and workflow assets were not present in these source projects.

## Renames and reference updates

| Source project | Migrated project |
|---|---|
| `ToDoApp.RestApi.Minimal` | `Exp.Todo.RestApi.Minimal` |
| `ToDoApp.RestApi.MvcControllers` | `Exp.Todo.RestApi.MvcControllers` |
| `ToDoApp.RestApi.Endpoints` | `Exp.Todo.RestApi.EndpointPerFile` |
| `ToDoApp.RestApi.FastEndpoints` | `Exp.Todo.RestApi.FastEndpoints` |
| `ToDoApp.GrpcApi.Native` | `Exp.Todo.GrpcApi.Native` |
| `ToDoApp.GrpcApi.Transcoding` | `Exp.Todo.GrpcApi.Transcoding` |
| `ToDoApp.GraphQLApi.HotChocolate` | `Exp.Todo.GraphQLApi` |
| `ToDoApp.SoapApi.CoreWcf` | `Exp.Todo.SoapApi` |
| `ToDoApp.Data.Sqlite` | `Exp.Todo.Infrastructure.Sqlite` |

Project filenames, assembly names, root namespaces, explicit C# namespaces/usings, generated-code namespaces, launch profile names, and project references were updated to the migrated names. Public REST routes, proto message/service definitions, GraphQL schema behavior, and SOAP contracts were not intentionally changed.

All hosts now reference the shared project through a root-relative project layout. SQLite configuration was repaired and made independently runnable with `Data Source=todo.db`; generated database files are ignored. Launch profiles use distinct ports:

- REST: 5101/7101 through 5104/7104
- gRPC: 5201/7201 and 5202/7202
- GraphQL: 5301/7301
- SOAP: 5401/7401

## Shared code decision

The four source repositories each contained a `ToDoApp.Data.Sqlite` project. Hash comparison showed that the model, interface, DbContext, migrations, project metadata, and all behavior were materially identical. The only `ToDoService.cs` difference was whitespace formatting in the REST copy. One implementation was therefore migrated as `shared/Todo/Exp.Todo.Infrastructure.Sqlite` and is used by all eight hosts.

The shared code was not split into separate Domain and Contracts projects because the migrated sources did not contain independently versioned or behaviorally distinct layers. Creating extra projects now would add architecture that the focused examples did not previously have. Host DTOs, REST handlers, gRPC protos, GraphQL schema types, and SOAP contracts remain local because similar names alone are not a sufficient sharing boundary.

## Solution and package restore

All nine migrated projects are included in the root `zenvera.dotnet-examples.slnx`.

`NuGet.config` was added at the repository root to clear machine-specific private feeds and use nuget.org for these public example dependencies. Validation restore command:

```powershell
dotnet restore zenvera.dotnet-examples.slnx --configfile NuGet.config -p:NuGetAudit=false
```

Result: **passed**, 9 of 9 projects restored.

The initial audited restore reported the historical AutoMapper advisory. The subsequent .NET 10 package upgrade moved AutoMapper to 16.2.0; the old vulnerable version is no longer resolved.

## Build results

The complete solution build passed with 0 errors and 69 warnings. Warnings include the AutoMapper advisory, existing analyzer findings (`CA1725`, `CA1848`, `CA2254`, `CA2016`, and `CA2201`), and an unused timestamp proto import. Implementations were not stylistically rewritten to suppress source-era analyzer warnings.

Each project was then built individually with the restored assets and audit network calls disabled:

| Project | Result |
|---|---|
| `Exp.Todo.Infrastructure.Sqlite` | Passed |
| `Exp.Todo.RestApi.Minimal` | Passed |
| `Exp.Todo.RestApi.MvcControllers` | Passed |
| `Exp.Todo.RestApi.EndpointPerFile` | Passed |
| `Exp.Todo.RestApi.FastEndpoints` | Passed |
| `Exp.Todo.GrpcApi.Native` | Passed |
| `Exp.Todo.GrpcApi.Transcoding` | Passed |
| `Exp.Todo.GraphQLApi` | Passed |
| `Exp.Todo.SoapApi` | Passed |

The first sandboxed individual-build loop was not a code failure: NuGet audit attempted blocked network access and consulted user-level private feeds despite `--no-restore`. Explicit repository restore plus `-p:NuGetAudit=false` produced the definitive results above.

## Tests

No test project exists in any of the four source repositories, so there were no available automated tests to migrate or run. Build validation does not replace future HTTP, gRPC, GraphQL, SOAP, persistence, or contract tests.

## External services and tools

No migrated host requires an external service at runtime; all use local SQLite. Optional client/tool dependencies are:

- REST: browser, Swagger UI, or an HTTP client.
- Native gRPC: a generated client, `grpcurl`, or another gRPC client.
- gRPC transcoding: a gRPC client or HTTP/Swagger client.
- GraphQL: the hosted GraphQL IDE or another GraphQL client.
- SOAP: a WSDL/SOAP-capable client.
- HTTPS profiles: a trusted ASP.NET Core development certificate.

## Unresolved issues

1. Resolved: AutoMapper was upgraded to 16.2.0 during the repository-wide .NET 10 package pass.
2. Existing analyzer warnings remain and should be triaged without obscuring the style comparisons.
3. There are no automated tests or runtime smoke tests.
4. The database path is process-working-directory-relative. This keeps each example independently runnable but can create more than one `todo.db` when invoked from different directories.
5. Checked-in Google API protos remain in the transcoding project to preserve its contract/build. Their future replacement with package-provided assets requires contract validation.
6. The installed .NET 10.0.301 CLI rejected an `api-styles.slnf` that referenced the root `.slnx`, reporting that listed projects were not in the solution. No invalid filter was retained. The root `.slnx` is the supported entry point until `.slnx`/`.slnf` tooling compatibility is resolved.
7. Per-project source READMEs were retained for historical context and mechanically updated for renamed identities; category READMEs are authoritative for monorepo run commands and ports.

## Source repositories retained

The original repositories under `D:\GitHub\VishwamKumar\example` remain intact:

- `D:\GitHub\VishwamKumar\example\exp.rest-apis.styles`
- `D:\GitHub\VishwamKumar\example\exp.grpc-apis.styles`
- `D:\GitHub\VishwamKumar\example\exp.graphql-apis.styles`
- `D:\GitHub\VishwamKumar\example\exp.soap-apis.styles`
