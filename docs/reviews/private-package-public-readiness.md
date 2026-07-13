# Private package public-readiness review

Date: 2026-07-12  
Scope: static, analysis-first review; no package or build behavior was changed.

## Executive assessment

The repository is **not currently independently restorable by a public visitor**. The authoritative root solution contains five projects with six direct references to private `Zenvera.Shared.*` packages. The infrastructure category solution contains the same projects. Both CI workflows inject GitHub Packages credentials and build all five projects, so the advertised green build is credential-dependent.

The other category solutions have no `Zenvera.Shared.*` reference, but the repository's only `NuGet.config` clears inherited sources and adds an authenticated GitHub Packages source globally. Consequently, a credential-free restore using the documented repository configuration is not reliably independent of that private endpoint even for otherwise-public categories. They are dependency-clean but feed-coupled.

Recommended strategy: use **Option 2**, replacing the private convenience wrappers in these five examples with small, example-local integrations built on public vendor packages and framework abstractions. The examples are about Redis, RabbitMQ, Azure Key Vault, and Serilog—not the Zenvera wrapper API—so showing the actual integration improves rather than weakens their learning objective. Remove the private feed and CI credential plumbing after all five migrations. Do not copy the production shared libraries.

## Inventory of private dependencies

All versions below come from the authoritative project files. Every affected project is in both `zenvera.dotnet-examples.slnx` and `solutions/zenvera.infrastructure.slnx`, is `buildEnabled: true` in `build/example-build-manifest.json`, and is restored, built, and formatting-checked by the infrastructure CI matrix. None has an enabled test project.

| Project | Package/version | Why it is used | Essential to example? | Build without it now? | Root solution | CI | Classification |
|---|---|---|---|---|---|---|---|
| `src/Infrastructure/Caching/Redis/Exp.Weather.RestApi.RedisCaching/Exp.Weather.RestApi.RedisCaching.csproj` | `Zenvera.Shared.Caching` 1.0.19 | Supplies `AddCachingService` and `ICacheService`; controller gets/sets the weather response in Redis. | Redis caching is essential; this wrapper is not. | No. Its types and registration extension are compile-time dependencies. | Yes | Yes | **B**—replaceable convenience dependency. |
| `src/Infrastructure/Messaging/RabbitMq/Exp.Weather.RestApi.RabbitMq/Exp.Weather.RestApi.RabbitMq.csproj` | `Zenvera.Shared.Queuing` 1.0.19 | Supplies `AddRabbitMqService` and `IRabbitMqService`; controller publishes and consumes serialized weather data. | RabbitMQ messaging is essential; this wrapper is not. | No. Its service contract and registration extension are compile-time dependencies. | Yes | Yes | **B**—replaceable convenience dependency. |
| `src/Infrastructure/Secrets/AzureKeyVault/Exp.Weather.BlazorServer.KeyVault/Exp.Weather.BlazorServer.KeyVault.csproj` | `Zenvera.Shared.Secrets` 1.0.21 | Supplies `AddSecretsService`, which wires secret/Key Vault behavior into the Blazor app. | Key Vault integration is essential; this wrapper is not. | No. The registration extension is a compile-time dependency. | Yes | Yes | **B**—replaceable convenience dependency. |
| `src/Infrastructure/Logging/Serilog/Exp.Weather.RestApi.Serilog/Exp.Weather.RestApi.Serilog.csproj` | `Zenvera.Shared.Logging` 1.0.19 | Supplies `AddCustomLogging`, `ILogTypeLogger<T>`, and `LogType`; the controller demonstrates typed application/transaction logging and sensitive-data handling. | Structured Serilog behavior is essential; the wrapper API is not. | No. Registration, logger contract, and enum are compile-time dependencies. | Yes | Yes | **B**—replaceable convenience dependency. |
| `src/Infrastructure/Logging/Serilog/Exp.Weather.BlazorServer.Serilog/Exp.Weather.BlazorServer.Serilog.csproj` | `Zenvera.Shared.Logging` 1.0.19 | Supplies `AddCustomLogging` for the Blazor host. | Serilog is essential; the wrapper is not. | No. Registration is a compile-time dependency. | Yes | Yes | **B**—replaceable convenience dependency. |
| Same Blazor Serilog project | `Zenvera.Shared.ErrorHandling` 1.0.19 | No package namespace or package-specific API is referenced in tracked source. The app uses ASP.NET Core's built-in `UseExceptionHandler`. It may be transitively/implicitly expected, but static evidence shows no direct use. | No demonstrated role in the learning objective. | Not as the project is authored because restore must resolve every direct reference; source likely compiles after removal, subject to a build check. | Yes | Yes | **B**—apparently unused convenience dependency. |

