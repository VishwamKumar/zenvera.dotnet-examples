# .NET 10 and package upgrade report

## Scope

All 44 maintained projects were retargeted to .NET 10. Web, library, worker, and test projects now target `net10.0`; the MAUI client targets the .NET 10 Android, iOS, Mac Catalyst, and Windows TFMs. `global.json` continues to select SDK 10.0.301.

Public package references were compared with the stable versions published by NuGet.org and upgraded without redesigning the examples. Major lines updated include ASP.NET Core and EF Core 10.0.9, Hot Chocolate 16.4.0, gRPC 2.80.0/Grpc.Tools 2.82.0, Swashbuckle 10.2.3, FastEndpoints 8.2.0, MediatR 14.2.0, Ocelot 24.1.0, and .NET MAUI 10.0.80. Exact versions remain in project files because central package management is still opt-in.

## Private GitHub Packages feed

`NuGet.config` defines the package source at `https://nuget.pkg.github.com/VishwamKumar/index.json` and deliberately contains no credentials. Local restores use `NuGetPackageSourceCredentials_github-packages`; GitHub Actions creates that variable from `GH_PACKAGES_READ_TOKEN`, `GH_PACKAGES_TOKEN`, or the workflow `GITHUB_TOKEN`. Setup is documented in the [CI validation runbook](../runbooks/ci-validation.md).

The exact latest private versions could not be enumerated in this environment because no GitHub Packages token was available. The migration therefore uses the latest locally verified versions for `Zenvera.Shared.Caching` and `Zenvera.Shared.Logging` (1.0.19), retains `Zenvera.Shared.Secrets` 1.0.21, `Zenvera.Shared.ErrorHandling` 1.0.6, and `Zenvera.Shared.Queuing` 0.0.10, and leaves their explicit versions visible for authenticated verification.

## Compatibility repairs

- FastEndpoints 8 response helpers now operate through `HttpContext.Response`; routes and response contracts are unchanged.
- Swashbuckle/OpenAPI 2 security references use the document-aware callback required by Swashbuckle 10.
- gRPC Swagger examples retain the `Microsoft.OpenApi.Models` namespace supplied by their 1.x transitive dependency, while Swashbuckle 10 hosts use the 2.x root namespace.

## Validation

| Scope | Result |
|---|---|
| API styles | Restore and build passed; the former transitive SQLite advisory has been remediated |
| Authentication | Restore and build passed after the OpenAPI compatibility repair |
| Architecture | Restore and build passed; 37/37 tests passed |
| Integration | Restore and build passed with two non-blocking analyzer/package-pruning warnings |
| Infrastructure | Authenticated restore not run because no GitHub Packages credential was available |
| User interface | Restore passed and the Android assembly compiled; the full multi-target build hit a local MSBuild task-host failure and requires workload-host/CI confirmation |

After the framework migration, the EF Core SQLite 10.0.9 graph was explicitly upgraded to `SQLitePCLRaw.bundle_e_sqlite3` 3.0.3 at the shared Todo and three Clean Architecture infrastructure projects. The resolved graph now uses `SourceGear.sqlite3` 3.50.4.5 plus SQLitePCLRaw configuration/provider 3.0.3 and no longer resolves vulnerable `SQLitePCLRaw.lib.e_sqlite3` 2.1.11. API-style builds and all 37 Clean Architecture tests passed with this override.
