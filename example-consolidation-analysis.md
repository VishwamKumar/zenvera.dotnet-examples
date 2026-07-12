# Zenvera .NET Examples Consolidation Assessment

## 1. Executive summary

This assessment covers all 11 directories whose names begin with `exp.` under `D:\GitHub\VishwamKumar\example`. The intended destination is the separate `zenvera.dotnet-examples` workspace. The source repositories were inspected read-only; no source repository was changed.

The collection is a good fit for a reference-example monorepo. Ten repositories are deliberately small, focused or layered demonstrations and should remain that way. `exp.apis.clean-arch` is the sole full Clean Architecture reference and should retain Domain, Application, Infrastructure, gRPC host, and test separation. The strongest consolidation opportunity is the byte-identical `ToDoApp.Data.Sqlite` project copied across the REST, gRPC, GraphQL, and SOAP repositories. It can become one narrowly scoped shared Todo persistence project. Weather authentication samples should remain independent because their point is comparative security configuration, although contracts and test helpers may later be shared where doing so does not hide the demonstrated mechanism.

The inventory contains 38 project files and 34 legacy `.sln` files, but no `.slnx`, solution filters, central package management, shared build configuration, or GitHub Actions workflows. Frameworks span .NET 8, 9, and 10; one Clean Architecture Domain project is anomalously `net9.0` while its referencing projects are `net10.0`. MAUI remains on .NET 8. Package versions also span framework generations. A single root `.slnx`, optional category `.slnf` files, root `Directory.Build.props`, and carefully staged `Directory.Packages.props` are recommended, while preserving multi-targeting where it is educationally relevant.

Security/configuration cleanup is required during migration. Committed values include sample JWT keys, user passwords, a Duende client secret, an mTLS certificate password, Redis/RabbitMQ credentials, Firebase credential files, SQL Server/MongoDB connection strings, and many fixed localhost ports. These appear to be demonstration values, but must still be replaced with explicit placeholders, user-secrets/environment-variable instructions, and safe sample files. Several SQLite paths are working-directory-sensitive, the GraphQL production connection string is malformed (`Data Source..`), gateway instructions depend on an unrelated Hour Tracker service, and Clean Architecture documentation contains stale repository names and ports.

Recommended approach: create monorepo foundations first; import each repository with history-preserving subtree/filter-repo operations into its category; establish a root build without changing behavior; consolidate only the proven Todo duplicate; keep Weather/auth and infrastructure examples focused; then normalize configuration, documentation, tests, packages, and local deployment assets in separate reviewable commits.

## 2. Complete repository inventory

### `exp.api-gateways.rest-apis`

- **Purpose/classification:** Ocelot and YARP reverse-proxy comparisons; focused integration-pattern examples, not a business layer or Clean Architecture implementation.
- **Solutions:** `src/ApiGateway.Ocelot.RestApi/ApiGateway.Ocelot.RestApi.sln`; `src/ApiGateway.Yarp.RestApi/ApiGateway.Yarp.RestApi.sln`.
- **Projects/frameworks:** `ApiGateway.Ocelot.RestApi.csproj` (`Microsoft.NET.Sdk.Web`, `net8.0`); `ApiGateway.Yarp.RestApi.csproj` (Web, `net9.0`).
- **NuGet:** Ocelot project: `Asp.Versioning.Mvc 8.1.0`, `Microsoft.AspNetCore.OpenApi 8.0.6`, `Microsoft.Extensions.Logging 8.0.0`, `MMLib.SwaggerForOcelot 8.2.0`, `Ocelot 23.3.3`, `Ocelot.Cache.CacheManager 23.3.3`, `Swashbuckle.AspNetCore 6.6.2`. YARP: `Yarp.ReverseProxy 2.3.0`, `Swashbuckle.AspNetCore 9.0.3`.
- **References/tests:** no internal project references and no test projects.
- **Deployment/CI:** appsettings and launch profiles only; no Docker/deployment assets and no GitHub Actions.
- **Documentation:** root README, Ocelot README, YARP `Docs/TestMe.md`.
- **Duplicates/shared code:** repeated ASP.NET host/appsettings/launch-profile structure; gateway-specific code should not become shared business code.
- **Configuration/staleness:** fixed ports include 7101/5101 and 7153/5208; YARP routes to `https://localhost:7198` and documentation assumes an external Hour Tracker API. Ocelot and YARP target different framework/package generations.
- **Target:** `src/Integration/ApiGateway/Ocelot` and `src/Integration/ApiGateway/Yarp`; operational notes under `docs/Integration/ApiGateway`; any runnable dependency composition under `deploy/local/api-gateway`.

### `exp.apis.clean-arch`

