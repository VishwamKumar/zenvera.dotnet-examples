# gRPC authentication demonstrations

## Purpose

These focused `net10.0` Weather gRPC services demonstrate API-key metadata, JWT bearer authentication, and mutual TLS. Each keeps protocol-specific middleware and proto contracts local.

| Mechanism/project | Ports HTTP/HTTPS | Security posture |
|---|---:|---|
| API key — `Exp.Auth.GrpcApi.ApiKey` | 6201 / 8201 | Internal/service use with TLS and substantial key lifecycle hardening. |
| JWT bearer — `Exp.Auth.GrpcApi.JwtBearer` | 6202 / 8202 | Production-capable with an external issuer, managed signing keys, validation, and revocation design. |
| Mutual TLS — `Exp.Auth.GrpcApi.MutualTls` | 6203 / 8203 | Strong service identity; production-capable with PKI lifecycle and strict certificate validation. |

## Request flows

- **API key:** client adds `X-Api-Key` metadata; middleware validates it before the Weather RPC executes.
- **JWT:** client calls the HTTP token endpoint with demonstration credentials, then sends bearer-token metadata on the protected RPC.
- **Mutual TLS:** TLS authenticates the server and requires a client certificate before gRPC dispatch; application code applies certificate validation.

## Configuration

Settings files contain blank secret values. Launch profiles inject development-only placeholders. Override them securely:

```powershell
$env:ApiKeys__0__Key = '<local-api-key>'
$env:UserCredentials__0__UserName = '<local-user>'
$env:UserCredentials__0__Password = '<local-password>'
$env:JwtSettings__SecretKey = '<at-least-32-random-bytes>'
$env:Kestrel__Endpoints__Https__Certificate__Path = '<absolute-server-pfx-path>'
$env:Kestrel__Endpoints__Https__Certificate__Password = '<pfx-password>'
```

Certificate files are deliberately not committed. The mTLS example cannot run successfully until a local server certificate and a trusted client certificate are supplied.

## Run

```powershell
dotnet run --project src/Authentication/Grpc/ApiKey/Exp.Auth.GrpcApi.ApiKey
dotnet run --project src/Authentication/Grpc/JwtBearer/Exp.Auth.GrpcApi.JwtBearer
dotnet run --project src/Authentication/Grpc/MutualTls/Exp.Auth.GrpcApi.MutualTls
```

## Test

No automated source tests exist. Use a generated gRPC client or `grpcurl` to check missing/invalid/valid metadata, expired JWT behavior, and certificate rejection/acceptance. Build validation alone does not exercise TLS negotiation.

## Security warnings and production considerations

- Never send API keys or bearer tokens over plaintext channels.
- Replace static users and in-process token issuance with a trusted identity provider.
- Scope and rotate API keys; use constant-time comparison or hashed lookup and rate limiting.
- Validate JWT issuer, audience, lifetime, signature algorithm, and keys; support rollover and revocation strategy.
- mTLS needs managed CA trust, SAN/EKU validation, revocation/short-lived certificates, automated renewal, private-key protection, and proxy end-to-end identity design.
- The source certificate-loading constructor is obsolete under the selected SDK and should move to `X509CertificateLoader` in a separate behavior-reviewed change.