No dependency qualifies as **A**: the private package itself is not the architectural subject. The packages resemble **C** cross-cutting production libraries and public publication is a viable organizational choice, but that classification is not the best fit for these samples because their abstractions hide the technology being taught. No example should be classified **D** as the target design: all five subjects belong in the public infrastructure catalog and can be made public-buildable without duplicating large libraries.

### Manifest drift

`build/example-build-manifest.json` does not accurately inventory the project references:

- caching says `0.0.25 resolves to 1.0.17`, but the project requests 1.0.19;
- queuing says 0.0.10, but the project requests 1.0.19;
- Blazor logging mentions only `Zenvera.Shared.ErrorHandling` 1.0.6, while the project requests both ErrorHandling and Logging 1.0.19;
- the REST logging entry omits a version;
- the secrets entry correctly says 1.0.21.

The manifest validator confirms schema/membership rules, not the truth of free-text `externalDependencies`, so it passes despite this drift.

## Feed, credential, configuration, and source-repository inventory

| Location | Finding | Public impact |
|---|---|---|
| `NuGet.config` | Clears inherited sources, adds nuget.org, and adds `https://nuget.pkg.github.com/VishwamKumar/index.json` as `github-packages`. It contains no credential. | Makes a private endpoint part of the default restore configuration for the entire repository. |
| `.github/workflows/build-examples.yml` | Grants `packages: read`; creates `NuGetPackageSourceCredentials_github-packages` from `GH_PACKAGES_READ_TOKEN`, then `GH_PACKAGES_TOKEN`, then `GITHUB_TOKEN`, with an optional package user. | Scheduled/main builds explicitly depend on package authentication; fallback success depends on package/repository permissions. |
| `.github/workflows/pr-validation.yml` | Uses the same permission and credential construction in both jobs, although repository checks do not restore packages. | PR validation is credential-aware and fork PR secrets are normally unavailable; the package-read fallback may not reach packages associated with another private repository. |
| `scripts/ci/Invoke-ExampleValidation.ps1` | Every restore explicitly uses repository `NuGet.config`; infrastructure entries are not excluded. | CI necessarily includes the private source and private packages. |
| `docs/runbooks/ci-validation.md` | Tells local users to create a user-scoped package credential and states infrastructure examples remain enabled. | Confirms private access is an expected prerequisite, contrary to public self-containment. |
| `README.md`, `docs/architecture/hld/hld.md`, and affected example READMEs | Disclose the private-package prerequisite. | Honest documentation, but documentation does not make clone/restore/build public-ready. |
| `docs/architecture/lld/rabbitmq-flow.md` and `docs/comparison-matrices/infrastructure-patterns.md` | Describe the private queuing/secrets abstractions. | Learning material is coupled to non-public APIs. |

No GitHub Packages credential or token value is stored in tracked files. No explicit URL for a private source-code repository was found. The package owner/feed implies the `VishwamKumar` GitHub namespace, but the packages' source repositories cannot be identified from tracked repository metadata; the only explicit repository URL is this public repository. Package contents and metadata were not downloaded or inspected.

## Affected examples and public-build impact

The root solution includes all five projects, so a fresh public restore cannot complete from public packages alone. `solutions/zenvera.infrastructure.slnx` has the same blocker. CI does not build the root or category solution directly; it uses the manifest to restore/build each enabled project. All five affected entries are enabled, making the infrastructure matrix credential-dependent.

