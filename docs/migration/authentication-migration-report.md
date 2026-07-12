# Authentication migration report

Date: 2026-07-12

## Scope and source resolution

The REST and gRPC sources were migrated from:

- `D:\GitHub\VishwamKumar\example\exp.rest-apis.auth-styles`
- `D:\GitHub\VishwamKumar\example\exp.grpc-apis.auth-styles`

The requested `exp.graphql-apis.auth-styles` directory does not exist locally. The inventory had previously identified the same repository under its on-disk misspelling, so GraphQL examples were migrated from:

- `D:\GitHub\VishwamKumar\example\exp.graphsql-apis.auth-styles`

All original repositories remain intact and were not modified or deleted.

## Authentication inventory and mapping

| Protocol | Source project | Mechanism | Target project |
|---|---|---|---|
| REST | `WeatherApp.RestApi.ApiKeyAuth` | Static API key in `X-Api-Key` | `src/Authentication/Rest/ApiKey/Exp.Auth.RestApi.ApiKey` |
| REST | `WeatherApp.RestApi.BasicAuth` | Custom HTTP Basic authentication | `src/Authentication/Rest/BasicAuthentication/Exp.Auth.RestApi.BasicAuthentication` |
| REST | `WeatherApp.RestApi.JwtAuth` | Locally issued symmetric JWT bearer | `src/Authentication/Rest/JwtBearer/Exp.Auth.RestApi.JwtBearer` |
| REST | `WeatherApp.RestApi.JwtAuthIdentity` | ASP.NET Identity/SQL Server plus JWT bearer | `src/Authentication/Rest/JwtBearerIdentity/Exp.Auth.RestApi.JwtBearerIdentity` |
| REST | `WeatherApp.RestApi.OAuth2Duende` | Duende OAuth2/OIDC client credentials and JWT validation | `src/Authentication/Rest/OAuth2Duende/Exp.Auth.RestApi.OAuth2Duende` |
| gRPC | `WeatherApp.GrpcApi.ApiKeyAuth` | API key in request metadata | `src/Authentication/Grpc/ApiKey/Exp.Auth.GrpcApi.ApiKey` |
| gRPC | `WeatherApp.GrpcApi.JwtAuth` | HTTP login plus symmetric JWT for gRPC | `src/Authentication/Grpc/JwtBearer/Exp.Auth.GrpcApi.JwtBearer` |
| gRPC | `WeatherApp.GrpcApi.MtlsAuth` | Mutual TLS/client certificates | `src/Authentication/Grpc/MutualTls/Exp.Auth.GrpcApi.MutualTls` |
| GraphQL | `WeatherApp.GraphQLApi.ApiKeyAuth` | API-key HTTP middleware | `src/Authentication/GraphQL/ApiKey/Exp.Auth.GraphQLApi.ApiKey` |
| GraphQL | `WeatherApp.GraphQLApi.JwtAuth` | Locally issued symmetric JWT plus Hot Chocolate authorization | `src/Authentication/GraphQL/JwtBearer/Exp.Auth.GraphQLApi.JwtBearer` |
| GraphQL | `WeatherApp.GraphQLApi.OAuth2Firebase` | Firebase Admin/Firebase ID token validation | `src/Authentication/GraphQL/FirebaseAuthentication/Exp.Auth.GraphQLApi.FirebaseAuthentication` |

Only mechanisms found in the source were created. No protocol-neutral shared authentication package was introduced: handlers, middleware, token issuance, certificate processing, and provider integration are transport/provider specific. Weather DTO similarity was not treated as sufficient reason to add `shared/Authentication`.

## Import exclusions and renames

The import excluded nested `.git`, `.gitignore`, `.gitattributes`, licenses, child `.sln`, workflows, `bin`, `obj`, `.vs`, `.idea`, user files, certificate/private-key files, Duende persisted signing-key files, and the Firebase credential file.

Project files, assembly names, root namespaces, explicit namespaces/usings, proto C# namespaces, launch profiles, and copied documentation were normalized to the `Exp.Auth.*` identities shown above. The internal API-key configuration class was renamed from `ApiKey` to `ApiKeyCredential` in three projects to avoid a C# namespace/type collision after the project namespace became `.ApiKey`. Header names and authentication behavior were unchanged.

