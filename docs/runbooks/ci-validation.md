# CI validation runbook

## Purpose

GitHub Actions validates repository hygiene and every build-enabled catalog example without starting unrelated external services. `build/example-build-manifest.json` is the source of truth for build/test participation and tracked exclusions.

## Workflows

- `.github/workflows/pr-validation.yml` runs manifest, hygiene, secret-pattern, Markdown-link, Compose, restore, build, test, and non-rewriting format checks on pull requests.
- `.github/workflows/build-examples.yml` runs the six-category build matrix on `main`, manual dispatch, and a weekly schedule.
- Infrastructure-dependent integration tests are a separate, currently disabled job because no manifest entry declares such an automated test. Compile validation never starts Redis, RabbitMQ, SQL Server, or MongoDB.

## Category solutions

| Category | Solution |
|---|---|
| API styles | `solutions/zenvera.api-styles.slnx` |
| Authentication | `solutions/zenvera.authentication.slnx` |
| Architecture | `solutions/zenvera.architecture.slnx` |
| Integration | `solutions/zenvera.integration.slnx` |
| Infrastructure | `solutions/zenvera.infrastructure.slnx` |
| User interface | `solutions/zenvera.user-interface.slnx` |

The manifest validator checks that every catalog entry belongs to its category solution. The root `zenvera.dotnet-examples.slnx` remains authoritative.

## Local equivalents

Run from the repository root with PowerShell 7 and the SDK selected by `global.json`:

For private `Zenvera.Shared.*` packages, set a user-scoped NuGet credential environment variable once. Use a classic GitHub token with `read:packages` and repository access; never place it in `NuGet.config`:

```powershell
$credential = "Username=<github-user>;Password=<read-packages-token>;ValidAuthenticationTypes=Basic"
[Environment]::SetEnvironmentVariable(
  "NuGetPackageSourceCredentials_github-packages",
  $credential,
  [EnvironmentVariableTarget]::User)
```

Open a new terminal afterward. GitHub Actions creates the same variable from `GH_PACKAGES_READ_TOKEN`, then `GH_PACKAGES_TOKEN`, falling back to the workflow `GITHUB_TOKEN`.

```powershell
./scripts/ci/Test-ExampleBuildManifest.ps1
./scripts/ci/Test-RepositoryHygiene.ps1

./scripts/ci/Invoke-ExampleValidation.ps1 -Category all -Action restore
./scripts/ci/Invoke-ExampleValidation.ps1 -Category all -Action build
./scripts/ci/Invoke-ExampleValidation.ps1 -Category all -Action test
./scripts/ci/Invoke-ExampleValidation.ps1 -Category all -Action format
```

Run one category while iterating:

```powershell
./scripts/ci/Invoke-ExampleValidation.ps1 -Category api-styles -Action restore
./scripts/ci/Invoke-ExampleValidation.ps1 -Category api-styles -Action build
./scripts/ci/Invoke-ExampleValidation.ps1 -Category api-styles -Action test
./scripts/ci/Invoke-ExampleValidation.ps1 -Category api-styles -Action format
```

Formatting uses `dotnet format whitespace --verify-no-changes`; it reports differences and never rewrites files.

## Tracked exclusions

An example may be disabled only when `exclusionReason` is non-empty. The MAUI client is the sole tracked exclusion because its Android, iOS, Mac Catalyst, and Windows targets require platform workloads and compatible macOS/Windows hosts that the Linux category runner cannot provide. Infrastructure examples remain enabled and restore from the authenticated GitHub Packages feed.

Runtime-only dependencies do not disable compilation. Firebase, certificates, databases, gateways, Redis, and logging sinks are required only for their documented runtime or integration scenarios.

## Adding or changing an example

1. Add the project to the root and category `.slnx` files.
2. Add or update its manifest entry, framework, dependencies, and test path.
3. Keep `buildEnabled` true unless a reproducible blocker remains after a reasonable non-material repair.
4. Document any exclusion precisely; never hide it in workflow YAML.
5. Run all four manifest actions for the affected category plus repository hygiene.

## Latest local validation

Validated during the .NET 10 upgrade on Windows with SDK 10.0.301:

- Manifest: 31 examples across six category solutions; one host-specific MAUI exclusion.
- Repository hygiene: secret patterns, generated-file paths, 84 internal Markdown documents, and one Compose file; passed.
- Category solutions: all six parsed and listed successfully with `dotnet sln`.
- Restore/build: API styles, authentication, architecture, and integration passed on .NET 10. Infrastructure requires the private-feed credential. MAUI restored and compiled its .NET 10 Android assembly locally; the complete platform build hit a local MSBuild task-host failure and remains for CI/workload-host confirmation.
- Tests: Simple 27/27, CQRS 5/5, and CQRS with MediatR 5/5 passed (37 total).
- Formatting: run the manifest-driven format check after authenticating the private feed so every enabled project can restore consistently.
- Docker Compose: `deploy/local/compose.yml` passed `docker compose config --quiet` without starting services.

The former `SQLitePCLRaw.lib.e_sqlite3` 2.1.11 advisory is remediated by an explicit `SQLitePCLRaw.bundle_e_sqlite3` 3.0.3 dependency at each SQLite infrastructure boundary. The resolved graph uses `SourceGear.sqlite3` 3.50.4.5 and no longer contains the vulnerable legacy native package. GitHub-hosted runners remain the authoritative check for workflow-specific behavior.
