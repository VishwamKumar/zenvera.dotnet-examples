# LLD — authentication flows

## Scope

The repository implements authentication demonstrations for REST, gRPC, and GraphQL. They are independent Level 1 examples; this document compares flow boundaries and does not define one shared authentication service.

## Credential paths

```mermaid
flowchart LR
  Client -->|X-Api-Key| ApiKey[API-key middleware]
  Client -->|Basic header| Basic[Basic middleware]
  Client -->|Credentials| Issuer[Demonstration JWT endpoint]
  Issuer -->|Bearer token| Client
  Client -->|Bearer token| Jwt[JWT validation]
  Client -->|Firebase ID token| Firebase[Firebase Admin validation]
  Client -->|Client certificate| MTLS[TLS certificate validation]
  Client -->|Client credentials| Duende[Duende token endpoint]
  Duende -->|Access token| Client
```

## Protocol placement

- REST mechanisms inspect HTTP headers before controller/endpoint execution.
- gRPC API keys and bearer tokens travel as call metadata; mTLS authenticates at the TLS connection boundary.
- GraphQL authenticates the HTTP request, then authorization must still protect schema fields/resolvers and data scope.

## JWT sequence

```mermaid
sequenceDiagram
  actor Client
  participant Token as Demo token endpoint / external issuer
  participant Auth as Authentication middleware
  participant Operation as REST action, RPC, or resolver
  Client->>Token: Credentials or standards-based grant
  Token-->>Client: Signed access token
  Client->>Auth: Request + Bearer token
  Auth->>Auth: Validate signature, issuer, audience, lifetime
  alt valid and authorized
    Auth->>Operation: Authenticated principal
    Operation-->>Client: Result
  else invalid or unauthorized
    Auth-->>Client: Reject
  end
```

## Security invariants

TLS is required for reusable credentials. Authentication does not replace authorization. Production systems need managed secret/key storage, rotation, revocation/incident strategy, credential scoping, audit, rate limits, account controls, and safe error responses. Static users, symmetric local issuers, and development placeholders are teaching devices.

See the [authentication comparison](../../comparison-matrices/authentication-styles.md).