- **Purpose/classification:** three complete gRPC Todo Clean Architecture variants: simple, CQRS without mediator, and CQRS with MediatR. This is the full Clean Architecture reference.
- **Solutions:** `grpc-api.clean-arch.simple/grpc-api.clean-arch.simple.sln`, `grpc-api.clean-arch.cqrs-only/grpc-api.clean-arch.cqrs-only.sln`, and `grpc-api.clean-arch.cqrs-mediatr/grpc-api.clean-arch.cqrs-mediatr.sln`.
- **Projects/frameworks:** each variant has `Exp.TodoApp.Domain`, `Application`, `Infrastructure`, `GrpcApi`, and `tests/Exp.TodoApp.Tests` (15 projects total). All target `net10.0` except `grpc-api.clean-arch.simple/src/Exp.TodoApp.Domain`, which targets `net9.0`.
- **NuGet:** common application packages are `AutoMapper 15.1.0`, `FluentValidation 12.1.0`, `FluentValidation.DependencyInjectionExtensions 12.1.0`, configuration/DI abstractions `10.0.0`, plus `MediatR 13.1.0` in the mediator variant. Hosts use `Google.Api.CommonProtos 2.17.0`, `Grpc.AspNetCore 2.71.0`, reflection `2.71.0`, `Grpc.Tools 2.76.0`, JSON transcoding `10.0.0`, gRPC Swagger `0.10.0`, and EF Design `10.0.0`; variants additionally use health checks or Serilog (`Serilog.AspNetCore 8.0.3`, console/file `6.0.0`, environment `3.0.1`, thread `4.0.0`). Infrastructure uses EF Core/SQLite/Tools `10.0.0` and Microsoft.Extensions `10.0.0`. Tests use xUnit `2.9.3`, runner `3.1.5`, `Microsoft.NET.Test.Sdk 18.0.1`, coverlet `6.0.4`, FluentAssertions `8.8.0`, Moq `4.20.72`, and EF InMemory `10.0.0` (with variant-specific application packages).
- **Internal references:** Application -> Domain; Infrastructure -> Application + Domain; host -> Application + Infrastructure. Tests reference either the host (MediatR variant) or Application + Domain + Infrastructure (other variants).
- **Tests:** three `Exp.TodoApp.Tests` projects, one per variant.
- **Deployment/CI:** each variant contains `.dockerignore` and `Dockerfile`; no compose/orchestration and no GitHub Actions.
- **Documentation:** root README plus one README per variant.
- **Duplicates/shared code:** extensive intentional overlap among all three variants in Domain, Infrastructure, host setup, tests, migrations, appsettings, Dockerfiles, and bundled `google/api/{annotations,http}.proto` and `google/protobuf/timestamp.proto`. Preserve variant boundaries for comparison; extract only genuinely invariant Todo contracts/test utilities after behavioral equivalence is proved. Prefer package-provided Google protos rather than four copied source sets.
- **Configuration/staleness:** repeated SQLite relative paths; simple Domain framework mismatch; root README clones `apis.clean-arch.examples.git`; documented ports 5000/5001 and 7113/5035 conflict across variants; one README shows a SQL Server-shaped value under `SqliteConnection`; Docker context/path assumptions must be revalidated after relocation.
- **Target:** `src/Architecture/CleanArchitecture/GrpcTodo/{Simple,Cqrs,CqrsMediatR}` retaining each variant's Domain/Application/Infrastructure/GrpcApi structure; tests under `tests/Architecture/CleanArchitecture/GrpcTodo/...`; Dockerfiles either remain beside hosts with common local orchestration in `deploy/local/clean-architecture`.

### `exp.graphql-apis.styles`

- **Purpose/classification:** Hot Chocolate GraphQL Todo API over SQLite; a layered example, not full Clean Architecture.
- **Solutions/projects/frameworks:** `src/ToDoApp.Data.Sqlite/ToDoApp.Data.Sqlite.sln` and `src/ToDoApp.GraphQLApi.HotChocolate/ToDoApp.GraphQLApi.HotChocolate.sln`; matching Data (SDK, `net9.0`) and Web host (`net9.0`) projects.
- **NuGet/references:** Data uses EF Core, Design, and SQLite `9.0.7`; host uses `AutoMapper 15.0.0`, `HotChocolate.AspNetCore 15.1.7`, and `HotChocolate.Data.EntityFramework 15.1.7`, and references the Data project.
- **Tests/deployment/CI:** no tests, Docker/deployment assets, or GitHub Actions; appsettings and launch profile only.
- **Documentation:** root README, both project READMEs, Data `Docs/Help.md`.
- **Duplicates/shared code:** Data project, migrations, model, DbContext, interface and most service code duplicate the REST/gRPC/SOAP copies. This is the clearest candidate for `shared/Todo`.
- **Configuration/staleness:** production appsettings contains malformed `Data Source..\...` while development uses `Data Source=...`; relative DB path depends on working directory; fixed launch ports.
- **Target:** `src/ApiStyles/GraphQL/HotChocolate`; consume `shared/Todo/Zenvera.Examples.Todo.Sqlite`; future tests under `tests/ApiStyles/GraphQL`.

### `exp.graphsql-apis.auth-styles`