Category status under the checked-in configuration:

| Solution | Private package references | Credential-free independent restore verdict |
|---|---:|---|
| `zenvera.dotnet-examples.slnx` | 6 | **No**. Contains all affected projects. |
| `solutions/zenvera.infrastructure.slnx` | 6 | **No**. Contains all affected projects. |
| `solutions/zenvera.api-styles.slnx` | 0 | **Not reliably independent as configured**. Dependencies are public, but the global config still includes the authenticated private source. |
| `solutions/zenvera.architecture.slnx` | 0 | **Not reliably independent as configured**, for the same feed-level reason. |
| `solutions/zenvera.authentication.slnx` | 0 | **Not reliably independent as configured**, for the same feed-level reason. |
| `solutions/zenvera.integration.slnx` | 0 | **Not reliably independent as configured**, for the same feed-level reason. |
| `solutions/zenvera.user-interface.slnx` | 0 | Private-package independent, but full restore/build also depends on installed MAUI workloads/platform support; it is already an explicit CI exclusion. The private source remains configured globally. |

This is a static public-readiness verdict, deliberately not a cache-dependent success claim. A restore on a maintainer machine could pass because credentials or packages already exist locally; that would not demonstrate that a new public visitor can restore.

## Options considered

### Option 1: Publish the shared packages publicly

This produces the smallest consumer-code change and preserves current APIs. It also creates a public versioning, support, provenance, vulnerability-management, and source-availability obligation for several production-oriented cross-cutting libraries. It is appropriate only if Zenvera intends those packages to be supported public products. It continues to hide Redis/RabbitMQ/Key Vault/Serilog mechanics behind custom wrappers, so it is not the preferred sample design.

### Option 2: Replace package dependencies with small local implementations

Preferred. Use official/public packages and small code colocated with each example. Keep implementations narrow: only the behavior demonstrated by the example, not copies of the shared production libraries. This removes private infrastructure, makes the teaching mechanism visible, and lets root and infrastructure restores use public sources only.

### Option 3: Add public fallback abstractions

Conditional package references or duplicate fallback interfaces would preserve private-package paths while adding public paths. That creates two behaviors to document and test, risks drift, and makes examples harder to read. It is useful only if demonstrating pluggable providers is itself a learning objective; it is not here.

### Option 4: Exclude private-package projects from default solution and CI

This can be a short-lived containment step if migrations cannot land together. Exclusions must be explicit in the root solution, manifest (`buildEnabled: false` with a precise reason), CI notices, catalog, and README. As a target state it removes all infrastructure examples except the background-service example from the green public build and substantially weakens repository coverage.

### Option 5: Keep current design with explicit documentation

The repository already follows this option. It is transparent but fails the primary decision principle: public users and CI still need private infrastructure. A green badge remains evidence of maintainer credentials, not public reproducibility.

## Recommended target design

Keep every affected example in the root and infrastructure solutions. Replace each private wrapper with the smallest direct public integration:

- Redis: a tiny example-local cache contract/implementation over a public Redis/distributed-cache package, or use `IDistributedCache` directly when typed serialization is shown locally.
- RabbitMQ: a narrow publisher/consumer service over the public RabbitMQ client, with connection/channel lifetime and cancellation made explicit.
- Azure Key Vault: use public Azure SDK/configuration packages and `DefaultAzureCredential`; keep the existing runtime opt-out for local exploration.
- Serilog: configure public Serilog packages directly and use `ILogger<T>` or a very small local category abstraction only if application/transaction categories are central to the lesson.
- Error handling: remove the unused direct package if an implementation build confirms no hidden generated dependency; retain ASP.NET Core's built-in exception handler.

After migration, make `NuGet.config` public-only (or remove it if defaults suffice), delete GitHub Packages credential setup and `packages: read` from CI, and add a credential-free clean-cache restore check. Root restore, category restore, manifest validation, builds, tests, and format checks should all run without package secrets.

