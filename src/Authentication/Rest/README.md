# REST authentication demonstrations

## Purpose

These focused `net9.0` Weather API projects compare authentication mechanisms at the REST transport boundary. They are demonstrations, not a shared authentication platform, and each remains independently runnable.

| Mechanism/project | Ports HTTP/HTTPS | Security posture |
|---|---:|---|
| API key — `Exp.Auth.RestApi.ApiKey` | 6101 / 8101 | Internal/service use only with TLS, rotation, hashing, scoping, and rate limits. |
| Basic — `Exp.Auth.RestApi.BasicAuthentication` | 6102 / 8102 | Demonstration/legacy interoperability; not recommended for new production systems. |
| Symmetric JWT — `Exp.Auth.RestApi.JwtBearer` | 6103 / 8103 | Production-capable only after external issuance/key management, rotation, revocation strategy, and validation hardening. |
| JWT + ASP.NET Identity — `Exp.Auth.RestApi.JwtBearerIdentity` | 6104 / 8104 | Production-capable with database, Identity, key management, account controls, and operational hardening. |
| Duende OAuth2/OIDC client credentials — `Exp.Auth.RestApi.OAuth2Duende` | 6105 / 8105 | Standards-oriented, production-capable with licensing review, durable stores, managed keys, and full hardening. |

## Request flows

- **API key:** client sends `X-Api-Key`; middleware compares it with configured keys, then allows the Weather controller request.
- **Basic:** client sends `Authorization: Basic <base64(username:password)>`; custom middleware validates configured demonstration users.
- **JWT bearer:** client posts demonstration credentials to the auth endpoint, receives a locally signed JWT, then sends `Authorization: Bearer <token>` to protected endpoints.
- **JWT + Identity:** client registers/logs in against ASP.NET Identity backed by SQL Server, receives a JWT, then accesses protected endpoints.
- **Duende:** machine client requests an access token through client credentials, then presents the bearer token to the API hosted in the same demonstration process.

## Configuration

Committed `appsettings*.json` files contain no usable keys, passwords, signing secrets, client secrets, or database connection strings. Launch profiles provide conspicuous development-only placeholders so the simple demonstrations can run. Override them with environment variables or user secrets:

```powershell
$env:ApiKeys__0__Key = '<local-api-key>'
$env:UserCredentials__0__UserName = '<local-user>'
$env:UserCredentials__0__Password = '<local-password>'
$env:JwtSettings__SecretKey = '<at-least-32-random-bytes>'
$env:ConnectionStrings__DefaultConnection = '<sql-server-connection-string>'
$env:IdentityServerSettings__ClientSecret = '<local-client-secret>'
```

Never use launch-profile placeholders outside local development. Prefer user secrets locally and a managed secret/key system in deployed environments.

## Run

```powershell
dotnet run --project src/Authentication/Rest/ApiKey/Exp.Auth.RestApi.ApiKey
dotnet run --project src/Authentication/Rest/BasicAuthentication/Exp.Auth.RestApi.BasicAuthentication
dotnet run --project src/Authentication/Rest/JwtBearer/Exp.Auth.RestApi.JwtBearer
dotnet run --project src/Authentication/Rest/JwtBearerIdentity/Exp.Auth.RestApi.JwtBearerIdentity
dotnet run --project src/Authentication/Rest/OAuth2Duende/Exp.Auth.RestApi.OAuth2Duende
```

Identity additionally requires reachable SQL Server and application of its existing EF migrations. Duende HTTPS metadata and issuer URLs use port 8105.

## Test

There are no automated source tests. Build with:

```powershell
dotnet build zenvera.dotnet-examples.slnx -p:NuGetAudit=false
```

Then use Swagger/curl to verify unauthenticated rejection, valid credential/token acceptance, invalid credential rejection, expiry, and protected endpoint behavior. Basic credentials must be base64 encoded but are not encrypted; always test over HTTPS.

## Security warnings and production considerations

- Static configured users and locally issued symmetric JWTs are demonstration-only identity stores/issuers.
- Basic authentication exposes reusable credentials to every request and is not recommended for new production designs.
- API keys are bearer secrets; store hashes, scope keys, audit usage, rotate, expire, and throttle.
- JWT validation must pin allowed algorithms, issuer, audience, lifetime, signing keys, and clock skew. Design revocation and incident response.
- Identity requires password policy, lockout, MFA where appropriate, confirmed accounts, secure recovery, migrations, and database protection.
- Duende requires licensing review, secure signing keys, durable grants/configuration, proper discovery/metadata, and separate authorization-server deployment for real systems.