- **Purpose/classification:** GraphQL Weather API authentication via API key, JWT, and Firebase OAuth2; focused authentication examples. Directory name has the `graphsql` typo.
- **Solutions/projects/frameworks:** three independent solution/project pairs under `WeatherApp.GraphQLApi.{ApiKeyAuth,JwtAuth,OAuth2Firebase}`; all Web SDK and `net9.0`.
- **NuGet/references:** API-key/JWT examples use Hot Chocolate ASP.NET Core/Authorization `15.1.7` as applicable, JWT bearer `9.0.7`, and Swashbuckle `9.0.3`; Firebase adds `FirebaseAdminAuthentication.DependencyInjection 1.1.0`, `Google.Apis.Auth 1.70.0`, and `Google.Cloud.Firestore 3.10.0`. No internal project references.
- **Tests/deployment/CI:** no tests, Docker/deployment assets, or GitHub Actions; appsettings/launch profiles only.
- **Documentation:** root README; README and `Docs/TestMe.Md` for each project (seven files total).
- **Duplicates/shared code:** WeatherForecast DTO/query and JWT sample users/settings substantially overlap REST and gRPC auth repositories. Keep auth pipelines local; only a neutral Weather contract could be shared.
- **Configuration/staleness:** fixed ports; committed JWT sample key/passwords; `firebasesettings.json` and sample contain project-specific Firebase identities/URLs and credential-shaped fields; launch profiles require `GOOGLE_APPLICATION_CREDENTIALS=firebasesettings.json`. Root README lists only two projects although Firebase is present. Naming says OAuth2 but implementation is Firebase Admin authentication.
- **Target:** `src/Authentication/GraphQL/{ApiKey,Jwt,Firebase}`; shared neutral model only in `shared/Weather`; security setup docs in `docs/Authentication/GraphQL`.

### `exp.grpc-apis.auth-styles`

- **Purpose/classification:** API-key, JWT, and mutual-TLS Weather gRPC services; focused authentication examples with simple layering.
- **Solutions/projects/frameworks:** three solution/project pairs under `WeatherApp.GrpcApi.{ApiKeyAuth,JwtAuth,MtlsAuth}`; all Web SDK and `net9.0`.
- **NuGet/references:** all use `Grpc.AspNetCore 2.71.0` and reflection `2.71.0`; JWT adds `Microsoft.AspNetCore.Authentication.JwtBearer 9.0.7`. No internal project references.
- **Tests/deployment/CI:** no tests, Docker/deployment assets, or GitHub Actions; appsettings and launch profiles only.
- **Documentation:** root README plus README and `Docs/TestMe.Md` per example.
- **Duplicates/shared code:** Weather service/proto and JWT users/settings overlap internally and across other auth repositories. Keep interceptors/handlers/certificate configuration in each focused sample; consider one Weather proto/contract only if it does not obscure the comparison.
- **Configuration/staleness:** committed JWT key/passwords; mTLS appsettings contain certificate password `test123`, certificate path assumptions, ports 5021/7026, and a fallback URI missing a scheme. Certificate material/path validity needs manual verification.
- **Target:** `src/Authentication/Grpc/{ApiKey,Jwt,MutualTls}`; neutral proto/contracts under `shared/Weather` only if all consumers can use the same schema.

### `exp.grpc-apis.styles`

- **Purpose/classification:** native gRPC and JSON-transcoded gRPC Todo APIs over SQLite; layered examples.
- **Solutions/projects/frameworks:** three solution/project pairs: `ToDoApp.Data.Sqlite`, `ToDoApp.GrpcApi.Native`, and `ToDoApp.GrpcApi.Transcoding`; all `net9.0` (Data SDK, hosts Web SDK).
- **NuGet/references:** Data uses EF Core/Design/SQLite `9.0.7`; hosts use `AutoMapper 15.0.0`, `Grpc.AspNetCore 2.71.0`, reflection `2.71.0`; transcoding adds JSON transcoding `9.0.7` and gRPC Swagger `0.9.7`. Both hosts reference Data.
- **Tests/deployment/CI:** none; appsettings and launch profiles only; no GitHub Actions.
- **Documentation:** root README, three project READMEs, Data Help, and two text docs.
- **Duplicates/shared code:** exact duplicated Todo data layer across four API-style repos; native/transcoding host setup and contracts overlap; bundled Google protos also duplicate Clean Architecture copies.
- **Configuration/staleness:** relative SQLite paths and fixed ports; documentation uses inconsistent `.txt`, `.md`, and filename casing; package-provided Google protos should be evaluated before retaining vendored copies.
- **Target:** `src/ApiStyles/Grpc/{Native,JsonTranscoding}` using `shared/Todo/Zenvera.Examples.Todo.Sqlite`; future tests under `tests/ApiStyles/Grpc`.

### `exp.infra.patterns`