## Staged implementation plan

1. Add a public-readiness CI probe that restores with an empty package cache and a public-only source configuration. Initially allow it to document failure; do not label the root build public-ready yet.
2. Migrate the Redis and Key Vault examples to direct public integrations; add focused tests for serialization/configuration and disabled external-service behavior.
3. Migrate RabbitMQ with explicit resource lifetime and cancellation behavior; add unit-level tests that do not require RabbitMQ and keep runtime integration instructions.
4. Migrate both Serilog examples together so their logging conventions remain consistent; verify sinks and sensitive-data examples do not imply unsafe production practice.
5. Remove the apparently unused ErrorHandling reference after a clean build proves it unnecessary.
6. Correct manifest dependency descriptions and update example READMEs, architecture diagrams/text, comparison matrix, catalog/root README, and CI runbook.
7. Remove the private source and both workflow credential blocks/`packages: read`. Run clean-cache restores for the root and every category, then build/test through the manifest on the same public-only configuration.
8. If any migration cannot be completed atomically, temporarily apply Option 4 only to that project with a visible manifest exclusion and documentation; remove the exclusion before declaring public readiness.

## Risks

- Reimplementing too much would create unsupported copies of production libraries. Keep code example-sized and technology-specific.
- RabbitMQ connection/channel handling and Redis serialization/expiration semantics can regress if reduced too aggressively.
- Direct public SDKs can expose more configuration complexity; documentation must preserve a short happy path.
- Logging behavior (category routing, sinks, enrichment, and redaction) may differ from the shared wrapper and needs explicit acceptance criteria.
- Key Vault must remain buildable and locally explorable without live Azure credentials while failing safely when enabled incorrectly.
- Removing the feed can reveal accidental private transitive dependencies; clean-cache/public-only validation is required.
- The MAUI category has a separate workload/platform limitation and must not be misreported as solved by this work.

## Exact files requiring modification for the recommended strategy

Package/feed/CI governance:

- `NuGet.config`
- `.github/workflows/build-examples.yml`
- `.github/workflows/pr-validation.yml`
- `build/example-build-manifest.json`
- `scripts/ci/Invoke-ExampleValidation.ps1` (only if adding/enforcing the public-only clean-cache restore mode here; otherwise add a dedicated validation script)
- `scripts/ci/Test-RepositoryHygiene.ps1` (if the private-feed prohibition is made a hygiene invariant)

Redis example:

- `src/Infrastructure/Caching/Redis/Exp.Weather.RestApi.RedisCaching/Exp.Weather.RestApi.RedisCaching.csproj`
- `src/Infrastructure/Caching/Redis/Exp.Weather.RestApi.RedisCaching/GlobalUsings.cs`
- `src/Infrastructure/Caching/Redis/Exp.Weather.RestApi.RedisCaching/Program.cs`
- `src/Infrastructure/Caching/Redis/Exp.Weather.RestApi.RedisCaching/Controllers/WeatherForecastController.cs`
- `src/Infrastructure/Caching/Redis/Exp.Weather.RestApi.RedisCaching/appsettings.json`
- `src/Infrastructure/Caching/Redis/Exp.Weather.RestApi.RedisCaching/README.md`
- new example-local cache implementation/test files as designed

RabbitMQ example:

- `src/Infrastructure/Messaging/RabbitMq/Exp.Weather.RestApi.RabbitMq/Exp.Weather.RestApi.RabbitMq.csproj`
- `src/Infrastructure/Messaging/RabbitMq/Exp.Weather.RestApi.RabbitMq/GlobalUsings.cs`
- `src/Infrastructure/Messaging/RabbitMq/Exp.Weather.RestApi.RabbitMq/Program.cs`
- `src/Infrastructure/Messaging/RabbitMq/Exp.Weather.RestApi.RabbitMq/Controllers/WeatherForecastController.cs`
- `src/Infrastructure/Messaging/RabbitMq/Exp.Weather.RestApi.RabbitMq/appsettings.json`
- `src/Infrastructure/Messaging/RabbitMq/Exp.Weather.RestApi.RabbitMq/README.md`
- new example-local messaging implementation/test files as designed

