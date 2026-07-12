# GraphQL authentication demonstrations

## Purpose

These focused `net10.0` Hot Chocolate Weather APIs demonstrate API-key middleware, symmetric JWT bearer authentication, and Firebase-issued token validation.

| Mechanism/project | Ports HTTP/HTTPS | Security posture |
|---|---:|---|
| API key — `Exp.Auth.GraphQLApi.ApiKey` | 6301 / 8301 | Internal/service use with TLS, scoping, rotation, hashing, and query controls. |
| JWT bearer — `Exp.Auth.GraphQLApi.JwtBearer` | 6302 / 8302 | Production-capable only with external issuance/key management and hardened resolver authorization. |
| Firebase authentication — `Exp.Auth.GraphQLApi.FirebaseAuthentication` | 6303 / 8303 | Production-capable with managed credentials, token validation, authorization, and Firebase operational controls. |

## Request flows

- **API key:** client sends `X-Api-Key` on the GraphQL HTTP request; middleware validates it before query execution.
- **JWT:** client obtains a locally signed demonstration token from the auth controller and sends it as `Authorization: Bearer <token>`; Hot Chocolate authorization protects schema operations.
- **Firebase:** client obtains a Firebase ID token externally and sends it as a bearer token; Firebase Admin validates it before authorized resolvers run.

## Configuration

API-key and JWT settings are blank in committed appsettings files. Launch profiles contain development-only placeholders. Override with environment variables or user secrets.

Firebase service-account credentials are never committed. Copy `firebasesettings.json.example` to an ignored secure local path, replace every placeholder from secure storage, and set:

```powershell
$env:GOOGLE_APPLICATION_CREDENTIALS = '<absolute-path-to-firebase-service-account-json>'
```

## Run

```powershell
dotnet run --project src/Authentication/GraphQL/ApiKey/Exp.Auth.GraphQLApi.ApiKey
dotnet run --project src/Authentication/GraphQL/JwtBearer/Exp.Auth.GraphQLApi.JwtBearer
dotnet run --project src/Authentication/GraphQL/FirebaseAuthentication/Exp.Auth.GraphQLApi.FirebaseAuthentication
```

The GraphQL endpoint is `/graphql`. Firebase will fail at startup without valid external credentials.

## Test

No automated source tests exist. Use the GraphQL IDE or an HTTP client to verify unauthenticated query rejection, invalid credential/token rejection, valid access, and authorization on each protected field. Include aliases, batched operations, introspection policy, and query complexity in future security tests.

## Security warnings and production considerations

- HTTP middleware authentication does not replace field/resolver authorization or tenant/data filtering.
- Protect GraphQL against expensive queries, excessive depth, batching abuse, introspection exposure, and authorization gaps.
- Static API keys and local symmetric token issuance are demonstration-only.
- Firebase service-account JSON is highly sensitive; use workload identity or managed secret delivery where possible, rotate exposed keys, validate token audience/project, and enforce application authorization after authentication.