- **Purpose/classification:** focused Weather examples for background services, caching/Redis, RabbitMQ, Serilog, Azure Key Vault, and Blazor logging; infrastructure-pattern examples, not a common production infrastructure layer.
- **Solutions/projects/frameworks:** six independent Web SDK `net10.0` projects/solutions: `WeatherApp.RestApi.UsingBackgroundService`, `.UsingCache`, `.UsingRabbitMQ`, `.UsingSerilog`, `WeatherApp.BlazorServer.UsingKeyVault`, and `.UsingSerilog`.
- **NuGet/references:** Swashbuckle is `10.0.1` except RabbitMQ at `9.0.3`; Zenvera packages are `Shared.Caching 0.0.25`, `Shared.Queuing 0.0.10`, `Shared.Secrets 1.0.21`, `Shared.ErrorHandling 1.0.6`, and `Shared.Logging 1.0.6`/`1.0.8`. No project references.
- **Tests/deployment/CI:** no tests, Docker/compose assets, or GitHub Actions despite external Redis/RabbitMQ/SQL/Mongo/Key Vault prerequisites.
- **Documentation:** root README and per-project READMEs/TestMe docs (ten files).
- **Duplicates/shared code:** Weather DTO/UI and host boilerplate recur, while Serilog exists in both REST and Blazor forms. Do not merge the demonstrated infrastructure behavior; a neutral Weather contract and shared test containers/utilities are acceptable.
- **Configuration/staleness:** Redis `foobared`, RabbitMQ `guest`, SQL Server `RnD`, MongoDB `TestLog`, fixed localhost endpoints/ports, environment-variable names with unrelated prefixes (`LPS_`, `SLC_`), and Azure tenant/client placeholders. Root claims Redis/RabbitMQ/Key Vault/Serilog but omits the background-service example. External services lack reproducible local deployment assets.
- **Target:** `src/Infrastructure/{BackgroundServices,Caching/Redis,Messaging/RabbitMq,Logging/SerilogRest,Logging/SerilogBlazor,Secrets/AzureKeyVault}`; dependencies in `deploy/local/infrastructure`; docs in `docs/Infrastructure`.

### `exp.rest-apis.auth-styles`

- **Purpose/classification:** REST Weather authentication comparisons: API key, Basic, JWT, JWT with ASP.NET Identity/SQL Server, and Duende OAuth2; focused authentication examples.
- **Solutions/projects/frameworks:** five independent Web SDK `net9.0` project/solution pairs under `WeatherApp.RestApi.{ApiKeyAuth,BasicAuth,JwtAuth,JwtAuthIdentity,OAuth2Duende}`.
- **NuGet/references:** basic/API-key use `Swashbuckle.AspNetCore 9.0.3`; JWT adds JWT bearer `9.0.7`; Identity adds OpenAPI/EF Core/SQL Server/Tools/Design/Identity/JWT bearer `9.0.7`, `System.IdentityModel.Tokens.Jwt 8.12.1`, and Swashbuckle `9.0.3`; Duende uses `Duende.IdentityServer 7.2.4`, JWT bearer `9.0.7`, `Microsoft.IdentityModel.JsonWebTokens 8.12.1`, and Swashbuckle `9.0.3`. No internal references.
- **Tests/deployment/CI:** no tests, Docker/deployment assets, or GitHub Actions; appsettings/launch profiles only.
- **Documentation:** root README plus README and TestMe for each sample (11 files).
- **Duplicates/shared code:** Weather DTO/controller and hardcoded users/JWT settings overlap internally and with gRPC/GraphQL auth examples. Authentication code must stay local; share only neutral Weather contracts/test utilities.
- **Configuration/staleness:** committed passwords, JWT keys, Duende client secret and user credentials; SQL Server localhost connection; many fixed ports; Duende uses resource-owner-password concepts that need an explicit educational/deprecation warning and licensing review; identity migrations/database bootstrap need verification.
- **Target:** `src/Authentication/Rest/{ApiKey,Basic,Jwt,JwtIdentity,OAuth2Duende}`; tests under `tests/Authentication/Rest`; local SQL/identity support under `deploy/local/authentication`.

### `exp.rest-apis.styles`

- **Purpose/classification:** MVC controllers, Minimal APIs, route-handler endpoints, and FastEndpoints over a common Todo SQLite layer; layered examples.
- **Solutions/projects/frameworks:** five project/solution pairs: Data plus `ToDoApp.RestApi.{MvcControllers,Minimal,Endpoints,FastEndpoints}`; all `net9.0` (Data SDK; hosts Web SDK).
- **NuGet/references:** Data uses EF Core/Design/SQLite `9.0.7`; MVC/Minimal/Endpoints use `AutoMapper 15.0.0` and Swashbuckle `9.0.3`, with OpenAPI `9.0.7` where applicable; FastEndpoints uses `FastEndpoints` and `FastEndpoints.Swagger 6.2.0`. Every host references Data.
- **Tests/deployment/CI:** no tests, Docker/deployment assets, or GitHub Actions; appsettings/launch profiles only.
- **Documentation:** root README, README per project, and Data Help.
- **Duplicates/shared code:** exact Todo data layer copied across API-style repositories; substantial CRUD/DTO/mapping repetition among hosts is useful for side-by-side comparison and should not all be abstracted away.
- **Configuration/staleness:** relative SQLite paths, fixed/legacy launch ports (including 63221), inconsistent `Data Source=` formatting, and repeated generic README content.
- **Target:** `src/ApiStyles/Rest/{MvcControllers,Minimal,RouteHandlers,FastEndpoints}` using `shared/Todo/Zenvera.Examples.Todo.Sqlite`; tests under `tests/ApiStyles/Rest`.

### `exp.soap-apis.styles`

