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
