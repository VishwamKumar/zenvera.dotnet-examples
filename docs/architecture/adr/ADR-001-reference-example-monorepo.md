# ADR-001: Use a reference-example monorepo

- Status: Accepted
- Date: 2026-07-12

## Context

Zenvera's .NET demonstrations cover REST, gRPC, GraphQL, SOAP, authentication, API gateways, infrastructure, Clean Architecture, and user-interface frameworks. The collection needs to make related examples easy to discover and compare while keeping each lesson independently understandable and runnable.

These repositories are educational references. They do not represent production bounded contexts and are not intended to be deployed together as one business system.

## Decision

Organize the examples in `zenvera.dotnet-examples` by learning category under `src`, with supporting `shared`, `tests`, `docs`, and `deploy/local` areas. Use one root `.slnx` as the canonical solution and independently openable category solutions under `solutions`.

Production bounded-context rules will not be applied literally. A small example may remain a single focused project or use a simple layered structure when that best exposes the API style, authentication mechanism, framework, or infrastructure concern being taught. Full Domain, Application, Infrastructure, host, and test separation is retained for examples whose subject is Clean Architecture.

Every example must remain independently runnable. It will keep its own host, configuration contract, prerequisites, and nearby run instructions. Shared projects are limited to proven neutral Todo or Weather contracts/persistence and test utilities; unrelated business behavior and example-specific authentication or infrastructure wiring will not be merged into shared libraries. Local orchestration may compose dependencies without turning the examples into one application.

The root solution provides one authoritative discovery and build entry point and supports repository-wide tooling. Category-based folders and category solutions preserve focused navigation and permit building subsets, including separation of platform-specific workloads such as MAUI.

## Consequences

### Positive

- Related technologies become easier to discover and compare.
- Repository, build, package, documentation, and CI conventions can be maintained once.
- Exact duplicate code can be consolidated selectively.
- One authoritative solution enables broad validation while category solutions keep common tasks focused.
- Full architecture references coexist with intentionally small examples without imposing unnecessary ceremony.

## 2026 amendment — independently openable category solutions

Six category `.slnx` files provide independently openable views of the repository. The root solution remains authoritative, and CI validates category membership so duplication stays visible.

### Trade-offs

- The root solution includes optional platform workloads and private-package prerequisites.
- Port, configuration, and dependency collisions require explicit local runbooks.
- Shared code must be reviewed carefully so abstraction does not hide the teaching point.

## Alternatives considered

### Keep every example in a separate repository

This preserves maximum isolation but retains duplicated maintenance, fragmented discovery, and configuration drift.

### Apply full Clean Architecture to every example

This would make structure uniform, but it would overwhelm small demonstrations and obscure the technology or pattern being compared.

### Organize by business bounded context

Todo and Weather are illustrative data shapes rather than production domains. Organizing around them would imply business ownership and coupling that the collection does not have.