- **Purpose/classification:** CoreWCF SOAP Todo service over SQLite; layered focused API-style example.
- **Solutions/projects/frameworks:** `ToDoApp.Data.Sqlite` and `ToDoApp.SoapApi.CoreWcf`, each with a solution; both `net9.0`, host uses Web SDK.
- **NuGet/references:** Data uses EF Core/Design/SQLite `9.0.7`; host uses `AutoMapper 15.0.0`, `CoreWCF.Http 1.7.0`, and references Data.
- **Tests/deployment/CI:** no tests, Docker/deployment assets, or GitHub Actions; appsettings/launch profile only.
- **Documentation:** root README, both project READMEs, Data Help.
- **Duplicates/shared code:** exact shared Todo data layer; keep SOAP contracts/service local.
- **Configuration/staleness:** relative SQLite path and fixed ports; client invocation/WSDL instructions should be checked after path/port normalization.
- **Target:** `src/ApiStyles/Soap/CoreWcf` using `shared/Todo/Zenvera.Examples.Todo.Sqlite`; tests under `tests/ApiStyles/Soap`.

### `exp.ui-styles.frameworks`

- **Purpose/classification:** .NET MAUI mobile Todo client consuming a REST API; focused UI/framework example.
- **Solutions/projects/frameworks:** `src/ToDoApp.MauiMobile.UsingRestApi/ToDoApp.MauiMobile.UsingRestApi.sln` and matching SDK project; `net8.0-android`, `net8.0-ios`, and `net8.0-maccatalyst`, with conditional Windows targeting in the project.
- **NuGet/references:** `Microsoft.Maui.Controls 8.0.61`, Compatibility `8.0.61`, Configuration.Json `8.0.0`, Http `8.0.0`, and Logging.Debug `8.0.0`; no internal project reference.
- **Tests/deployment/CI:** no tests, packaging/deployment automation, or GitHub Actions; appsettings and launch profile only.
- **Documentation:** root README, project README, `Docs/HelpMe.md`.
- **Duplicates/shared code:** Todo DTO/client models overlap API contracts, but a mobile contract package must remain platform-safe and should not reference EF or ASP.NET. UI assets are unique.
- **Configuration/staleness:** REST base URL/device-emulator routing and platform prerequisites require verification; .NET 8 MAUI workload differs from .NET 9/10 server SDKs; target list depends on host OS.
- **Target:** `src/UserInterface/Maui/TodoRestClient`; portable DTOs may use `shared/Todo/Zenvera.Examples.Todo.Contracts`; UI tests under `tests/UserInterface`.

## 3. Current-to-target mapping table

| Current repository/content | Recommended target | Treatment |
|---|---|---|
| `exp.rest-apis.styles` hosts | `src/ApiStyles/Rest/{MvcControllers,Minimal,RouteHandlers,FastEndpoints}` | Preserve focused hosts; replace copied Data reference with shared Todo project. |
| `exp.grpc-apis.styles` hosts | `src/ApiStyles/Grpc/{Native,JsonTranscoding}` | Preserve comparison; centralize neutral Todo persistence/contracts. |
| `exp.graphql-apis.styles` host | `src/ApiStyles/GraphQL/HotChocolate` | Preserve layered GraphQL example. |
| `exp.soap-apis.styles` host | `src/ApiStyles/Soap/CoreWcf` | Preserve focused SOAP example. |
| Four copied `ToDoApp.Data.Sqlite` projects | `shared/Todo/Zenvera.Examples.Todo.Sqlite` | Consolidate only after hash/API verification; keep contracts separately platform-safe if needed. |
| `exp.rest-apis.auth-styles` | `src/Authentication/Rest/{ApiKey,Basic,Jwt,JwtIdentity,OAuth2Duende}` | Keep each authentication pipeline independent. |
| `exp.grpc-apis.auth-styles` | `src/Authentication/Grpc/{ApiKey,Jwt,MutualTls}` | Keep protocol/auth concerns visible. |
| `exp.graphsql-apis.auth-styles` | `src/Authentication/GraphQL/{ApiKey,Jwt,Firebase}` | Correct GraphQL naming during import; keep mechanisms independent. |
| `exp.apis.clean-arch` variants | `src/Architecture/CleanArchitecture/GrpcTodo/{Simple,Cqrs,CqrsMediatR}` | Preserve full layers and variant boundaries. |
| Clean Architecture tests | `tests/Architecture/CleanArchitecture/GrpcTodo/...` | Preserve one suite per variant initially. |
| `exp.api-gateways.rest-apis` | `src/Integration/ApiGateway/{Ocelot,Yarp}` | Preserve framework comparison; add reproducible downstream service config. |
| `exp.infra.patterns` | `src/Infrastructure/...` | One folder per demonstrated concern; do not create a universal infrastructure library. |
| `exp.ui-styles.frameworks` | `src/UserInterface/Maui/TodoRestClient` | Retain MAUI target/workload isolation. |
| Per-project tests added later | `tests/{ApiStyles,Authentication,Integration,Infrastructure,UserInterface}/...` | Mirror source taxonomy. |
| Repository/project READMEs | README beside example plus indexes in `docs/` | Keep runnable instructions close; centralize navigation/design guidance. |
| Dockerfiles and future compose | host-adjacent Dockerfiles; `deploy/local/...` orchestration | Preserve build context explicitly. |
| 34 `.sln` files | root `zenvera.dotnet-examples.slnx` plus optional `.slnf` files | Remove redundant solutions only after root solution is validated. |

## 4. Duplicate-code assessment

### High-confidence consolidation candidates