All 11 projects were added to `zenvera.dotnet-examples.slnx`. The solution now contains 20 projects in total (the previous nine API-style/shared projects plus 11 authentication projects).

## Secret and configuration handling

The following source material was not retained as usable configuration:

- API keys `ApiKey1`/`ApiKey2`
- demonstration passwords `TestPassword1` through `TestPassword4`
- symmetric JWT keys
- Duende client secret and persisted signing-key JSON files
- SQL Server machine-specific connection string
- mTLS PFX files, path, and password
- Firebase `firebasesettings.json` and project-specific identities

Committed `appsettings*.json` sensitive fields are blank. Launch profiles supply values explicitly named `development-only-...` for simple local demonstrations; environment variables or user secrets override them. Identity uses `ConnectionStrings__DefaultConnection`; mutual TLS uses Kestrel certificate environment variables. Firebase includes only `firebasesettings.json.example`; the actual `firebasesettings.json` remains ignored and must be supplied outside source control.

The Identity `UsersContext` hardcoded SQL Server fallback was removed so configuration is authoritative. Copied TestMe documents were sanitized.

## Ports

- REST API key 6101/8101; Basic 6102/8102; JWT 6103/8103; Identity JWT 6104/8104; Duende 6105/8105.
- gRPC API key 6201/8201; JWT 6202/8202; mutual TLS 6203/8203.
- GraphQL API key 6301/8301; JWT 6302/8302; Firebase 6303/8303.

## Validation results

### Restore

Command:

```powershell
dotnet restore zenvera.dotnet-examples.slnx --configfile NuGet.config -p:NuGetAudit=false
```

Result: **passed** for the complete 20-project solution.

### Build

The first build correctly exposed a migration-induced `ApiKey` namespace/type collision in the three API-key projects. The internal model rename described above repaired it. Stale generated proto files were removed, assets were regenerated, and the clean build was rerun.

Final complete-solution result: **passed**, 0 errors and 32 warnings. Warnings are existing nullable/analyzer issues plus an obsolete `X509Certificate2` loading constructor in the mTLS example. No warning was hidden or converted into an error.

Every migrated authentication project was also built individually: **11 passed, 0 failed**.

### Tests

None of the three source repositories contains a test project. There were no automated tests to migrate or run. Category READMEs describe required manual negative/positive authentication checks.

### Credential scan

The final high-confidence scan checked for the known source values and fingerprints, nonempty sensitive values in committed appsettings, Firebase credential filenames, PFX/P12/private-key files, and Duende persisted signing-key files.

Result:

- Known source credential fingerprints: **0**
- Nonempty sensitive appsettings values: **0**
- Sensitive credential files: **0**

Development-only placeholders remain intentionally in launch profiles and documentation. They are conspicuously named and are not production credentials.

## Broken, incomplete, or externally dependent examples

1. **Mutual TLS:** builds, but cannot run until a server PFX and trusted client certificate are provided. Certificate validation and PKI lifecycle are demonstration-level. The certificate-loading constructor is obsolete under the selected SDK.
2. **Firebase:** builds, but `FirebaseApp.Create()` requires a real external service-account credential or workload identity configuration and a Firebase project. The actual credential file is intentionally absent.
3. **Identity JWT:** builds, but requires SQL Server and application of the existing EF migrations. No local SQL deployment was added in this step.
4. **Duende:** builds and uses in-memory configuration, but requires licensing review and is not a production authorization-server topology. Source-era commented/resource-owner-password provider material is retained only for demonstration context and is not recommended for new systems.
5. **Static API-key/Basic/JWT users:** launch-profile placeholders make local flows demonstrable, but these are not production identity or secret stores.
6. **GraphQL API-key docs/source:** the copied project README historically described JWT in places; the new category README and comparison matrix are authoritative.
7. **Tests:** no automated security, protocol, integration, revocation, rotation, or runtime startup tests exist.
8. **Analyzer warnings:** existing code-quality/security-adjacent warnings remain for later behavior-reviewed work.

## Documentation added

- `src/Authentication/Rest/README.md`
- `src/Authentication/Grpc/README.md`
- `src/Authentication/GraphQL/README.md`
- `docs/comparison-matrices/authentication-styles.md`
- this report

## Source repositories retained

Final `git status --short` checks against all three on-disk source repositories are expected to remain clean; the migration used read-only copies and made changes only inside `zenvera.dotnet-examples`.