Key Vault example:

- `src/Infrastructure/Secrets/AzureKeyVault/Exp.Weather.BlazorServer.KeyVault/Exp.Weather.BlazorServer.KeyVault.csproj`
- `src/Infrastructure/Secrets/AzureKeyVault/Exp.Weather.BlazorServer.KeyVault/GlobalUsings.cs`
- `src/Infrastructure/Secrets/AzureKeyVault/Exp.Weather.BlazorServer.KeyVault/Program.cs`
- `src/Infrastructure/Secrets/AzureKeyVault/Exp.Weather.BlazorServer.KeyVault/appsettings.json`
- `src/Infrastructure/Secrets/AzureKeyVault/Exp.Weather.BlazorServer.KeyVault/README.md`
- new example-local configuration/test files as designed

Serilog examples:

- `src/Infrastructure/Logging/Serilog/Exp.Weather.RestApi.Serilog/Exp.Weather.RestApi.Serilog.csproj`
- `src/Infrastructure/Logging/Serilog/Exp.Weather.RestApi.Serilog/GlobalUsings.cs`
- `src/Infrastructure/Logging/Serilog/Exp.Weather.RestApi.Serilog/Extensions/ServiceExtensions.cs`
- `src/Infrastructure/Logging/Serilog/Exp.Weather.RestApi.Serilog/Controllers/WeatherForecastController.cs`
- `src/Infrastructure/Logging/Serilog/Exp.Weather.RestApi.Serilog/appsettings.json`
- `src/Infrastructure/Logging/Serilog/Exp.Weather.RestApi.Serilog/README.md`
- `src/Infrastructure/Logging/Serilog/Exp.Weather.BlazorServer.Serilog/Exp.Weather.BlazorServer.Serilog.csproj`
- `src/Infrastructure/Logging/Serilog/Exp.Weather.BlazorServer.Serilog/GlobalUsings.cs`
- `src/Infrastructure/Logging/Serilog/Exp.Weather.BlazorServer.Serilog/Extensions/ServiceExtensions.cs`
- `src/Infrastructure/Logging/Serilog/Exp.Weather.BlazorServer.Serilog/appsettings.json`
- `src/Infrastructure/Logging/Serilog/Exp.Weather.BlazorServer.Serilog/README.md`
- new example-local logging configuration/test files as designed

Repository documentation:

- `README.md`
- `docs/runbooks/ci-validation.md`
- `docs/architecture/hld/hld.md`
- `docs/architecture/lld/rabbitmq-flow.md`
- `docs/comparison-matrices/infrastructure-patterns.md`
- `docs/catalog.md` if its prerequisites/build claims are updated

The solution files do **not** need modification for the preferred target design because the examples remain included. They would need modification only for the temporary Option 4 containment path: `zenvera.dotnet-examples.slnx` and `solutions/zenvera.infrastructure.slnx`.

## Static validation performed

- `scripts/ci/Test-ExampleBuildManifest.ps1`: passed; 31 examples across 6 categories, with only MAUI explicitly excluded.
- `scripts/ci/Test-RepositoryHygiene.ps1`: passed; 742 files, 81 Markdown files, and 1 Compose file checked.
- `dotnet sln <solution> list`: succeeded for the root and all six category solutions; confirmed the membership stated above.
- Repository-wide text/reference scan excluding `.git`, IDE state, vendored web assets, and source maps: found the six project references, one private feed, workflow/runbook credential wiring, and documentation references described above; found no stored credential and no explicit private source-repository URL.

Final static verdict:

- Public root solution independently restorable: **No**.
- Public category solutions independently restorable: **Infrastructure: no. Other categories: no private package dependency, but not reliably private-feed-independent under the checked-in global configuration; MAUI also has its separately documented workload constraint.**
- CI depends on private credentials: **Yes**, specifically for the enabled infrastructure examples; both workflows nevertheless configure credentials more broadly than needed.