1. **Todo SQLite layer:** byte-identical copies exist in REST, gRPC, GraphQL, and SOAP repositories, including `ToDo.cs`, `ITodoService.cs`, `AppDbContext.cs`, project file, migrations, snapshot, global usings, solution, Help, and much of `ToDoService.cs`. Create one `Zenvera.Examples.Todo.Sqlite`; do not share its `.sln` or example-specific README.
2. **Repository scaffolding:** nearly every repository repeats `.gitignore`, `.gitattributes`, `LICENSE`, root README conventions, individual `.sln` files, appsettings templates, and launch profiles. Consolidate at root where semantics match.
3. **Google protos:** `annotations.proto`, `http.proto`, and `timestamp.proto` appear in all three Clean Architecture variants and the gRPC transcoding example. Prefer authoritative package assets or one contracts location with license/upgrade provenance.
4. **Clean Architecture variants:** Domain entities, persistence, migrations, host configuration, tests, Dockerfiles, and settings overlap heavily. Some duplication is intentional pedagogy. Keep each variant independently readable; share only contracts/test utilities that do not erase the CQRS/MediatR contrast.
5. **Weather/auth boilerplate:** WeatherForecast DTOs, sample users, JWT settings, authorization attributes, and TestMe snippets recur across REST, gRPC, and GraphQL. A tiny `shared/Weather` contract is reasonable; shared authentication implementation is not.
6. **Infrastructure Weather/UI:** repeated Weather generation and display code can use a neutral contract, but caching, queuing, secrets, logging, and background-service wiring must remain local to each example.

Identical configuration files are common (seven matching development appsettings in API-style hosts, five matching production appsettings, and repeated README templates). Consolidate configuration defaults only when all hosts need the same behavior; otherwise use root conventions and retain local files for discoverability.

## 5. Dependency and framework compatibility assessment

| Area | Current state | Compatibility/action |
|---|---|---|
| SDK/frameworks | .NET 8 gateway + MAUI, .NET 9 most API/auth examples, .NET 10 Clean Architecture/infrastructure | Install/pin all required SDKs in `global.json` roll-forward strategy or intentionally retarget in a later change. Do not silently upgrade during import. MAUI requires separate workloads. |
| Clean Architecture | Mostly `net10.0`; Simple Domain is `net9.0` | A `net10.0` project can reference `net9.0`, but the mismatch is likely accidental; decide and normalize after baseline build. |
| ASP.NET/EF | 8.0.x, 9.0.7, and 10.0.0/10.0.1 packages track different TFMs | Central versions must allow per-framework version families; do not force one incompatible version globally. |
| gRPC | Server `2.71.0`; Tools `2.76.0`; transcoding/Swagger 9.x and 10.x | Generally separable, but validate generated code and transitive versions per TFM. Centralize compatible families only. |
| Swagger | Swashbuckle 6.6.2, 9.0.3, 10.0.1 | Keep framework-appropriate versions initially; review breaking API changes before consolidation. |
| AutoMapper | 15.0.0 and 15.1.0 | Low-risk alignment after build/tests; check licensing/version policy. |
| Hot Chocolate | 15.1.7 | Consistent across GraphQL examples; good central-package candidate. |
| EF providers | SQLite 9/10, SQL Server 9, InMemory 10 | Keep provider major aligned with consuming framework/EF runtime. |
| Zenvera.Shared | Package versions range `0.0.10` to `1.0.21` | External/private feed availability is a major build risk. Decide whether examples should retain packages, pin them centrally, or replace only with in-repo illustrative adapters. |
| Duende/Firebase | Duende 7.2.4; Firebase/Google packages | Confirm licenses, credentials setup, and current supported authentication guidance before publishing. |
| CoreWCF/FastEndpoints/YARP/Ocelot | Independent focused dependencies | Keep scoped to example projects; avoid leaking into shared projects. |
| Tests | Only Clean Architecture has tests | Add smoke/contract tests by category; a root build alone will not validate runtime configuration or auth. |

No `Directory.Packages.props`, `Directory.Build.props`, `NuGet.config`, or root SDK pin was observed in the source inventory. Introduce central package management incrementally: first record existing versions exactly, then align versions in a separate commit.

## 6. Naming inconsistencies

- `exp.graphsql-apis.auth-styles` should be `graphql`; the contained projects already use `GraphQLApi`.
- `exp.api-gateways.rest-apis` is plural `gateways`, while the proposed name/example list used singular `api-gateway`.
- `exp.apis.clean-arch` is broader than its actual gRPC Todo scope and differs from the proposed `exp.grpc-api.clean-arch`.
- `ToDoApp`, `TodoApp`, `ToDo`, and `Todo` coexist. Prefer `Todo` in new namespaces/folders while avoiding behavior-changing renames during history import.
- `GraphQLApi`, `GrpcApi`, `RestApi`, `SoapApi`, and `ApiGateway` capitalization is mostly consistent, but folder taxonomy should use `GraphQL`, `Grpc`, `Rest`, and `Soap` as proposed.
- `MtlsAuth` should be presented as `MutualTls` (or consistently `mTLS`) in navigation.
- `OAuth2Firebase` describes the mechanism imprecisely; `Firebase` or `FirebaseAuthentication` is clearer.
- `Endpoints` is ambiguous beside Minimal and FastEndpoints; `RouteHandlers` is a clearer target label if that is what it demonstrates.
- Documentation names/case vary: `TestMe.md`, `TestMe.Md`, `ReadMe.txt`, `Help.md`, `HelpMe.md`, and `Docs`.
- Clean Architecture variants use `cqrs-mediatr`, `cqrs-only`, and `simple`; target names should be `CqrsMediatR`, `Cqrs`, and `Simple`.
- Infrastructure environment variables use `LPS_`/`SLC_`, names unrelated to Zenvera examples.

## 7. Migration risks

1. **History loss:** copying files directly would discard per-repository history. Import each untouched repository through `git subtree` or `git filter-repo` into a staging branch/tag, retain source commit IDs in a migration manifest, and never rewrite the originals.
2. **Credential exposure:** committed credential-shaped values and Firebase files may be real or derived from real projects. Treat them as exposed until manually confirmed/rotated; Git history preservation means removal from the new tip does not remove historical disclosure.
3. **Build breakage from path relocation:** SQLite paths, Docker `COPY` paths, project references, MAUI appsettings, certificate paths, gateway routes, and documentation all assume current relative locations.
4. **Port collisions:** many projects reuse or hardcode localhost ports; all cannot run together without an allocation registry and compose/profile overrides.
5. **Framework/workload availability:** CI must install .NET 8/9/10 and optionally MAUI workloads. Building the entire `.slnx` on Linux may fail for platform-specific MAUI targets.
6. **Premature abstraction:** sharing authentication, infrastructure wiring, or CRUD handlers could hide the exact technology each example is intended to teach.
7. **Clean Architecture comparison erosion:** deduplicating whole layers across variants would make the variants harder to compare and no longer complete references.
8. **Private package/feed dependency:** `Zenvera.Shared.*` restore may fail without feed configuration or published packages.
9. **No baseline CI:** there is no workflow evidence that all current projects restore/build/test. Capture baseline results per repository before attributing failures to migration.
10. **Package/license changes:** Duende, AutoMapper, Firebase, and other ecosystem packages need license and support review before broad publication or upgrades.
11. **Database/migration collision:** four copied SQLite migrations and three Clean Architecture databases need unique runtime locations or deliberate sharing.
12. **Stale docs:** gateway Hour Tracker dependency, old clone URL, inconsistent ports, malformed connection examples, and incomplete project lists can mislead users after import.

## 8. Proposed final folder tree

```text
zenvera.dotnet-examples/
|-- zenvera.dotnet-examples.slnx
|-- global.json
|-- Directory.Build.props
|-- Directory.Packages.props
|-- .editorconfig
|-- .gitattributes
|-- .gitignore
|-- LICENSE
|-- README.md
|-- src/
|   |-- ApiStyles/
|   |   |-- Rest/{MvcControllers,Minimal,RouteHandlers,FastEndpoints}/
|   |   |-- Grpc/{Native,JsonTranscoding}/
|   |   |-- GraphQL/HotChocolate/
|   |   `-- Soap/CoreWcf/
|   |-- Authentication/
|   |   |-- Rest/{ApiKey,Basic,Jwt,JwtIdentity,OAuth2Duende}/
|   |   |-- Grpc/{ApiKey,Jwt,MutualTls}/
|   |   `-- GraphQL/{ApiKey,Jwt,Firebase}/
|   |-- Architecture/CleanArchitecture/GrpcTodo/
|   |   |-- Simple/{Domain,Application,Infrastructure,GrpcApi}/
|   |   |-- Cqrs/{Domain,Application,Infrastructure,GrpcApi}/
|   |   `-- CqrsMediatR/{Domain,Application,Infrastructure,GrpcApi}/
|   |-- Integration/ApiGateway/{Ocelot,Yarp}/
|   |-- Infrastructure/
|   |   |-- BackgroundServices/
|   |   |-- Caching/Redis/
|   |   |-- Messaging/RabbitMq/
|   |   |-- Logging/{SerilogRest,SerilogBlazor}/
|   |   `-- Secrets/AzureKeyVault/
|   `-- UserInterface/Maui/TodoRestClient/
|-- shared/
|   |-- Todo/
|   |   |-- Zenvera.Examples.Todo.Contracts/
|   |   `-- Zenvera.Examples.Todo.Sqlite/
|   |-- Weather/Zenvera.Examples.Weather.Contracts/
|   `-- Testing/Zenvera.Examples.Testing/
|-- tests/
|   |-- ApiStyles/{Rest,Grpc,GraphQL,Soap}/
|   |-- Authentication/{Rest,Grpc,GraphQL}/
|   |-- Architecture/CleanArchitecture/GrpcTodo/{Simple,Cqrs,CqrsMediatR}/
|   |-- Integration/ApiGateway/
|   |-- Infrastructure/
|   `-- UserInterface/
|-- docs/
|   |-- architecture/
|   |-- api-styles/
|   |-- authentication/
|   |-- integration/
|   |-- infrastructure/
|   |-- user-interface/
|   |-- port-registry.md
|   `-- migration-manifest.md
|-- deploy/local/
|   |-- api-gateway/
|   |-- authentication/
|   |-- clean-architecture/
|   `-- infrastructure/
`-- .github/workflows/
    |-- build.yml
    `-- examples-smoke.yml
```

The `shared/Weather` and `shared/Testing` projects should be created only when at least two consumers have a proven identical, neutral contract/utility. Empty taxonomy directories should not be committed merely to match the diagram.

## 9. Step-by-step migration sequence

1. Freeze and tag each source repository; record origin URL, branch, tag/commit SHA, license, and intended target path in `docs/migration-manifest.md`.
2. Create the target repository foundations: root license/readme, `.gitignore`, `.gitattributes`, `.editorconfig`, `global.json`, `Directory.Build.props`, and an initially version-preserving `Directory.Packages.props` strategy.
3. Create one root `zenvera.dotnet-examples.slnx` and decide solution-filter categories. Do not delete imported `.sln` files until their replacement is validated.
4. Import repositories one at a time with history-preserving tooling into temporary top-level staging paths. Tag each import merge.
5. Move imported content within the target repository to the mapping above in separate commits, updating only paths/references needed to restore/build. Keep original repositories untouched.
6. Capture a baseline per example: `dotnet restore`, build, existing Clean Architecture tests, and MAUI workload/platform constraints. Record pre-existing failures.
7. Normalize the root build and solution membership without retargeting frameworks or upgrading packages. Exclude platform-specific MAUI targets from generic Linux CI as needed.
8. Consolidate the four byte-identical Todo Data projects into `shared/Todo`; split platform-safe contracts from EF/SQLite persistence if the MAUI client consumes contracts. Add contract/persistence tests before switching all hosts.
9. Preserve all three Clean Architecture variants, relocate their tests, and deduplicate only clearly invariant contracts/test utilities or package-provided Google protos. Validate Docker build contexts.
10. Correct credential handling: remove live values from the working tree, create `.sample` configuration, use environment variables/user secrets, rotate anything possibly real, and document certificate/Firebase provisioning. Consider whether sensitive history needs a separate security-approved rewrite despite the history-preservation objective.
11. Add `deploy/local` compositions for Redis, RabbitMQ, SQL Server/Identity, logging sinks, gateways and downstream demo services. Establish a documented non-colliding port registry.
12. Fix stale/malformed configuration and docs: GraphQL SQLite string, gateway Hour Tracker assumption, Clean Architecture clone URL/ports/SQL-shaped SQLite example, project lists, doc filename casing, and MAUI emulator URLs.
13. Introduce CI in stages: server restore/build matrix by SDK, Clean Architecture tests, focused smoke tests, Docker builds, secret scanning, then optional MAUI jobs on suitable runners.
14. Align package versions only after the version-preserving monorepo builds. Use dependency-family conditions where .NET 8/9/10 require different majors.
15. Once root `.slnx`, filters, CI, docs, and local deployment are proven, remove redundant imported `.sln`, `.gitignore`, `.gitattributes`, and repeated build/package files in a dedicated cleanup commit.
16. Publish a migration map from old repository paths/URLs to new example locations; archive rather than delete the original repositories so history and inbound links remain available.

## 10. Items requiring manual decisions

1. Should all server examples be retargeted to one supported .NET version, or should .NET 8/9/10 remain as an intentional compatibility matrix? MAUI may need a separate decision.
2. Is the `net9.0` Simple Clean Architecture Domain project intentional?
3. Should the three Clean Architecture variants remain fully standalone (maximum pedagogical completeness) or share Domain/contracts/test fixtures (less duplication)?
4. Should Todo SQLite be one shared persistence project for all API styles, and should MAUI consume a separate contracts-only package?
5. Is a shared Weather contract valuable enough to offset added indirection in tiny authentication/infrastructure samples?
6. Are `Zenvera.Shared.*` packages publicly restorable and intended dependencies, or should the examples demonstrate local abstractions/adapters instead?
7. Are any committed JWT keys, passwords, Duende secrets, Firebase credentials, certificate files/passwords, database strings, or Azure identifiers real? If so, rotate them and decide whether the target history may include the imported secret-bearing commits.
8. Should Firebase files be imported at all, imported only as sanitized samples, or replaced with setup instructions?
9. What downstream API should gateway examples route to in the monorepo: a new minimal sample, one REST Todo host, or the external Hour Tracker API?
10. Should local infrastructure be Docker Compose-based, Aspire-based, or both?
11. Is Duende's resource-owner-password example intentionally retained for legacy education, and are licensing/security warnings acceptable?
12. Should API style comparisons use identical routes/contracts/data for rigorous side-by-side comparison, or preserve current small behavioral differences?
13. What naming convention is authoritative: `Todo` versus `ToDo`, `MutualTls` versus `Mtls`, and `Firebase` versus `OAuth2Firebase`?
14. Which optional `.slnf` files are desired (API styles, authentication, Clean Architecture, infrastructure, UI), and should MAUI be omitted from the default root build?
15. Which ports should be reserved per example, and which examples must run concurrently?
16. Should the original repositories be archived with redirect READMEs after migration, or remain independently maintained mirrors?

---

Assessment basis: filesystem and project metadata inspection of all `exp.*` repositories, excluding generated `.git`, `bin`, `obj`, `.vs`, and `node_modules` content. No build, restore, runtime, credential validation, or source modification was performed in this analysis-only step.
